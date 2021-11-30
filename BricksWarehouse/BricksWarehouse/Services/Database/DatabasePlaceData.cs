namespace BricksWarehouse.Services.Database;

public class DatabasePlaceData : IPlaceData
{
    private readonly WarehouseContext _context;
    private readonly ILogger<DatabasePlaceData> _logger;
    public DatabasePlaceData(WarehouseContext context, ILogger<DatabasePlaceData> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<Place>> GetAllAsync(bool includes = false, bool withTrashed = false)
    {
        IQueryable<Place> query = (includes) 
            ? _context.Places.Include(p => p.ProductType).AsQueryable() 
            : _context.Places.AsQueryable();
        if (!withTrashed)
            query.Where(pt => !pt.IsDelete);
        return await query.ToArrayAsync().ConfigureAwait(false);
    }

    public async Task<Place> GetAsync(int id)
    {
        var result = await _context.Places.Include(p => p.ProductType).SingleOrDefaultAsync(p => p.Id == id).ConfigureAwait(false);
        return result;
    }

    public async Task<int> AddAsync(Place place)
    {
        if (place is null)
            throw new ArgumentNullException(nameof(place));
        _context.Places.Add(place);
        await _context.SaveChangesAsync().ConfigureAwait(false);
        return place.Id;
    }

    public async Task UpdateAsync(Place place)
    {
        if (place is null)
            throw new ArgumentNullException(nameof(place));
        if (_context.Places.Local.Any(p => p == place) == false)
        {
            var origin = await _context.Places.FindAsync(place.Id).ConfigureAwait(false);
            origin.Name = place.Name;
            origin.Order = place.Order;
            origin.Number = place.Number;
            origin.ProductTypeId = place.ProductTypeId;
            origin.Count = place.Count;
            origin.Size = place.Size;
            origin.LastDateTime = place.LastDateTime;
            origin.PlaceStatus = place.PlaceStatus;
            origin.Comment = place.Comment;
            origin.IsDelete = place.IsDelete;
            _context.Update(origin);
        }
        else
            _context.Update(place);
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        if (await GetAsync(id) is not { } item)
            return false;
        _context.Places.Remove(item);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> TrashAsync(int id, bool undo = false)
    {
        if (await GetAsync(id).ConfigureAwait(false) is { } place)
            place.IsDelete = !undo;
        else
            return false;
        await _context.SaveChangesAsync();
        return true;
    }
}

