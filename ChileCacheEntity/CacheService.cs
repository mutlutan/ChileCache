
public class CacheService
{
	private readonly CacheContext _context;

	public CacheService(CacheContext context)
	{
		_context = context;
	}

	public void AddItem(CacheItem item)
	{
		_context.CacheItems.Add(item);
		_context.SaveChanges();
	}

	public IEnumerable<CacheItem> SearchItems(string searchTerm)
	{
		return _context.CacheItems
			.Where(item => item.Name.Contains(searchTerm) || item.Description.Contains(searchTerm))
			.ToList();
	}

	public void RemoveItem(int id)
	{
		var item = _context.CacheItems.Find(id);
		if (item != null)
		{
			_context.CacheItems.Remove(item);
			_context.SaveChanges();
		}
	}
}
