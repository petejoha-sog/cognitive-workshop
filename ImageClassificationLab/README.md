# Lab: Creating Image Classification Applications using the Custom Vision API

In this lab, you will create an application that deals with the classification of plants.

In the Workshop.ImageClassificationLabStarter\Images folder are three folders:

- Dent
- WriteOff
- Test

The Dent and WriteOff folders contain images of that
will be classified by tagging and training the images. The Test folder contains an image that will be used to
perform the test prediction


## Prerequisites
This lab makes use of the Azure resource Custom Vision. You can create it through the Azure portal or on <https://customvision.ai>

## Step 0: The API keys, endpoints and resource id's

You also need to have a training and prediction API key. The training API key allows you to
create, manage, and train Custom Vision projects programatically. All operations
on <https://customvision.ai> are exposed through this library, allowing you to
automate all aspects of the Custom Vision Service. You can obtain a key by
accessing <https://customvision.ai> and then clicking on the
"setting" gear in the top right or through the Azure portal.

> Note: Internet Explorer is not supported. We recommend using Edge, Firefox, or Chrome.

### Step 1: Add your keys, endpoints and resource id into the Program.cs file

Find the following code and update it with your information.

```c#
string trainingEndpoint = "<your training endpoint url>";
string trainingKey = "<your training key>";

string predictionEndpoint = "<your prediction endpoint url>";
string predictionKey = "<your prediction key>";
string predictionResourceId = "<your prediction resource id>";
```

## Step 2: Create a Custom Vision Service project

To create a new Custom Vision Service project, add the following code in the body of the `Main()` method after the call to `new CustomVisionTrainingClient()`. Replace the _ with a call to the correct method.

```c#
// Create new project
Console.WriteLine($"Creating project {projectName}");
var project = trainingApi._(projectName);
```


## Step 3: Add tags to your project

To add tags to your project, insert the following code after the line where your create the Custom Vision Project.

```c#
// Make two tags in the new project
var dentTag = trainingApi._(project.Id, "Dent");
var writeOffTag = trainingApi._(project.Id, "WriteOff");

```

>Q. What method should you replace the _ with to create a tag?

## Step 4: Upload images to the project

To add the images we have in memory to the project, insert the following code
after the lines where your created your tags.

```c#
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
```

## Step 5: Train the project

Now that we have added tags and images to the project, we can train it. Insert
the following code after the end of code that you added in the prior step. This
creates the first iteration in the project. We can then publish the iteration to make predictions.



```c#
// Now there are images with tags start training the project
Console.WriteLine("\tTraining");
var iteration = trainingApi._(project.Id);

// The returned iteration will be in progress, and can be queried periodically to see when it has completed
while (iteration.Status == "Training")
{
    Thread.Sleep(1000);

    // Re-query the iteration to get it's updated status
    iteration = trainingApi.GetIteration(project.Id, iteration.Id);
}



// The iteration is now trained. Publish it to the prediction end point.
var publishedModelName = "carAssessmentModel";
trainingApi.PublishIteration(project.Id, iteration.Id, publishedModelName, predictionResourceId);
Console.WriteLine("Done!\n");
```

>Q. What method should you replace the _ with to train the project?

## Step 6: Get and use the default prediction endpoint

We are now ready to use the model for prediction. First we create the prediction client. Then we send test images to the project using our published model.
Insert the code after the training code you have just
entered.

```c#
var predictionApi = new CustomVisionPredictionClient()
{
    ApiKey = predictionKey,
    Endpoint = predictionEndpoint
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
```

## Step 8: Run the example

Build and run the solution. The training and prediction of the images can take 2 minutes.
The prediction results appear on the console.


### Further Reading

The source code for the Windows client library is available on
[github](https://github.com/Microsoft/Cognitive-CustomVision-Windows/).

This lab is based off of <https://github.com/Azure/LearnAI-Bootcamp>.