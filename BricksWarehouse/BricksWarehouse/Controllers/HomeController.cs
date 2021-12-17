namespace BricksWarehouse.Controllers;

public class HomeController : Controller
{
    private readonly IWebHostEnvironment _AppEnvironment;
    public HomeController(IWebHostEnvironment appEnvironment)
    {
        _AppEnvironment = appEnvironment;
    }
    public async Task<IActionResult> Index([FromServices] FillingsInfoService fillingsInfoService, [FromServices] CountsInfoService countsInfoService)
    {
        ViewBag.Fillings = await fillingsInfoService.GetFillings();
        ViewBag.Counts = await countsInfoService.GetCounts();
        return View();
    }

    public IActionResult FirstStart()
    {
        return View();
    }

    public IActionResult About()
    {
        return View();
    }

    public VirtualFileResult GetAndroidAppFile()
    {
        var filepath = Path.Combine("~/files", "BricksWarehouseMin.apk");
        return File(filepath, "application/vnd.android.package-archive", "BricksWarehouse.apk");
    }

    public IActionResult Error(string id)
    {        
        switch (id)
        {
            default: return Content($"Status --- {id}");
            case "404": return View("Error404");
        }
    }
}

