using FoodstoreApi.Web.Authorization;
using FoodstoreApi.Web.Extensions;
using FoodstoreApi.Usecase.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FoodstoreApi.Web.ApiResponse;

namespace FoodstoreApi.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MediaController : ControllerBase
{
    private readonly IMediaService _mediaService;
    private readonly IEmployeeRepository _employeeRepo;
    private readonly ICustomerRepository _customerRepo;

    public MediaController(IMediaService mediaService, IEmployeeRepository employeeRepo, ICustomerRepository customerRepo)
    {
        _mediaService = mediaService;
        _employeeRepo = employeeRepo;
        _customerRepo = customerRepo;
    }

    [HttpPost("upload")]
    [RequirePermission("media.upload")]
    public async Task<IActionResult> UploadFile(IFormFile file, [FromForm] string? folder = "blog")
    {
        if (file == null || file.Length == 0)
            return ApiResult.BadRequest("No file uploaded");

        var userId = User.GetUserId();

        var employee = await _employeeRepo.GetByUserIdAsync(userId);
        if (employee != null)
        {
            using var stream = file.OpenReadStream();
            var media = await _mediaService.UploadFileAsync(stream, file.FileName, file.ContentType, file.Length, employee.Id, folder ?? "blog");
            return ApiResult.Success(media);
        }

        var customer = await _customerRepo.GetByUserIdAsync(userId);
        if (customer != null)
        {
            using var stream = file.OpenReadStream();
            var media = await _mediaService.UploadFileForCustomerAsync(stream, file.FileName, file.ContentType, file.Length, customer.Id, folder ?? "blog");
            return ApiResult.Success(media);
        }

        return ApiResult.BadRequest("User profile not found");
    }

    [HttpGet]
    [RequirePermission("system.view")]
    public async Task<IActionResult> GetAllMedia()
    {
        var media = await _mediaService.GetAllMediaAsync();
        return ApiResult.Success(media);
    }

    [HttpDelete("{id:guid}")]
    [RequirePermission("media.delete")]
    public async Task<IActionResult> DeleteMedia(Guid id)
    {
        var result = await _mediaService.DeleteMediaAsync(id);
        if (!result) return ApiResult.NotFound();
        return NoContent();
    }

    [HttpGet("check-usage")]
    [RequirePermission("media.delete")]
    public async Task<IActionResult> CheckUsage([FromQuery] string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return ApiResult.BadRequest("URL is required");

        var result = await _mediaService.CheckImageUsageAsync(url);
        return ApiResult.Success(result);
    }

    [HttpGet("unused")]
    [RequirePermission("media.delete")]
    public async Task<IActionResult> GetUnusedImages()
    {
        var allMedia = await _mediaService.GetAllMediaAsync();
        var usedUrls = await _mediaService.GetAllUsedImageUrlsAsync();

        var unusedMedia = allMedia.Where(m =>
            !usedUrls.Any(u => m.FileUrl.Contains(u, StringComparison.OrdinalIgnoreCase) ||
                               u.Contains(m.FileUrl, StringComparison.OrdinalIgnoreCase))
        ).ToList();

        return ApiResult.Success(new
        {
            totalMedia = allMedia.Count,
            usedCount = allMedia.Count - unusedMedia.Count,
            unusedCount = unusedMedia.Count,
            unusedMedia
        });
    }

    [HttpDelete("cleanup")]
    [RequirePermission("media.delete")]
    public async Task<IActionResult> CleanupUnusedImages()
    {
        var allMedia = await _mediaService.GetAllMediaAsync();
        var usedUrls = await _mediaService.GetAllUsedImageUrlsAsync();

        var unusedMedia = allMedia.Where(m =>
            !usedUrls.Any(u => m.FileUrl.Contains(u, StringComparison.OrdinalIgnoreCase) ||
                               u.Contains(m.FileUrl, StringComparison.OrdinalIgnoreCase))
        ).ToList();

        var deletedCount = 0;
        var errors = new List<string>();

        foreach (var media in unusedMedia)
        {
            try
            {
                var result = await _mediaService.DeleteMediaAsync(media.Id);
                if (result) deletedCount++;
            }
            catch (Exception ex)
            {
                errors.Add($"Failed to delete {media.FileName}: {ex.Message}");
            }
        }

        return ApiResult.Success(new
        {
            message = $"Deleted {deletedCount} unused photos.",
            deletedCount,
            errors
        });
    }
}
