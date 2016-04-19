using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Net.Http;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;

namespace WeatherStationHeadless
{
    public sealed partial class StartupTask
    {

        /// <summary>
        /// Send the snapshot sensor data to the Web Application database using the HTTP POST Request protocol and an HttpClient
        /// </summary>
        /// <param name="message"></param>
        private async void sendMessage(float Altitude, float BarometricPressure, float CelsiusTemperature, float FahrenheitTemperature,
                                 float Humidity, float LightReading, int WindDirection, double WindSpeed, double PeakWindSpeed, double RainFall)
        {
            shield.GreenLEDPin.Write(Windows.Devices.Gpio.GpioPinValue.High);

            try
            {
                using (var client = new HttpClient())
                {
                    var values = new Dictionary<string, string>
                {
                   { "Alt", Altitude.ToString() },
                    {"Pressure", BarometricPressure.ToString() },
                    {"CTemp", CelsiusTemperature.ToString() },
                    {"FTemp", FahrenheitTemperature.ToString() },
                    {"Humidity", Humidity.ToString() },
                    {"Light", LightReading.ToString() },
                    {"WindDir", WindDirection.ToString() },
                    {"WindSpeed", WindSpeed.ToString() },
                    {"PeakWindSpeed", PeakWindSpeed.ToString() },
                    {"Rainfall", RainFall.ToString() }
                };

                    var content = new FormUrlEncodedContent(values);

                    var response = await client.PostAsync("  ", content); //Put URL here for POST target.

                    var responseString = await response.Content.ReadAsStringAsync();

                    if (!(response.IsSuccessStatusCode))
                    {
                        Debug.WriteLine("HTTP Post not successful: {0}", responseString);
                    }
                    else
                    {
                        Debug.WriteLine("HTTP POST was successful!");
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error: {0}", e.ToString());
            }

            shield.GreenLEDPin.Write(Windows.Devices.Gpio.GpioPinValue.Low);
        }

        /// <summary>
        /// ToJson function is used to convert sensor data into a JSON string to be sent to Azure Event Hub
        /// </summary>
        /// <returns>JSon String containing all info for sensor data</returns>
        public string ToJson()
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(WeatherData));
            MemoryStream ms = new MemoryStream();
            ser.WriteObject(ms, this);
            string json = Encoding.UTF8.GetString(ms.ToArray(), 0, (int)ms.Length);

            return json;
        }

        //Redundant code that utilises a MySql Database locally - kept for future project work
        //private MySqlConnection conn;
        // private string myConnectionString = "server=;user id=;password=;port=3306;sslmode=None;database=weatherdata;persistsecurityinfo=True;charset=utf8";

        //Redundant code to send to a local MySql database, kept for testing and future project.
        /*
        conn = null;

        try
        {
        conn = new MySqlConnection(myConnectionString);
        conn.Open();

            MySqlCommand command = new MySqlCommand();
            command.Connection = conn;
            command.CommandText = "INSERT INTO metReadings(readTime, altitude, baroPress, cTemp, fTemp, humid, lightReading, windDir, windSp, peakWindSp, rainFall) VALUES(@time, @Altitude, @BarometricPressure, @CelsiusTemperature, @FahrenheitTemperature, @Humidity, @LightReading, @WindDirection, @WindSpeed, @PeakWindSpeed, @RainFall)";
            command.Prepare();
            command.Parameters.AddWithValue("@time", time.DateTime);
            command.Parameters.AddWithValue("@Altitude", Altitude);
            command.Parameters.AddWithValue("@BarometricPressure", BarometricPressure);
            command.Parameters.AddWithValue("@CelsiusTemperature", CelsiusTemperature);
            command.Parameters.AddWithValue("@FahrenheitTemperature", FahrenheitTemperature);
            command.Parameters.AddWithValue("@Humidity", Humidity);
            command.Parameters.AddWithValue("@LightReading", LightReading);
            command.Parameters.AddWithValue("@WindDirection", WindDirection);
            command.Parameters.AddWithValue("@WindSpeed", WindSpeed);
            command.Parameters.AddWithValue("@PeakWindSpeed", PeakWindSpeed);
            command.Parameters.AddWithValue("@RainFall", RainFall);

            command.ExecuteNonQuery();
        }

        catch (MySqlException ex)
        {
            Debug.WriteLine("Error: {0}", ex.ToString());
        }
        catch (Exception e)
        {
            Debug.WriteLine("Error: {0}", e.ToString());
        }

        finally
        {
            if(conn != null)
            {
                conn.Close();
            }
        } */
    }

        /*
        /// <summary>
        /// Helper function to get SAS token for connecting to Azure Event Hub
        /// </summary>
        /// <returns></returns>
        private string SasTokenHelper()
        {
            int expiry = (int)DateTime.UtcNow.AddMinutes(20).Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            string stringToSign = UrlEncode(uri.ToString()) + "\n" + expiry.ToString();
            string signature = HmacSha256(localSettings.Key.ToString(), stringToSign);
            string token = string.Format("sr={0}&sig={1}&se={2}&skn={3}", UrlEncode(uri.ToString()), UrlEncode(signature), expiry, localSettings.KeyName.ToString());

            return token;
        }

        /// <summary>
        /// Because Windows.Security.Cryptography.Core.MacAlgorithmNames.HmacSha256 doesn't
        /// exist in WP8.1 context we need to do another implementation
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public string HmacSha256(string key, string kvalue)
        {
            var keyStrm = CryptographicBuffer.ConvertStringToBinary(key, BinaryStringEncoding.Utf8);
            var valueStrm = CryptographicBuffer.ConvertStringToBinary(kvalue, BinaryStringEncoding.Utf8);

            var objMacProv = MacAlgorithmProvider.OpenAlgorithm(MacAlgorithmNames.HmacSha256);
            var hash = objMacProv.CreateHash(keyStrm);
            hash.Append(valueStrm);

            return CryptographicBuffer.EncodeToBase64String(hash.GetValueAndReset());
        }
        */
       

    }

