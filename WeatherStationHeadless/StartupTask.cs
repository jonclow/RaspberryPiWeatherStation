using System;
using System.Collections.Generic;
using System.Threading;
using Windows.ApplicationModel.Background;
using Windows.System.Threading;
using WeatherStationHeadless.Sparkfun;
using Windows.Devices.Gpio;
using System.Diagnostics;
using System.Linq;
using Windows.Storage;


// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace WeatherStationHeadless
{
    public sealed partial class StartupTask : IBackgroundTask
    {
        private readonly int i2cReadIntervalSeconds = 120;
        private ThreadPoolTimer i2cTimer;
        private readonly int windInterruptSampleInterval = 1;
        private ThreadPoolTimer windInterruptSample;
        private Mutex mutex;
        private string mutexId = "WeatherStation";
        private WeatherShield shield = new WeatherShield();
        private BackgroundTaskDeferral taskDeferral;
        private WeatherData weatherData = new WeatherData();
        //Hard coded altitude of the weather station for use in the correction to seal level pressure.
        private float locationAlt = 340;
        //For the weathershield, set to 5V even though powered from 3.3V
        private const float ReferenceVoltage = 5F;
        //ADC input line connected to light sensor
        private const byte ADCChannel = 0;
        //ADC input channel connected to Wind Direction magnetic reed switch on anemometer
        private const byte ADCWindDirChannel = 1;
        private MCP3008 mcp3008 = new MCP3008(ReferenceVoltage);

        private DateTimeOffset windReadTime;
        private DateTimeOffset windLastTime = DateTimeOffset.Now;
        private byte windClicks = 0;

        private DateTimeOffset rainReadTime;
        private DateTimeOffset rainLastTime = DateTimeOffset.Now;
        private double rainClicks = 0;

        //10 millisecond TimeSpan value to check against for bad switch data - bounces etc when sensors operating near maximum
        private TimeSpan evalutateWindSensorTime;
        private TimeSpan evalutateRainSensorTime;
        private TimeSpan badDataTime = new TimeSpan(0, 0, 0, 0, 10);

        //two lists to take 120 data points of wind speed and direction sampled each second via threadpool timer to then average for entry to data base.  Using generic list for extension functions
        //and also to prevent loss of data if threadpool timed actions dont line up and there are more than 120 data points.
        List<double> windSpeedList = new List<double>();
        List<int> windDirectionList = new List<int>();

        //Webcam 
        private WebCamHelper webcam = new WebCamHelper();
        private ThreadPoolTimer pictureTimer;
        private readonly int pictureTimerSeconds = 780;


        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            // Ensure our background task remains running
            taskDeferral = taskInstance.GetDeferral();

            // Mutex will be used to ensure only one thread at a time is talking to the shield / isolated storage
            mutex = new Mutex(false, mutexId);

            // Initialize WeatherShield
            await shield.BeginAsync();

            //Initialise the MCP3008 ADC Chip
            mcp3008.Initialize();

            //Initialise an attached web cam ready to take an image
            await webcam.InitialiseCamerAsync();

            // Create a timer-initiated ThreadPool task to read data from I2C
            i2cTimer = ThreadPoolTimer.CreatePeriodicTimer(PopulateWeatherData, TimeSpan.FromSeconds(i2cReadIntervalSeconds));

            //Create a timer-initiated ThreadPool task to read data from the interrupt handler counting the wind instrument activity
            windInterruptSample = ThreadPoolTimer.CreatePeriodicTimer(MeasureWindEventData, TimeSpan.FromSeconds(windInterruptSampleInterval));

            //Create a timer driven thread pool task to take a photo.
            pictureTimer = ThreadPoolTimer.CreatePeriodicTimer(TakePhoto, TimeSpan.FromSeconds(pictureTimerSeconds));

            // Task cancellation handler, release our deferral there 
            taskInstance.Canceled += OnCanceled;

            //Create the interrupt handler listening to the wind speed pin (13).  Triggers the GpioPin.ValueChanged event on that pin 
            //connected to the anemometer.
            shield.WindSpeedPin.ValueChanged += WindSpeedPin_ValueChanged;

            //Create the interrupt handler listening to the rain guage pin (26).  Triggers the Gpio.ValueChanged event on that pin
            //connected to the rain guage. 
            shield.RainPin.ValueChanged += RainPin_ValueChanged;
        }

        //Rain sensor interrupt logic
        private void RainPin_ValueChanged(GpioPin sender, GpioPinValueChangedEventArgs args)
        {
            if ((args.Edge.CompareTo(GpioPinEdge.FallingEdge) == 0))
            {
                rainReadTime = DateTimeOffset.Now;
                evalutateRainSensorTime = rainReadTime - rainLastTime;

                if ((evalutateRainSensorTime > badDataTime))//ignore switch bounce glitches less than 10ms
                {
                    rainLastTime = rainReadTime;  //reset the last read tie to evaluate if sensor information is good (more than 10 millisecond interval)
                    rainClicks += 0.2; //Update: Optical sensor set to 0.2mm per switch.  Legacy tipping bucket: each click represents 0.2794mm of rain (or 0.011 inches).  This total is added to the database every 2 min then zeroed.
                }
            }
        }

        //Anemometer interrup logic
        private void WindSpeedPin_ValueChanged(GpioPin sender, GpioPinValueChangedEventArgs args)
        {
            if ((args.Edge.CompareTo(GpioPinEdge.FallingEdge) == 0))
            {
                windReadTime = DateTimeOffset.Now;
                evalutateWindSensorTime = windReadTime - windLastTime;


                if (evalutateWindSensorTime > badDataTime) //ignore switch bounces of less than 10ms after the reed switch closes
                {
                    windLastTime = windReadTime;  //reset the last read tie to evaluate if sensor information is good (more than 10 millisecond interval)
                    windClicks++; //there is 1.492MPH for each click per second (or 1.2965kts, or 2.4011kph)

                }
            }
        }

        private void OnCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            // Relinquish our task deferral
            taskDeferral.Complete();
        }

        //Snapshot of weather data at the interval stipulated in the threadpool timer.  Calls send method which exports info to the 
        //website using HttpClient and POST protocol.
        private void PopulateWeatherData(ThreadPoolTimer timer)
        {
            bool hasMutex = false;

            try
            {
                hasMutex = mutex.WaitOne(1000);
                if (hasMutex)
                {
                    weatherData.TimeStamp = DateTimeOffset.Now;

                    shield.BlueLEDPin.Write(Windows.Devices.Gpio.GpioPinValue.High);

                    weatherData.Altitude = shield.Altitude;
                    weatherData.CelsiusTemperature = shield.Temperature;
                    weatherData.FahrenheitTemperature = (weatherData.CelsiusTemperature * 9 / 5) + 32;
                    weatherData.Humidity = shield.Humidity;
                    weatherData.WindSpeed = windSpeedList.Average();
                    weatherData.PeakWindSpeed = windSpeedList.Max();
                    weatherData.WindDirection = calcWindDirection();
                    weatherData.RainFall = rainClicks;
                    weatherData.BarometricPressure = weatherData.getSeaLevelPressure(shield.Pressure, weatherData.CelsiusTemperature, locationAlt);

                    int cdsReadVal = mcp3008.ReadADC(ADCChannel);
                    weatherData.LightSensorVoltage = mcp3008.ADCToVoltage(cdsReadVal);

                    shield.BlueLEDPin.Write(GpioPinValue.Low);

                    // Push the WeatherData to the website for use via HTTP Post request
                    sendMessage(weatherData.Altitude, weatherData.BarometricPressure, weatherData.CelsiusTemperature,
                        weatherData.FahrenheitTemperature, weatherData.Humidity, weatherData.LightSensorVoltage, weatherData.WindDirection, 
                        weatherData.WindSpeed, weatherData.PeakWindSpeed, weatherData.RainFall); 
                }
            }
            finally
            {
                //Clear the lists that get populated with a data point once per second for the next 2 min averaging
                windSpeedList.Clear();
                windDirectionList.Clear();
                //Zero the rainfall counter for next 2 min count.
                rainClicks = 0;

                if (hasMutex)
                {
                    mutex.ReleaseMutex();
                }
            }
        }

        //Method to read the wind vane every second and store this in a generic collection which is then averaged every two minutes and sent to the database.
        //Also takes the number of clicks off the anemometer interrupt in the second and converts to a wind speed in knots and adds this to a separate generic collection
        //which is averaged every two minutes for smoothed wind speed.  Peak or gust is taken as maximum in the list.
        private void MeasureWindEventData(ThreadPoolTimer windTimer)
        {
            bool hasMutex = false;

            try
            {
                hasMutex = mutex.WaitOne(1000);
                if (hasMutex)
                {
                    int tempWindDirection = 0;
                    double tempWindSpeed = 0;
                    int cdsReadVal2 = mcp3008.ReadADC(ADCWindDirChannel);

                    tempWindDirection = weatherData.getWindDirection(cdsReadVal2);
                    if (tempWindDirection != -1)
                    {
                        windDirectionList.Add(tempWindDirection);  //Add it to the list which gets averaged (circular average) every 120seconds
                    }
                    else
                    {
                        Debug.WriteLine("Error:  MCP3008 on Ch1 not OK.  Wind direction reading not working");
                    }

                    tempWindSpeed = windClicks * 1.2965;  //using data sheet of wind speed evaluated from events per second, converted to knots
                    windSpeedList.Add(tempWindSpeed);  //Add it to the list which will then get averaged every 120 seconds to smooth data

                    windClicks = 0;  //reset the interrupt counter on the wind speed pin.

                    //Note:  The rain gets summed every 120 seconds in the separate ThreadPoolTimer.
                }
            }
            finally
            {
                if (hasMutex)
                {
                    mutex.ReleaseMutex();
                }
            }

        }

        //Method that averages the wind direction data points taken each second, stored in a list.
        //Uses the Mitsuta method of averaging circular quantities (does not employ trig functions).
        private int calcWindDirection()
        {
            long sum = windDirectionList[0];
            int D = windDirectionList[0];
            int avgDirection = 0;

            for (int i = 1; i < windDirectionList.Count; i++)
            {
                int delta = windDirectionList[i] - D;
                if (delta < -180)
                {
                    D += delta + 360;
                }
                else if (delta > 180)
                {
                    D += delta - 360;
                }
                else
                {
                    D += delta;
                }

                sum += D;
            }
            avgDirection = (int)sum / windDirectionList.Count;
            if (avgDirection >= 360)
            {
                avgDirection -= 360;
            }
            else if (avgDirection < 0)
            {
                avgDirection += 360;
            }

            return avgDirection;
        }

        //Method to use the WebCamHelper object to take a photo.  Returns a storage file which is then passed to send method to HTTP POST to website.
        private async void TakePhoto(ThreadPoolTimer timer)
        {
            if (webcam.initialised)
            {
                try
                {
                    StorageFile photo = await webcam.CapturePhoto();
                    if (photo != null)
                    {
                        sendPhoto(photo);
                    }
                    else
                    {
                        Debug.WriteLine("Cannot take a photo.  The camera has not been initialised properly, or has failed.");
                    }
                }
                //If there has been an issue with the camera, the webcam object reference will be null.
                catch (NullReferenceException nre)
                {
                    webcam = new WebCamHelper();
                    await webcam.InitialiseCamerAsync();
                    Debug.WriteLine("Error: {0}", nre.ToString());
                }

                catch (Exception ex)
                {
                    webcam.Cleanup();
                    Debug.WriteLine("Error: {0}", ex.ToString());
                }
            } else
            {
                webcam = new WebCamHelper();
                await webcam.InitialiseCamerAsync();
            }
        }
        }
    }

