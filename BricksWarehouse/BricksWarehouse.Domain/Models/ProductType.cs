using BricksWarehouse.Domain.Models.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BricksWarehouse.Domain.Models
{
    /// <summary> Вид товаров </summary>
    public class ProductType : Entity
    {
        private string _Name;
        /// <summary> Название </summary>
        [Required(ErrorMessage = "Название вида товаров обязательно")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Название вида товаров должно быть длинной от 3 до 200 символов")]
        public string Name
        {
            get => _Name;
            set => Set(ref _Name, value);
        }
        private int _Order;
        /// <summary> Сортировка </summary>
        public int Order
        {
            get => _Order;
            set => Set(ref _Order, value);
        }
        private int _Units;
        /// <summary> Количество едениц в пачке </summary>
        [Range(0, int.MaxValue, ErrorMessage = "Количество едениц в пачке товаров должно быть положительным числом")]
        public int Units
        {
            get => _Units;
            set => Set(ref _Units, value);
        }
        private double _Volume;
        /// <summary> Объем, занимаемый одной пачкой </summary>
        [Range(0.0, double.MaxValue, ErrorMessage = "Объем одной пачки должен быть положительным числом")]
        public double Volume
        {
            get => _Volume;
            set => Set(ref _Volume, value);
        }
        private double _Weight;
        /// <summary> Вес одной пачки </summary>
        [Range(0.0, double.MaxValue, ErrorMessage = "Вес одной пачки должен быть положительным числом")]
        public double Weight
        {
            get => _Weight;
            set => Set(ref _Weight, value);
        }
        private bool _IsDelete;
        /// <summary> Метка удаления вида товаров </summary>
        public bool IsDelete
        {
            get => _IsDelete;
            set => Set(ref _IsDelete, value);
        }

        /// <summary> Места размещения этого вида товаров </summary>
        public ICollection<Place> Places { get; set; }
    }
}
