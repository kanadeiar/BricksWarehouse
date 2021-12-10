namespace BricksWarehouse.Interfaces.Services;

/// <summary> Источник видов товаров </summary>
public interface IProductTypeData
{
    /// <summary> Получить все </summary>
    Task<IEnumerable<ProductType>> GetAllAsync(bool includes = false, bool trashed = false);
    /// <summary> Данные одного </summary>
    Task<ProductType> GetAsync(int id);
    /// <summary> Добавить </summary>
    Task<int> AddAsync(ProductType productType);
    /// <summary> Обновить </summary>
    Task UpdateAsync(ProductType productType);
    /// <summary> Пометка удаления </summary>
    Task<bool> TrashAsync(int id, bool undo = false);
    /// <summary> Удалить </summary>
    Task<bool> DeleteAsync(int id);

    /// <summary> Данные одного по формату </summary>
    Task<ProductType> GetByFormatAsync(int format);
}

