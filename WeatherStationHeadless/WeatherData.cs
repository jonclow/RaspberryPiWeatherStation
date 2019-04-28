using System;
using System.Runtime.Serialization;

namespace WeatherStationHeadless
{
    [DataContract]
    public sealed class WeatherData
    {
        public WeatherData(float alt, float baro, float degc, float degf, float humid, long time, float light, int windDir, double windSp, double pkWindSp, double rain)
        {
            altitude = alt;
            barometricPressure = baro;
            celsiusTemperature = degc;
            fahrenheitTemperature = degf;
            humidity = humid;
            readTime = time;
            lightSensorVoltage = light;
            windDirection = windDir;
            windSpeed = windSp;
            peakWindSpeed = pkWindSp;
            rainFall = rain;
        }

        [DataMember]
        public float altitude { get; set; }

        [DataMember]
        public float barometricPressure { get; set; }

        [DataMember]
        public float celsiusTemperature { get; set; }

        [DataMember]
        public float fahrenheitTemperature { get; set; }

        [DataMember]
        public float humidity { get; set; }

        [DataMember]
        public long readTime { get; set; }

        [DataMember]
        public float lightSensorVoltage { get; set; }

        [DataMember]
        public int windDirection { get; set; }

        [DataMember]
        public double windSpeed { get; set; }

        [DataMember]
        public double peakWindSpeed { get; set; }

        [DataMember]
        public double rainFall { get; set; }

        //Conversion of the local barometric pressure to sea level pressure.  
        public static float getSeaLevelPressure(float baroPress, float temp, float alt)
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
        public static int getWindDirection(int analogRead)
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
