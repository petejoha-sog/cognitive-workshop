using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace Workshop.ImageClassificationLabStarter
{

    class Program
    {
        private static List<MemoryStream> dentImages;

        private static List<MemoryStream> writeOffImages;

        private static MemoryStream writeOffTestImage;
        private static MemoryStream dentTestImage;

        static void Main(string[] args)
        {
            // Add your endpoint and keys from the settings page of the customvision.ai portal 
            string trainingEndpoint = "<your training endpoint url>";
            string trainingKey = "<your training key>";

            string predictionEndpoint = "<your prediction endpoint url>";
            string predictionKey = "<your prediction key>";
            string predictionResourceId = "<your prediction resource id>";

            string projectName = "Car Assessment";


            // Create the Api, passing in the training key and endpoint
            var trainingApi = new CustomVisionTrainingClient()
            {
                ApiKey = trainingKey,
                Endpoint = trainingEndpoint
            };

            
        }

        private static void LoadImagesFromDisk()
        {
            var projectDir = AppContext.BaseDirectory.Substring(0, AppContext.BaseDirectory.IndexOf("bin"));
            var imagesDir = Path.GetFullPath(Path.Combine(projectDir, "Images"));

            // this loads the images to be uploaded from disk into memory
            dentImages = Directory.GetFiles(Path.Combine(imagesDir, "Dent")).Select(f => new MemoryStream(File.ReadAllBytes(f))).ToList();
            writeOffImages = Directory.GetFiles(Path.Combine(imagesDir, "WriteOff")).Select(f => new MemoryStream(File.ReadAllBytes(f))).ToList();
            writeOffTestImage = new MemoryStream(File.ReadAllBytes(Path.Combine(imagesDir, "Test", "WriteOffTest.jpg")));
            dentTestImage = new MemoryStream(File.ReadAllBytes(Path.Combine(imagesDir, "Test", "DentTest.jpg")));

        }


    }
}