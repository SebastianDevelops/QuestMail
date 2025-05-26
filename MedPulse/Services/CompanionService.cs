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
        _pinataClient = new PinataClient(_settings.Pinata.JWT);
        _unitOfWork = unitOfWork;
    }

    public async Task<string> GetOrSetCompanionImageUrlResponseAsync()
    {
        var companion = await _unitOfWork.Companions.GetByIdAsync(Context.CompanionId);
        
        if (String.IsNullOrEmpty(companion.ImageUrl))
        {
            var imageUrl = await UploadImageToPinataAsync();
            companion.ImageUrl = imageUrl;
            await _unitOfWork.Companions.UpdateAsync(companion);
            return imageUrl;
        }
        
        return companion.ImageUrl;
    }
    
    private async Task<string> UploadImageToPinataAsync()
    {
        
        var googleAi = new GoogleAi(_settings.GoogleGemini.Apikey);
        var imageModel = googleAi.CreateImageModel(_settings.GoogleGemini.Model);
        var response = await imageModel.GenerateImagesAsync(Constants.ImageGenPrompt);
        var image = response?.Predictions?.FirstOrDefault();
        var imageBytes = image?.BytesBase64Encoded;
        
        using (var ms = new MemoryStream(Convert.FromBase64String(imageBytes)))
        {
            var pinataResponse = await _pinataClient.UploadFileAsync(ms, $"{Context.CompanionName}.png");
            Task.Delay(2500).Wait();
            return $"{_settings.Pinata.BaseUrl}{pinataResponse.Data.Cid}";
        }
    }
}