using MunicipalityMvc.Core.Models;

namespace MunicipalityMvc.Core.Services;

public interface IServiceRequestStatusService
{
	void LoadRequests(List<ServiceRequest> requests);
	ServiceRequest FindByRequestNumber(string requestNumber);
	List<ServiceRequest> GetAllRequests();
	List<ServiceRequest> GetPriorityRequests();
	List<ServiceRequest> GetDependencies(Guid requestId);
	List<ServiceRequest> GetRequestsDependingOn(Guid requestId);
}

