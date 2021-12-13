namespace BricksWarehouse.Services.Database;

public class DatabaseOutTaskData : IOutTaskData
{
    private readonly WarehouseContext _context;
    private readonly ILogger<DatabasePlaceData> _logger;
    public DatabaseOutTaskData(WarehouseContext context, ILogger<DatabasePlaceData> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<OutTask>> GetAllAsync(bool includes = false)
    {
        IQueryable<OutTask> query = (includes)
            ? _context.OutTasks.Include(ot => ot.ProductType).AsQueryable()
            : _context.OutTasks.AsQueryable();
        return await query.ToArrayAsync().ConfigureAwait(false);
    }

    public async Task<OutTask> GetAsync(int id)
    {
        var outtask = await _context.OutTasks.Include(ot => ot.ProductType).SingleOrDefaultAsync(ot => ot.Id == id).ConfigureAwait(false);
        return outtask;
    }

    public async Task<int> AddAsync(OutTask outTask)
    {
        if (outTask is null)
            throw new ArgumentNullException(nameof(outTask));
        _context.OutTasks.Add(outTask);
        await _context.SaveChangesAsync().ConfigureAwait(false);
        return outTask.Id;
    }

    public async Task UpdateAsync(OutTask outTask)
    {
        if (outTask is null)
            throw new ArgumentNullException(nameof(outTask));
        if (_context.OutTasks.Local.Any(e => e == outTask) == false)
        {
            var origin = await _context.OutTasks.FindAsync(outTask.Id).ConfigureAwait(false);
            origin.Name = outTask.Name;
            origin.Number = outTask.Number;
            origin.ProductTypeId = outTask.ProductTypeId;
            origin.Count = outTask.Count;
            origin.TruckNumber = outTask.TruckNumber;
            origin.Loaded = outTask.Loaded;
            origin.CreatedDateTime = outTask.CreatedDateTime;
            origin.Comment = outTask.Comment;
            origin.IsCompleted = outTask.IsCompleted;
            _context.Update(origin);
        }
        else
            _context.Update(outTask);
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        if (await GetAsync(id) is not { } item)
            return false;
        _context.Remove(item);
        await _context.SaveChangesAsync();
        return true;
    }
}

