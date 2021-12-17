namespace BricksWarehouse.Controllers;

public class HomeController : Controller
{
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

    public IActionResult Error(string id)
    {        
        switch (id)
        {
            default: return Content($"Status --- {id}");
            case "404": return View("Error404");
        }
    }
}

