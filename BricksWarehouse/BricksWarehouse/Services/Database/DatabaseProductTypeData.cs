namespace BricksWarehouse.Services.Database;

public class DatabaseProductTypeData : IProductTypeData
{
    private readonly WarehouseContext _context;
    private readonly ILogger<DatabaseProductTypeData> _logger;
    public DatabaseProductTypeData(WarehouseContext context, ILogger<DatabaseProductTypeData> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<ProductType>> GetAllAsync(bool includes = false, bool trashed = false)
    {
        IQueryable<ProductType> query = (includes) 
            ? _context.ProductTypes.Include(pt => pt.Places).AsQueryable() 
            : _context.ProductTypes.AsQueryable();
        query = !trashed 
            ? query.Where(pt => !pt.IsDelete) 
            : query.Where(pt => pt.IsDelete);
        return await query.ToArrayAsync().ConfigureAwait(false);
    }

    public async Task<ProductType> GetAsync(int id)
    {
        var result = await _context.ProductTypes.Include(pt => pt.Places).SingleOrDefaultAsync(pt => pt.Id == id).ConfigureAwait(false);
        return result;
    }

    public async Task<int> AddAsync(ProductType productType)
    {
        if (productType is null)
            throw new ArgumentNullException(nameof(productType));
        _context.ProductTypes.Add(productType);
        await _context.SaveChangesAsync().ConfigureAwait(false);
        return productType.Id;
    }

    public async Task UpdateAsync(ProductType productType)
    {
        if (productType is null)
            throw new ArgumentNullException(nameof(productType));
        if (_context.ProductTypes.Local.Any(e => e == productType) == false)
        {
            var origin = await _context.ProductTypes.FindAsync(productType.Id).ConfigureAwait(false);
            origin.Name = productType.Name;
            origin.FormatNumber = productType.FormatNumber;
            origin.Order = productType.Order;
            origin.Units = productType.Units;
            origin.Volume = productType.Volume;
            origin.Weight = productType.Weight;
            origin.IsDelete = productType.IsDelete;
            _context.Update(origin);
        }
        else
            _context.Update(productType);
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        if (await GetAsync(id) is not { } item)
            return false;
        _context.ProductTypes.Remove(item);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> TrashAsync(int id, bool undo = false)
    {
        if (await GetAsync(id).ConfigureAwait(false) is { } productType)
            productType.IsDelete = !undo;
        else
            return false;
        await _context.SaveChangesAsync();
        return true;
    }


    public async Task<ProductType> GetByFormatAsync(int format)
    {
        var result = await _context.ProductTypes.Include(pt => pt.Places).SingleOrDefaultAsync(pt => pt.FormatNumber == format).ConfigureAwait(false);
        return result;
    }
}

