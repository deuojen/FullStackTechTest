using System.Diagnostics;
using DAL;
using Microsoft.AspNetCore.Mvc;
using FullStackTechTest.Models.ImportData;
using FullStackTechTest.Models.Shared;
using System.Text;
using Newtonsoft.Json;
using Models;

namespace FullStackTechTest.Controllers;

public class ImportDataController : Controller
{
    private readonly ILogger<ImportDataController> _logger;
    private readonly IPersonRepository _personRepository;
    private readonly IImportDataRepository _importDataRepository;

    public ImportDataController(ILogger<ImportDataController> logger, IPersonRepository personRepository, IImportDataRepository importDataRepository)
    {
        _logger = logger;
        _personRepository = personRepository;
        _importDataRepository = importDataRepository;
    }

    public IActionResult Index()
    {
        var model = new IndexViewModel();
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Index(IFormFile postedFile)
    {
        var model = new IndexViewModel();
        if (postedFile == null || postedFile.Length == 0)
        {
            ModelState.AddModelError("UploadFile", "No file selected for upload!");
            return View(model);
        }

        string contentType = postedFile.ContentType;
        if (contentType != "application/json")
        {
            ModelState.AddModelError("UploadFile", "File format is not suitable! Please upload valid json file.");
            return View(model);
        }

        var result = new StringBuilder();
        using (var reader = new StreamReader(postedFile.OpenReadStream()))
        {
            while (reader.Peek() >= 0)
                result.AppendLine(await reader.ReadLineAsync());
        }

        var persons = JsonConvert.DeserializeObject<List<PersonWithAddress>>(result.ToString());

        model = await IndexViewModel.InsertAsync(_importDataRepository, persons);

        ViewData["Message"] = $"File successfully processed. Duplicate data ignored and {model.InsertPeopleCount} of {persons.Count} people imported.";

        return View(model);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}