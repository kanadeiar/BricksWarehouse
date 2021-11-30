namespace BricksWarehouse.WebModels;

/// <summary> Вебмодель редактирования видов товаров </summary>
public class ProductTypeEditWebModel
{
    [HiddenInput(DisplayValue = false)]
    public int Id { get; set; }
    [Display(Name = "Имя")]
    [Required(ErrorMessage = "Необходимо ввести название вида товаров")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "Длинна названия вида товара должна быть от 3 до 200 символов")]
    public string Name { get; set; }
    /// <summary> Номер формата </summary>
    [Display(Name = "Номер формата")]
    [Range(0, int.MaxValue, ErrorMessage = "Номер формата должен быть положительным числом")]
    public int FormatNumber { get; set; }
    /// <summary> Сортировка </summary>
    [Display(Name = "Сортировка")]
    public int Order { get; set; }
    /// <summary> Количество едениц в пачке </summary>
    [Display(Name = "Количество в пачке")]
    [Range(0, int.MaxValue, ErrorMessage = "Количество едениц в пачке товаров должно быть положительным числом")]
    public int Units { get; set; }
    /// <summary> Объем, занимаемый одной пачкой </summary>
    [Display(Name = "Объем одной пачки")]
    [Range(0.0, double.MaxValue, ErrorMessage = "Объем одной пачки должен быть положительным числом")]
    public double Volume { get; set; }
    /// <summary> Вес одной пачки </summary>
    [Display(Name = "Вес одной пачки")]
    [Range(0.0, double.MaxValue, ErrorMessage = "Вес одной пачки должен быть положительным числом")]
    public double Weight { get; set; }
    /// <summary> Метка удаления вида товаров </summary>
    [Display(Name = "Метка удаления")]
    public bool IsDelete { get; set; }
}

