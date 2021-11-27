using System.Threading.Tasks;
using HippoServiceExample.Repositories;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenServiceBroker.Bindings;
using OpenServiceBroker.Catalogs;
using OpenServiceBroker.Errors;

namespace HippoServiceExample.Bindings;

public class ServiceBindingBlocking : IServiceBindingBlocking
{

	private readonly DbContext _context;
	private readonly ILogger<ServiceBindingBlocking> _logger;
	private readonly Catalog _catalog;

	public ServiceBindingBlocking(DbContext context, ILogger<ServiceBindingBlocking> logger, Catalog catalog)
	{
		_context = context;
		_logger = logger;
		_catalog = catalog;
	}

	public async Task<ServiceBindingResource> FetchAsync(string instanceId, string bindingId)
	{
		_logger.LogTrace("Fetching instance {} with binding {}", instanceId, bindingId);

		var instance = await _context.ServiceInstances.FindAsync(instanceId);
		var binding = await _context.ServiceBindings.FindAsync(bindingId);
		if (binding == null) throw new NotFoundException($"Binding '{bindingId}' not found.");

		// TODO: return a NotFoundException if a binding operation is still in progress

		return new ServiceBindingResource
		{
			Credentials = (binding.Credentials == null) ? null : JsonConvert.DeserializeObject<JObject>(binding.Credentials),
			SyslogDrainUrl = binding.SyslogDrainUrl,
			RouteServiceUrl = binding.RouteServiceUrl,
			VolumeMounts = binding.VolumeMounts,
			Parameters = (binding.Parameters == null) ? null : JsonConvert.DeserializeObject<JObject>(binding.Parameters)
		};
	}

	public async Task<ServiceBinding> BindAsync(ServiceBindingContext context, ServiceBindingRequest request)
	{
		_logger.LogTrace("Binding id {} with service {} and plan {}", context.BindingId, request.ServiceId, request.PlanId);
		throw new System.NotImplementedException();
	}

	public async Task UnbindAsync(ServiceBindingContext context, string serviceId, string planId)
	{
		throw new System.NotImplementedException();
	}
}
