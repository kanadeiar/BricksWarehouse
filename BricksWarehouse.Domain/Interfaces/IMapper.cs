namespace BricksWarehouse.Domain.Interfaces
{
    /// <summary> Маппер </summary>
    /// <typeparam name="TOut">Требуемый тип</typeparam>
    public interface IMapper<TIn, TOut>
    {
        /// <summary> Преобразование типа </summary>
        /// <param name="source">Исходный тип</param>
        /// <returns>Результат</returns>
        TOut Map(TIn source);
    }
}
