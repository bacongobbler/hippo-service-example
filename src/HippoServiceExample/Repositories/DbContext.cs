using HippoServiceExample.Models;
using Microsoft.EntityFrameworkCore;

namespace HippoServiceExample.Repositories;

/// <summary>
/// Describes the service's database model.
/// Used as a combination of the Unit Of Work and Repository patterns.
/// </summary>
public class DbContext : Microsoft.EntityFrameworkCore.DbContext
{
	public DbSet<ServiceInstance> ServiceInstances { get; set; } = default!;
	public DbSet<ServiceBinding> ServiceBindings { get; set; } = default!;

	public DbContext(DbContextOptions options)
		: base(options)
	{ }
}
