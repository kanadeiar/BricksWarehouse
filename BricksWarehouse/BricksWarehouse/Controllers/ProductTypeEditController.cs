using Microsoft.AspNetCore.Mvc;

namespace BricksWarehouse.Controllers
{
    public class ProductTypeEditController : Controller
    {
        private readonly IProductTypeData _productTypeData;
        private readonly ILogger<ProductTypeEditController> _logger;
        private readonly IMapper<ProductTypeEditWebModel> _mapperToWeb;
        private readonly IMapper<ProductType> _mapperFromWeb;

        public ProductTypeEditController(IProductTypeData productTypeData, ILogger<ProductTypeEditController> logger,
            IMapper<ProductTypeEditWebModel> mapperToWeb, IMapper<ProductType> mapperFromWeb)
        {
            _productTypeData = productTypeData;
            _logger = logger;
            _mapperToWeb = mapperToWeb;
            _mapperFromWeb = mapperFromWeb;
        }

        public async Task<IActionResult> Index()
        {
            var productTypes = (await _productTypeData.GetAllAsync()).OrderBy(p => p.Order);
            return View(productTypes.Select(pt => _mapperToWeb.Map(pt)));
        }

        public async Task<IActionResult> Trashed()
        {
            var productTypes = (await _productTypeData.GetAllAsync( trashed:true )).OrderBy(p => p.Order);
            return View(productTypes.Select(pt => _mapperToWeb.Map(pt)));
        }


    }
}
