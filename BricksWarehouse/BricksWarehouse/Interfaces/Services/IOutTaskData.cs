namespace BricksWarehouse.Interfaces.Services
{
    /// <summary> Источник заданий операторов </summary>
    public interface IOutTaskData
    {
        /// <summary> Получить все </summary>
        Task<IEnumerable<OutTask>> GetAllAsync(bool includes = false);
        /// <summary> Данные одного </summary>
        Task<OutTask> GetAsync(int id);
        /// <summary> Добавить </summary>
        Task<int> AddAsync(OutTask productType);
        /// <summary> Обновить </summary>
        Task UpdateAsync(OutTask productType);
        /// <summary> Удалить </summary>
        Task<bool> DeleteAsync(int id);
    }
}
