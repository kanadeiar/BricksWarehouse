using System.Linq;

namespace BricksWarehouse.Services
{
    public class CountsInfoService
    {
        private readonly IProductTypeData _productTypeData;
        public CountsInfoService(IProductTypeData productTypeData)
        {
            _productTypeData = productTypeData;
        }

        public async Task<IEnumerable<CountsInfoWebModel>> GetCounts()
        {
            var counts = await _productTypeData.GetAllAsync(true);
            var results = counts.Select(pt => new CountsInfoWebModel
            {
                Id = pt.Id,
                Name = pt.Name,
                NumberFormat = pt.FormatNumber,
                Count = pt.Places.Sum(p => p.Count),
                Volume = pt.Volume * pt.Places.Sum(p => p.Count),
                CountUnits = pt.Units * pt.Places.Sum(p => p.Count),
            }).ToList();
            var maxCount = results.Max(p => p.Count);
            foreach (var product in results)
            {
                product.CountPercent = Convert.ToInt32(product.Count / (double)maxCount * 100.0);
                if (product.Count == 0)
                    product.Status = "Отсутствует товар на складе";
                else if (product.Count >= maxCount * 0.9)
                    product.Status = "Избыток товара на складе";
                else if (product.Count <= maxCount * 0.1)
                    product.Status = "Недостаток товара на складе";
                else
                    product.Status = "Товар есть на складе";
            }
            return results.OrderBy(r => r.Name);
        }
    }

    public class CountsInfoWebModel
    {
        public int Id { get; set; }
        [Display(Name = "Название вида товара")]
        public string Name { get; set; }
        [Display(Name = "Номер формата")]
        public int NumberFormat { get; set; }
        [Display(Name = "Количество пачек, шт.")]
        public int Count { get; set; }
        [Display(Name = "Количество процент от максимального %")]
        public int CountPercent { get; set; }
        [Display(Name = "Объем товара, м3")]
        public double Volume { get; set; }
        [Display(Name = "Количество едениц, шт.")]
        public int CountUnits { get; set; }
        [Display(Name = "Статус")]
        public string Status { get; set; }
    }
}
