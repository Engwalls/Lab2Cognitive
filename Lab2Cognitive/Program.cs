using Microsoft.Extensions.Configuration;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;


// URLs to try
//https://www.gstatic.com/webp/gallery/1.jpg
//https://www.gstatic.com/webp/gallery/2.jpg
//https://www.gstatic.com/webp/gallery/3.jpg
//https://via.placeholder.com/200
//https://via.placeholder.com/300
//https://via.placeholder.com/400


class Program
{
    private static ComputerVisionClient cvClient;

    static async Task Main(string[] args)
    {
        // Read configuration from appsettings.json
        IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        string apiKey = configuration["AzureCognitiveServices:ApiKey"];
        string endpoint = configuration["AzureCognitiveServices:Endpoint"];

        // Create an instance of Computer Vision Client with API key and endpoint
        cvClient = new ComputerVisionClient(new ApiKeyServiceClientCredentials(apiKey))
        {
            Endpoint = endpoint
        };

        // Allow the user to input a local file path or an image URL
        Console.WriteLine("Enter a local file path or an image URL:");
        string userInput = Console.ReadLine();

        // Validate user input
        if (Uri.IsWellFormedUriString(userInput, UriKind.Absolute))
        {
            // The user entered a valid URL, retrieve the image and analyze it
            using (var httpClient = new HttpClient())
            {
                var imageStream = await httpClient.GetStreamAsync(userInput);
                AnalyzeImage(cvClient, imageStream);

                // Call GetThumbnail to generate and save a thumbnail
                await GenerateThumbNail(userInput);
            }
        }
        else if (File.Exists(userInput))
        {
            // The user entered a valid local file path, analyze the image
            using (var stream = File.OpenRead(userInput))
            {
                AnalyzeImage(cvClient, stream);

                // Call GetThumbnail to generate and save a thumbnail
                await GenerateThumbNailFromLocalFile(userInput);
            }
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a valid local file path or image URL.");
        }
    }

    static void AnalyzeImage(ComputerVisionClient client, Stream imageStream)
    {
        IList<VisualFeatureTypes?> features = new List<VisualFeatureTypes?>
    {
        VisualFeatureTypes.Description,
        VisualFeatureTypes.Tags,
        VisualFeatureTypes.Categories,
        VisualFeatureTypes.Color,
        VisualFeatureTypes.Objects,
        VisualFeatureTypes.Adult
    };

        var result = client.AnalyzeImageInStreamAsync(imageStream, features).Result;

        // Description
        Console.WriteLine();
        Console.WriteLine("Description: " + result.Description.Captions[0].Text);
        Console.WriteLine();

        // Tags
        Console.WriteLine("Tags: " + string.Join(", ", result.Tags.Select(tag => tag.Name)));
        Console.WriteLine();

        // Categories
        Console.WriteLine("Categories: " + string.Join(", ", result.Categories.Select(tag => tag.Name)));
        Console.WriteLine();

        // Colors
        Console.WriteLine("Colors: " + string.Join(", ", result.Color.DominantColors));
        Console.WriteLine();

        // Objects
        foreach (var obj in result.Objects)
        {
            Console.WriteLine($"Objects: {obj.ObjectProperty}");
        }
        Console.WriteLine();

        // Adult Content?
        Console.WriteLine($"Adult Content?: {result.Adult.IsAdultContent}");
        Console.WriteLine();
    }




    static async Task GenerateThumbNail(string userInput)
    {
        try
        {
            Console.WriteLine("Generating thumbnail....");
            Console.WriteLine();

            // Anropa GenerateThumbnailAsync med bild-URL
            var thumbnailStream = await cvClient.GenerateThumbnailAsync(100, 100, userInput, true);

            // Ange den absoluta sökvägen där du vill spara thumbnail
            string thumbnailFileName = Path.Combine(Directory.GetCurrentDirectory(), "thumbnail.png");

            using (Stream thumbnailFile = File.Create(thumbnailFileName))
            {
                await thumbnailStream.CopyToAsync(thumbnailFile);
            }

            Console.WriteLine($"Thumbnail saved in {thumbnailFileName}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error generating thumbnail: {ex.Message}");
        }
    }


    static async Task GenerateThumbNailFromLocalFile(string filePath)
    {
        try
        {
            Console.WriteLine("Generating thumbnail from a local file....");

            // Read the local image file
            using (var fileStream = File.OpenRead(filePath))
            {
                // Call GenerateThumbnailInStreamAsync to generate a thumbnail
                var thumbnailStream = await cvClient.GenerateThumbnailInStreamAsync(100, 100, fileStream, true);

                string thumbnailFileName = "thumbnail.png";

                using (Stream thumbnailFile = File.Create(thumbnailFileName))
                {
                    await thumbnailStream.CopyToAsync(thumbnailFile);
                }

                Console.WriteLine($"Thumbnail saved in {thumbnailFileName}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error generating the thumbnail: {ex.Message}");
        }
    }
}
