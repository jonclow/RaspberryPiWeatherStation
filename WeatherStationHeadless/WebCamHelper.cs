using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage;
using System.Diagnostics;

namespace WeatherStationHeadless
{
    class WebCamHelper
    {
        public MediaCapture mediaCapture;
        private bool initialised = false;

        /// <summary>
        /// Asynchronously looks for and initialises an attached Webcamera.  Allows access to live feed and photo capture.
        /// </summary>
        public async Task InitialiseCamerAsync()
        {
            if (mediaCapture == null)
            {
                //Attempt to locate an attached web camera
                var cameraDevice = await FindCameraDevice();

                if (cameraDevice == null)
                {
                    //No camera found - report error and stop init.
                    Debug.WriteLine("No camera found - check connections and if model is supported");
                    initialised = false;
                    return;
                }

                //Otherwise camera is found.  Create initialisation settings with the webcam device.
                var settings = new MediaCaptureInitializationSettings { VideoDeviceId = cameraDevice.Id };
                mediaCapture = new MediaCapture();
                await mediaCapture.InitializeAsync(settings);
                initialised = true;
            }



        }

        ///<summary>
        ///Asynchrounously looks for and returns the first camera device found.
        ///If no device is found, NULL is returned.
        /// </summary>
        private static async Task<DeviceInformation> FindCameraDevice()
        {
            //Get the available web cameras.
            var allVideoDevices = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);

            if (allVideoDevices.Count > 0)
            {
                //There is one or more found.  Return the first.
                return allVideoDevices[0];
            }
            else
            {
                //No devices found.  Return NULL.
                return null;
            }
        }

        ///<summary>
        ///Async begin live feed from the webcam
        /// </summary>
        public async Task StartCameraPreview()
        {
            try
            {
                await mediaCapture.StartPreviewAsync();
            }
            catch
            {
                initialised = false;
                Debug.WriteLine("Failed to start the camera preview stram");
            }
        }

        ///<summary>
        ///Async stop live feed from the webcam
        /// </summary>
        public async Task StopCameraPreview()
        {
            try
            {
                await mediaCapture.StopPreviewAsync();
            }
            catch
            {
                initialised = false;
                Debug.WriteLine("Failed to stop the camera preview stram");
            }
        }

        ///<summary>
        ///Async capture photo from the camera feed and store it in local storage.  Returns image file as a storage file.
        ///File is stored in a temporary folder and could be deleted by the system at any time.
        /// </summary>
        public async Task<StorageFile> CapturePhoto()
        {
            //Create storage file in local app storage
            string fileName = GenerateNewFileName() + ".jpg";
            CreationCollisionOption collisionOption = CreationCollisionOption.GenerateUniqueName;
            StorageFile file = await ApplicationData.Current.TemporaryFolder.CreateFileAsync(fileName, collisionOption);

            //Capture and store an image file as jpeg
            await mediaCapture.CapturePhotoToStorageFileAsync(ImageEncodingProperties.CreateJpeg(), file);

            //Return the image file for use
            Debug.WriteLine("Image taken" + fileName);
            return file;
        }

        ///<summary>
        ///Generate a unique file name based on the current time and date.  Return the file name as a string for use in image capture.
        /// </summary>
        private string GenerateNewFileName()
        {
            return DateTime.UtcNow.ToString("HH-mm-ss") + "_WeatherStation_Scene";
        }

        ///<summary>
        ///Web cam init check.  Return true if the webcam has been successfully initialised.
        /// </summary>
        public bool IsInitialised()
        {
            return initialised;
        }
    }


}

