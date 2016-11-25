using System;
using Windows.Devices.Spi;
using Windows.Devices.Enumeration;
using System.Diagnostics;

namespace WeatherStationHeadless
{
    class MCP3008
    {
        //Constants for the SPI controller chip interface
        private SpiDevice mcp3008;
        const int SPI_CHIP_SELECT_LINE = 0; //SPI0 CS0 pin 24

        //ADC chip operational constants
        const byte MCP3008_SingleEnded = 0x08;
        const byte MCP3008_Differential = 0x00;

        //Used in voltage calculations from ADC units
        float ReferenceVoltage;
        public const uint Min = 0;
        public const uint Max = 1023;

        public MCP3008(float referenceVoltage)
        {
            //Store the refernce voltage value for later use in the voltage calculation
            ReferenceVoltage = referenceVoltage;
        }

        ///<summary>
        ///Used to configure the RPi2 to communicate over the SPI bus to the MCP3008 ADC chip.
        /// </summary>
        /// 

        public async void Initialize()
        {
            try
            {
                //Setup the SPI bus configuration
                var settings = new SpiConnectionSettings(SPI_CHIP_SELECT_LINE);

                //3.6MHz is the rated speed of the MCP3008 at 5V
                settings.ClockFrequency = 3600000;
                settings.Mode = SpiMode.Mode0;

                //Ask Windows for the list of SPI Devices
                //Get a selector string that will return all SPI Controllers on the system
                string aqs = SpiDevice.GetDeviceSelector();

                //Find the SPI bus controller devices with our selector string
                var dis = await DeviceInformation.FindAllAsync(aqs);

                //Create an SPI device with our bus controller and SPI settings
                mcp3008 = await SpiDevice.FromIdAsync(dis[0].Id, settings);

                if (mcp3008 == null)
                {
                   // Debug.WriteLine("SPI controller {0} is currently in use by another application");
                    return;
                }
            }

            catch (Exception e)
            {
                Debug.WriteLine("Exception: " + e.Message + "\n" + e.StackTrace);
                throw;
            }
            
        }

        ///<summary>
        ///This method performs the work of communicationg over the SPI bus with the chip.
        /// To line up for reading back on byte boundary we will pad the command start bit with 7 leading '0' bits.
        /// Refer to the MCP3008 datasheet for message format
        /// 
        /// Write 0000 000S GDDD xxxx xxxx xxxx
        /// Read ???? ???? ???? ?N98 7654 3210
        /// S = start bit
        /// G = single / differential
        /// D = channel data
        /// ? = undefined, ignore
        /// N = 0 'null bit'
        /// 9-0 = 10 data bits
        /// </summary>
        public int ReadADC(byte whichChannel)
        {
            byte command = whichChannel;
            command |= MCP3008_SingleEnded;
            command <<= 4;

            byte[] commandBuf = new byte[] { 0x01, command, 0x00 };
            byte[] readBuf = new byte[] { 0x00, 0x00, 0x00 };

            mcp3008.TransferFullDuplex(commandBuf, readBuf);

            int sample = readBuf[2] + ((readBuf[1] & 0x03) << 8);
            int s2 = sample & 0x3FF;
            Debug.Assert(sample == s2);

            return sample;
        }
        
        ///<summary>
        ///Returns the ADC value as a float voltage based on the configured reference voltage
        /// </summary>
        /// <param name="adc">The ADC value to convert</param>
        /// <returns>Returns the computed voltage based on the reference voltage</returns>
        public float ADCToVoltage(int adc)
        {
            return (float)adc * ReferenceVoltage / (float)Max;
        }
    }
}
