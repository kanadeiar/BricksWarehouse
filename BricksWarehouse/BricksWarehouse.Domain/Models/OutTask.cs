using BricksWarehouse.Domain.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BricksWarehouse.Domain.Models
{
    /// <summary> Задания на отгрузку товаров </summary>
    public class OutTask : Entity
    {
        /// <summary> Название </summary>
        [Required(ErrorMessage = "Название задания обязательно")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Название задания должно быть длинной от 3 до 200 символов")]
        public string Name { get; set; }

        /// <summary> Номер задания </summary>
        [Range(0, int.MaxValue, ErrorMessage = "Номер задания должен быть положительным числом")]
        public int Number { get; set; }

        /// <summary> Вид товаров </summary>
        public int? ProductTypeId { get; set; }
        /// <summary> Вид товаров </summary>
        [ForeignKey(nameof(ProductTypeId))]
        public ProductType? ProductType { get; set; }

        /// <summary> Количество </summary>
        [Range(0, int.MaxValue, ErrorMessage = "Количество товара на этом месте должно быть положительным числом")]
        public int Count { get; set; }

        /// <summary> Регистрационный номер грузовика </summary>
        [Required(ErrorMessage = "Регистрационный номер грузовика обязателен")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Регистрационный номер грузовика должен быть длинной от 3 до 200 символов")]
        public string TruckNumber { get; set; }

        /// <summary> Загружено </summary>
        [Range(0, int.MaxValue, ErrorMessage = "Количество загруженного товара на этом грузовике должно быть положительным числом")]
        public int Loaded { get; set; }

        /// <summary> Дата и время задания </summary>
        [Required(ErrorMessage = "Дата и время задания должна быть выбрана")]
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;

        /// <summary> Комментарий </summary>
        [StringLength(200, ErrorMessage = "Комментарий должен быть короче 200 символов")]
        public string? Comment { get; set; }

        /// <summary> Статус выполнения задания </summary>
        public bool IsCompleted { get; set; }
    }
}
