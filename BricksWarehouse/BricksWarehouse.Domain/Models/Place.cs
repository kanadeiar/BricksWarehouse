using BricksWarehouse.Domain.Models.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BricksWarehouse.Domain.Models
{
    /// <summary> Место размещения товаров </summary>
    public class Place : Entity
    {
        /// <summary> Название </summary>
        [Required(ErrorMessage = "Название места размещения товаров обязательно")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Название места размещения товаров должно быть длинной от 3 до 200 символов")]
        public string Name { get; set; }
        /// <summary> Сортировка </summary>
        public int Order { get; set; }
        /// <summary> Номер </summary>
        [Range(0, int.MaxValue, ErrorMessage = "Номер места должно быть положительным числом")]
        public int Number { get; set; }
        /// <summary> Вид товаров </summary>
        public int? ProductTypeId { get; set; }
        /// <summary> Вид товаров </summary>
        [ForeignKey(nameof(ProductTypeId))]
        public ProductType ProductType { get; set; }
        /// <summary> Количество </summary>
        [Range(0, int.MaxValue, ErrorMessage = "Количество товара на этом месте должно быть положительным числом")]
        public int Count { get; set; }
        /// <summary> Вместимость </summary>
        [Range(0, int.MaxValue, ErrorMessage = "Вместимость товаров на этом месте должно быть положительным числом")]
        public int Size { get; set; }
        /// <summary> Дата и время помещения последнего товара </summary>
        [Required(ErrorMessage = "Дата и время помещения последнего товара на склад должна быть выбрана")]
        public DateTime LastDateTime { get; set; } = DateTime.Now; 
        /// <summary> Статус места </summary>
        public PlaceStatus PlaceStatus { get; set; }
        /// <summary> Комментарий </summary>
        [StringLength(200, ErrorMessage = "Комментарий должен быть короче 200 символов")]
        public string Comment { get; set; }
        /// <summary> Метка удаления вида товаров </summary>
        public bool IsDelete { get; set; }
    }

    /// <summary> Статус места размещения товаров </summary>
    public enum PlaceStatus
    {
        /// <summary> По умолчанию </summary>
        Default,
        /// <summary> Набор товара </summary>
        Collect,
        /// <summary> Выжидание </summary>
        Wait,
        /// <summary> Выдача </summary>
        Delivery
    }
}
