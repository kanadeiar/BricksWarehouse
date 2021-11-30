using BricksWarehouse.Domain.Models.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BricksWarehouse.Domain.Models
{
    /// <summary> Вид товаров </summary>
    public class ProductType : Entity
    {
        /// <summary> Название </summary>
        [Required(ErrorMessage = "Название вида товаров обязательно")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Название вида товаров должно быть длинной от 3 до 200 символов")]
        public string Name { get; set; }
        /// <summary> Номер формата </summary>
        [Range(0, int.MaxValue, ErrorMessage = "Номер формата должен быть положительным числом")]
        public int FormatNumber { get; set; }
        /// <summary> Сортировка </summary>
        public int Order { get; set; }
        /// <summary> Количество едениц в пачке </summary>
        [Range(0, int.MaxValue, ErrorMessage = "Количество едениц в пачке товаров должно быть положительным числом")]
        public int Units { get; set; }
        /// <summary> Объем, занимаемый одной пачкой </summary>
        [Range(0.0, double.MaxValue, ErrorMessage = "Объем одной пачки должен быть положительным числом")]
        public double Volume { get; set; }
        /// <summary> Вес одной пачки </summary>
        [Range(0.0, double.MaxValue, ErrorMessage = "Вес одной пачки должен быть положительным числом")]
        public double Weight { get; set; }
        /// <summary> Метка удаления вида товаров </summary>
        public bool IsDelete { get; set; }

        /// <summary> Места размещения этого вида товаров </summary>
        public ICollection<Place> Places { get; set; } = new List<Place>();
    }
}
