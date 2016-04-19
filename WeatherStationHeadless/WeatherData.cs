using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;
using System.Xml.Serialization;


namespace WeatherStationHeadless
{
    public sealed class WeatherData
    {
        public WeatherData()
       {
        //    TimeStamp = DateTimeOffset.Now;
       }
  
        public float Altitude { get; set; }
        public float BarometricPressure { get; set; }
        public float CelsiusTemperature { get; set; }
        public float FahrenheitTemperature { get; set; }
        public float Humidity { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
        public float LightSensorVoltage { get; set; }
        public int WindDirection { get; set; }
        public double WindSpeed { get; set; }
        public double PeakWindSpeed { get; set; }
        public double RainFall { get; set; }

        public string JSON
        {
            get
            {
                var jsonSerializer = new DataContractJsonSerializer(typeof(WeatherData));
                using (MemoryStream strm = new MemoryStream())
                {
                    jsonSerializer.WriteObject(strm, this);
                    byte[] buf = strm.ToArray();
                    return Encoding.UTF8.GetString(buf, 0, buf.Length);
                }
            }
        }

        public string XML
        {
            get
            {
                var xmlserializer = new XmlSerializer(typeof(WeatherData));
                var stringWriter = new StringWriter();
                using (var writer = XmlWriter.Create(stringWriter))
                {
                    xmlserializer.Serialize(writer, this, new XmlSerializerNamespaces());
                    return stringWriter.ToString();
                }
            }
        }

        public string HTML
        {
            get
            {
                return string.Format(@"<html><head><title>My Weather Station</title></head><body>
                                    Time:{0}<br />
                                    Temperature (C/F): {1:N2}/{2:N2}<br />
                                    Barometric Pressure (kPa): {3:N4}<br />
                                    Relative Humidity (%): {4:N2}<br /></body></html>",
                                    TimeStamp, CelsiusTemperature, FahrenheitTemperature, (BarometricPressure / 1000), Humidity);
            }
        }

        //Conversion of the local barometric pressure to sea level pressure.  
        public float getSeaLevelPressure(float baroPress, float temp, float alt)
        {
            float pressure;
            double bottomLine;
            double altConvert;
            double interimResult;
            double _interimResult;
            double _power;

            altConvert = 0.0065 * alt;
            bottomLine = temp + altConvert + 273.15;
            interimResult = 1 - (altConvert / bottomLine);
            _interimResult = Math.Abs(interimResult);
            _power = Math.Abs(-5.257);
            _interimResult = Math.Pow(_interimResult, _power);
            interimResult = (1 / _interimResult);
            interimResult = interimResult * (baroPress / 100);
            pressure = (float)interimResult;

            return pressure;
        }

        //Method which takes analog wind vane reading and converts to bearing.
        public int getWindDirection(int analogRead)
        {
            int directionTrue = 0;
            
            if (analogRead < 380)
            {
                directionTrue = 113;
                return directionTrue;
            }
            else if(analogRead < 393)
            {
                directionTrue = 68;
                return directionTrue;
            }
            else if (analogRead < 414)
            {
                directionTrue = 90;
                return directionTrue;
            }
            else if (analogRead < 456)
            {
                directionTrue = 158;
                return directionTrue;
            }
            else if (analogRead < 508)
            {
                directionTrue = 135;
                return directionTrue;
            }
            else if (analogRead < 551)
            {
                directionTrue = 203;
                return directionTrue;
            }
            else if (analogRead < 615)
            {
                directionTrue = 180;
                return directionTrue;
            }
            else if (analogRead < 680)
            {
                directionTrue = 23;
                return directionTrue;
            }
            else if (analogRead < 746)
            {
                directionTrue = 45;
                return directionTrue;
            }
            else if (analogRead < 801)
            {
                directionTrue = 248;
                return directionTrue;
            }
            else if (analogRead < 833)
            {
                directionTrue = 225;
                return directionTrue;
            }
            else if (analogRead < 878)
            {
                directionTrue = 338;
                return directionTrue;
            }
            else if (analogRead < 913)
            {
                directionTrue = 0;
                return directionTrue;
            }
            else if (analogRead < 940)
            {
                directionTrue = 293;
                return directionTrue;
            }
            else if (analogRead < 967)
            {
                directionTrue = 315;
                return directionTrue;
            }
            else if (analogRead < 990)
            {
                directionTrue = 270;
                return directionTrue;
            }
            else
            {
                return -1;
            }
        }

      }
}
