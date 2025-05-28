using MedPulse.Infrastructure;
using PinataNET;
using GenerativeAI;
using MedPulse.Repositories;
using MedPulse.ViewModel;


namespace MedPulse.Services;

public class CompanionService : ICompanionService
{
    private readonly PinataClient _pinataClient;
    private readonly Settings _settings;
    private readonly IUnitOfWork _unitOfWork;

    public CompanionService(Settings settings, IUnitOfWork unitOfWork)
    {
        _settings = settings;
        _pinataClient = new PinataClient( Environment.GetEnvironmentVariable("Settings.Pinata.JWT"));
        _unitOfWork = unitOfWork;
    }


    public async Task<string> GetCompanionImageBase64()
    {
        var imageUrl = await GetOrSetCompanionImageUrlResponseAsync();
        
        using (var httpClient = new HttpClient())
        {
            var imageBytes = await httpClient.GetByteArrayAsync(imageUrl);
            return Convert.ToBase64String(imageBytes);
        }
    }

    
    private async Task<string> GetOrSetCompanionImageUrlResponseAsync()
    {
        var companion = await _unitOfWork.Companions.GetByIdAsync(Context.CompanionId);
        Console.WriteLine("companion image url: "+ companion.ImageUrl);
        if (String.IsNullOrEmpty(companion.ImageUrl))
        {
            var companionChar = companion.Name.Substring(0);
            var imageUrl = $"https://placehold.co/60x60/e0ac69/1a1a2e?text={companionChar}";
            companion.ImageUrl = imageUrl;
            await _unitOfWork.Companions.UpdateAsync(companion);
            return imageUrl;
        }
        
        return companion.ImageUrl;
    }
    
    // private async Task<string> UploadImageToPinataAsync()
    // {
    //     Console.WriteLine("Starting image generation...");
    //     var googleAi = new GoogleAi( Environment.GetEnvironmentVariable("Settings.GoogleGemini.apikey"));
    //     Console.WriteLine("Google AI initialized.");
    //     var imageModel = googleAi.CreateImageModel(Environment.GetEnvironmentVariable("Settings.GoogleGemini.model"));
    //     Console.WriteLine("Image model created.");
    //     var response = await imageModel.GenerateImagesAsync(Constants.ImageGenPrompt);
    //     Console.WriteLine("Image generation completed.");
    //     var image = response?.Predictions?.FirstOrDefault();
    //     Console.WriteLine("Image retrieved from response.");
    //     var imageBytes = image?.BytesBase64Encoded;
    //     Console.WriteLine("Image bytes retrieved.");
    //     
    //     using (var ms = new MemoryStream(Convert.FromBase64String(imageBytes)))
    //     {
    //         var pinataResponse = await _pinataClient.UploadFileAsync(ms, $"{Context.CompanionName}.png");
    //         Console.WriteLine("Image uploaded to Pinata.");
    //         Task.Delay(2500).Wait();
    //         return $"{Environment.GetEnvironmentVariable("Settings.Pinata.BaseUrl")}{pinataResponse.Data.Cid}";
    //     }
    // }
}