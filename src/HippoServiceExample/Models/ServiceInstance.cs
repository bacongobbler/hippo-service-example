using System.ComponentModel.DataAnnotations;

namespace HippoServiceExample.Models;

public class ServiceInstance
{
	[Key]
	public string Id { get; set; } = default!;

	public string ServiceId { get; set; } = default!;

	public string PlanId { get; set; } = default!;

	public string? Parameters { get; set; }
}
