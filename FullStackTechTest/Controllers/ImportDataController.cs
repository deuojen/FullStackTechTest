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

    /// <summary>
    /// Import Data allows to users to insert data from json file
    /// Validations: 
    ///     1. Empty file should warn to user to can't upload
    ///     2. Non Json files should warn to user to can't process
    ///     3. Wrong Json formats should warn to user to can't process
    ///     
    /// We have to check unique people by their GMC number, 
    /// If same GMC number is exists ignore insert for person and address
    /// Same process for current import, if person inserted already ignore insert again
    /// 
    /// If person address not exists in the file add empty data so on edit page we can update it
    /// 
    /// </summary>
    /// <param name="postedFile"></param>
    /// <returns></returns>
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

        try
        {
            var persons = JsonConvert.DeserializeObject<List<PersonWithAddress>>(result.ToString());

            model = await IndexViewModel.InsertAsync(_importDataRepository, persons);

            ViewData["Message"] = $"File successfully processed. Duplicate data ignored and {model.InsertPeopleCount} of {persons.Count} people imported.";
        }
        catch (Exception)
        {
            ModelState.AddModelError("UploadFile", "File content is not suitable! Please upload valid json file.");
            return View(model);
        }

        return View(model);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}