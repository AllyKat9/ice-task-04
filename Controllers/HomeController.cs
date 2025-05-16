using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using cldv_ice_task_04.Models;
using cldv_ice_task_04.Services;

namespace cldv_ice_task_04.Controllers;

public class HomeController : Controller
{
    private readonly AzureBlobService _blobService;
    private static List<string> _uploadedUrls = new(); // In-memory store

    public HomeController(IConfiguration configuration)
    {
        var connectionString = configuration["AzureBlobStorage:ConnectionString"];
        var containerName = configuration["AzureBlobStorage:ContainerName"];
        _blobService = new AzureBlobService(connectionString, containerName);
    }

    [HttpGet]
    public IActionResult Index()
    {
        ViewBag.UploadedImages = _uploadedUrls;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return RedirectToAction("Index");

        var blobUrl = await _blobService.UploadFileAsync(file);
        _uploadedUrls.Add(blobUrl);

        return RedirectToAction("Index");
    }
}

