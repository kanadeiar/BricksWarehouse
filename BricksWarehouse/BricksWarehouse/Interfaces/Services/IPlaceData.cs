namespace BricksWarehouse.Interfaces.Services;

/// <summary> Источник мест хранений товаров </summary>
public interface IPlaceData
{
    /// <summary> Получить все </summary>
    Task<IEnumerable<Place>> GetAllAsync(bool includes = false, bool trashed = false);
    /// <summary> Данные одного </summary>
    Task<Place?> GetAsync(int id);
    /// <summary> Добавить </summary>
    Task<int> AddAsync(Place place);
    /// <summary> Обновить данные </summary>
    Task UpdateAsync(Place place);
    /// <summary> Пометка удаления </summary>
    Task<bool> TrashAsync(int id, bool undo = false);
    /// <summary> Удалить </summary>
    Task<bool> DeleteAsync(int id);

    /// <summary> Данные одного по номеру </summary>
    Task<Place> GetByNumberAsync(int number);
}

