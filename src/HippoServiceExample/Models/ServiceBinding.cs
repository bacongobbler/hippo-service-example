using System.Collections.Generic;
using OpenServiceBroker.Bindings;

namespace HippoServiceExample.Models;

public class ServiceBinding
{
	public string Credentials { get; set; } = default!;

	public string SyslogDrainUrl { get; set; } = default!;

	public string RouteServiceUrl { get; set; } = default!;

	public virtual ICollection<ServiceBindingVolumeMount> VolumeMounts { get; set; } = default!;

	public string? Parameters { get; set; }
}
