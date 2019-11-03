using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace ObjectDetectionStarter
{
    class Program
    {

        static void Main(string[] args)
        {
            // Add your Azure Custom Vision endpoint to your environment variables.
            // <snippet_endpoint>
            string ENDPOINT = "<your cognitive services endpoint url>";
            // </snippet_endpoint>

            // <snippet_keys>
            // Add your training & prediction key from the settings page of the portal
            string trainingKey = "<your cognetive services key>";
            string predictionKey = trainingKey;
            // </snippet_keys>

            // Create the Api, passing in the training key
            CustomVisionTrainingClient trainingApi = new CustomVisionTrainingClient()
            {
                ApiKey = trainingKey,
                Endpoint = ENDPOINT
            };

            // <snippet_create>
            // Find the object detection domain


            // </snippet_create>

            // <snippet_tags>
            // Make two tags in the new project

            // </snippet_tags>

            // <snippet_upload_regions>
            // Create dictionary of files and their bounding boxes.

            // </snippet_upload_regions>

            // <snippet_upload>
            // Add all images for fork

            // </snippet_upload>

            // <snippet_train>
            // Now there are images with tags start training the project

            // </snippet_train>

            // <snippet_publish>
            // The iteration is now trained. Publish it to the prediction end point.

            // </snippet_publish>

            // Now there is a trained endpoint, it can be used to make a prediction

            // <snippet_prediction_endpoint>
            // Create a prediction endpoint, passing in the obtained prediction key
            CustomVisionPredictionClient endpoint = new CustomVisionPredictionClient()
            {
                ApiKey = trainingKey,
                Endpoint = ENDPOINT
            };
            // </snippet_prediction_endpoint>

            // <snippet_prediction>
            // Make a prediction against the new project

            // </snippet_prediction>
        }
    }
}
