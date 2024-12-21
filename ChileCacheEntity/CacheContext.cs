
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

public class CacheContext(DbContextOptions<CacheContext> options) : DbContext(options)
{
	public DbSet<CacheItem> CacheItems { get; set; }
}

