using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace Workshop.ImageClassificationLabSolution
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
            string endpoint = "https://northeurope.api.cognitive.microsoft.com/";
            string key = "<your cognitive services key";
            string resourceId = "<your cognitive services resource id";

            string projectName = "Car Assessment Solution";


            // Create the Api, passing in the training key and endpoint
            var trainingApi = new CustomVisionTrainingClient()
            {
                ApiKey = key,
                Endpoint = endpoint
            };

            // Create new project
            Console.WriteLine($"Creating project {projectName}");
            var project = trainingApi.CreateProject(projectName);

            // Make two tags in the new project
            var dentTag = trainingApi.CreateTag(project.Id, "Dent");
            var writeOffTag = trainingApi.CreateTag(project.Id, "WriteOff");

            // Add some images to the tags
            Console.WriteLine("\tUploading images");
            LoadImagesFromDisk();

            foreach (var image in dentImages)
            {
                trainingApi.CreateImagesFromData(project.Id, image, new List<Guid>() { dentTag.Id });
            }

            foreach (var image in writeOffImages)
            {
                trainingApi.CreateImagesFromData(project.Id, image, new List<Guid>() { writeOffTag.Id });
            }

            // Now there are images with tags start training the project
            Console.WriteLine("\tTraining");
            var iteration = trainingApi.TrainProject(project.Id);

            // The returned iteration will be in progress, and can be queried periodically to see when it has completed
            while (iteration.Status == "Training")
            {
                Thread.Sleep(1000);

                // Re-query the iteration to get it's updated status
                iteration = trainingApi.GetIteration(project.Id, iteration.Id);
            }



            // The iteration is now trained. Publish it to the prediction end point.
            var publishedModelName = "carAssessmentModel";
            trainingApi.PublishIteration(project.Id, iteration.Id, publishedModelName, resourceId);
            Console.WriteLine("Done!\n");

            var predictionApi = new CustomVisionPredictionClient()
            {
                ApiKey = key,
                Endpoint = endpoint
            };

            // Make a prediction against the new project
            Console.WriteLine("Making a prediction for WriteOff image:");
            var writeOffResult = predictionApi.ClassifyImage(project.Id, publishedModelName, writeOffTestImage);

            // Loop over each prediction and write out the results
            foreach (var c in writeOffResult.Predictions)
            {
                Console.WriteLine($"\t{c.TagName}: {c.Probability:P1}");
            }

            Console.WriteLine("Making a prediction for Dent image:");
            var dentResult = predictionApi.ClassifyImage(project.Id, publishedModelName, dentTestImage);

            // Loop over each prediction and write out the results
            foreach (var c in dentResult.Predictions)
            {
                Console.WriteLine($"\t{c.TagName}: {c.Probability:P1}");
            }
            Console.ReadKey();
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