using BricksWarehouse.Domain.Models.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BricksWarehouse.Domain.Models
{
    /// <summary> Место размещения товаров </summary>
    public class Place : Entity
    {
        private string _Name;
        /// <summary> Название </summary>
        [Required(ErrorMessage = "Название места размещения товаров обязательно")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Название места размещения товаров должно быть длинной от 3 до 200 символов")]
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
        private int _Number;
        /// <summary> Номер </summary>
        [Range(0, int.MaxValue, ErrorMessage = "Номер места должно быть положительным числом")]
        public int Number
        {
            get => _Number;
            set => Set(ref _Number, value);
        }

        private int? _ProductTypeId;
        /// <summary> Вид товаров </summary>
        public int? ProductTypeId
        {
            get => _ProductTypeId;
            set => Set(ref _ProductTypeId, value);
        }
        private string _ProductTypeName;
        /// <summary> Название вида товаров </summary>
        [ForeignKey(nameof(ProductTypeId))]
        public string ProductTypeName
        {
            get => _ProductTypeName;
            set => Set(ref _ProductTypeName, value);
        }

        private int _Count;
        /// <summary> Количество </summary>
        [Range(0, int.MaxValue, ErrorMessage = "Количество товара на этом месте должно быть положительным числом")]
        public int Count
        {
            get => _Count;
            set => Set(ref _Count, value);
        }
        private int _Size;
        /// <summary> Вместимость </summary>
        [Range(0, int.MaxValue, ErrorMessage = "Вместимость товаров на этом месте должно быть положительным числом")]
        public int Size
        {
            get => _Size;
            set => Set(ref _Size, value);
        }
        private DateTime _LastDateTime = DateTime.Now;
        /// <summary> Дата и время помещения последнего товара </summary>
        [Required(ErrorMessage = "Дата и время помещения последнего товара на склад должна быть выбрана")]
        public DateTime LastDateTime 
        { 
            get => _LastDateTime; 
            set => Set(ref _LastDateTime, value); 
        }
        private PlaceStatus _PlaceStatus;
        /// <summary> Статус места </summary>
        public PlaceStatus PlaceStatus
        {
            get => _PlaceStatus;
            set => Set(ref _PlaceStatus, value);
        }

        private string _Comment;
        /// <summary> Комментарий </summary>
        [StringLength(200, ErrorMessage = "Комментарий должен быть короче 200 символов")]
        public string Comment
        {
            get => _Comment;
            set => Set(ref _Comment, value);
        }
        private bool _IsDelete;
        /// <summary> Метка удаления вида товаров </summary>
        public bool IsDelete
        {
            get => _IsDelete;
            set => Set(ref _IsDelete, value);
        }
    }

    /// <summary> Статус места размещения товаров </summary>
    public enum PlaceStatus
    {
        /// <summary> По умолчанию </summary>
        IsDefault,
        /// <summary> Набор товара </summary>
        IsCollect,
        /// <summary> Выжидание </summary>
        IsWait,
        /// <summary> Выдача </summary>
        IsDelivery
    }
}
