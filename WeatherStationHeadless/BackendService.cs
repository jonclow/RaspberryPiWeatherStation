using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using Windows.Storage;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Windows.Devices.Gpio;

namespace WeatherStationHeadless
{
    public sealed partial class StartupTask
    {
        /// <summary>
        /// Send a datapoint to the API
        /// </summary>
        /// <param name="message"></param>
        private async void sendMessage(float alt, float baro, float ctemp, float ftemp, float humidity, long readTime, float light, int windDir, double wind, double peakWind, double rain)
        {
            shield.GreenLEDPin.Write(GpioPinValue.High);
            WeatherData wx = new WeatherData(alt, baro, ctemp, ftemp, humidity, readTime, light, windDir, wind, peakWind, rain);
            string requestPayload = ToJson(wx);

            try
            {
                using (var client = new HttpClient())
                {

                    var response = await client.PostAsync(
                        "http://api.redmercury.co.nz/weather/data", 
                        new StringContent(requestPayload, Encoding.UTF8, "application/json")
                        );

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

            shield.GreenLEDPin.Write(GpioPinValue.Low);
        }

        /// <summary>
        /// When the web cam is triggered by the threadpool timer, take the StorageFile image and send it to the website via HTTP Post.
        /// </summary>
        /// <param name="photo"></param>
        public async void sendPhoto(StorageFile photo)
        {
                try
                {
                using (var client = new HttpClient())
                {
                    // client.BaseAddress = new Uri("http://your.url.com/"); //use this if not using a web app with data controllers and independant php scripts
                    MultipartFormDataContent form = new MultipartFormDataContent();
                    HttpContent content = new StringContent("fileToUpload");

                    form.Add(content, "fileToUpload");
                    var stream = await photo.OpenStreamForReadAsync();
                    content = new StreamContent(stream);
                    content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                    {
                        Name = "fileToUpload",
                        FileName = photo.Name
                    };
                    form.Add(content);
                    var response = await client.PostAsync("http://www.redmercury.co.nz/writeimage/store", form);

                    var responseString = await response.Content.ReadAsStringAsync();

                    if (!(response.IsSuccessStatusCode))
                    {
                        Debug.WriteLine("HTTP Post of image stream not successful: {0}", responseString);
                    }
                    else
                    {
                        Debug.WriteLine("HTTP POST of image stream was successful!");
                    }
                }
                }
                catch (TaskCanceledException e)
                {
                    Debug.WriteLine("Error: {0}", e.ToString());
                }

                catch (Exception ex)
                {
                    Debug.WriteLine("Error: {0}", ex.ToString());
                }
        }

        /// <summary>
        /// ToJson function is used to convert sensor data into a JSON string to be sent to Azure Event Hub
        /// </summary>
        /// <returns>JSon String containing all info for sensor data</returns>
        public string ToJson(WeatherData obj)
         {
             DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(WeatherData));
             MemoryStream ms = new MemoryStream();
             ser.WriteObject(ms, obj);
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

