namespace BricksWarehouse.Interfaces.Services
{
    /// <summary> Маппер </summary>
    /// <typeparam name="TOut">Требуемый тип</typeparam>
    public interface IMapper<out TOut>
    {
        /// <summary> Преобразование типа </summary>
        /// <param name="source">Исходный тип</param>
        /// <returns>Результат</returns>
        TOut Map(object source);
    }
}
