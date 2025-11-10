using MunicipalityMvc.Core.Models;

namespace MunicipalityMvc.Core.Services;

public class SeedDataService
{
	public static List<ServiceRequest> GetSampleRequests()
	{
		var requests = new List<ServiceRequest>();
		var baseDate = DateTime.UtcNow.AddDays(-30);

		var req1 = new ServiceRequest
		{
			Id = Guid.NewGuid(),
			RequestNumber = "SR-A2K9P3",
			Title = "Water Pipe Burst",
			Description = "Major water pipe burst causing flooding",
			Category = "Utilities",
			Location = "Main Street, Johannesburg",
			ReportedBy = "John Smith",
			ContactInfo = "john.smith@email.com | 0821234567",
			Priority = RequestPriority.High,
			Status = RequestStatus.InProgress,
			AssignedTo = "Water Department",
			CreatedDate = baseDate.AddDays(1),
			StatusUpdatedDate = baseDate.AddDays(2)
		};
		requests.Add(req1);

		var req2 = new ServiceRequest
		{
			Id = Guid.NewGuid(),
			RequestNumber = "SR-B7X4M2",
			Title = "Broken Streetlight",
			Description = "Broken streetlight creating safety hazard",
			Category = "Safety",
			Location = "Oak Avenue, Pretoria",
			ReportedBy = "Sarah Johnson",
			ContactInfo = "sarah.j@email.com | 0837654321",
			Priority = RequestPriority.High,
			Status = RequestStatus.Assigned,
			AssignedTo = "Electrical Team",
			CreatedDate = baseDate.AddDays(3),
			StatusUpdatedDate = baseDate.AddDays(4),
			Dependencies = new List<Guid> { req1.Id }
		};
		requests.Add(req2);

		var req3 = new ServiceRequest
		{
			Id = Guid.NewGuid(),
			RequestNumber = "SR-C3N8R5",
			Title = "Pothole Repair",
			Description = "Large pothole on main road",
			Category = "Roads",
			Location = "Park Road, Durban",
			ReportedBy = "Michael Brown",
			ContactInfo = "mbrown@email.com | 0729876543",
			Priority = RequestPriority.Medium,
			Status = RequestStatus.InProgress,
			AssignedTo = "Roads Department",
			CreatedDate = baseDate.AddDays(5),
			StatusUpdatedDate = baseDate.AddDays(6)
		};
		requests.Add(req3);

		var req4 = new ServiceRequest
		{
			Id = Guid.NewGuid(),
			RequestNumber = "SR-D9K2L7",
			Title = "Garbage Collection",
			Description = "Overflowing garbage bins at beach",
			Category = "Sanitation",
			Location = "Beach Front, Cape Town",
			ReportedBy = "Emma Wilson",
			ContactInfo = "emma.w@email.com | 0814567890",
			Priority = RequestPriority.Medium,
			Status = RequestStatus.Assigned,
			AssignedTo = "Sanitation Team",
			CreatedDate = baseDate.AddDays(7),
			StatusUpdatedDate = baseDate.AddDays(8)
		};
		requests.Add(req4);

		var req5 = new ServiceRequest
		{
			Id = Guid.NewGuid(),
			RequestNumber = "SR-E4P6T9",
			Title = "Playground Repair",
			Description = "Damaged playground equipment",
			Category = "Parks",
			Location = "Central Park, Bloemfontein",
			ReportedBy = "David Lee",
			ContactInfo = "dlee@email.com | 0723456789",
			Priority = RequestPriority.Medium,
			Status = RequestStatus.New,
			AssignedTo = "",
			CreatedDate = baseDate.AddDays(10),
			StatusUpdatedDate = null
		};
		requests.Add(req5);

		var req6 = new ServiceRequest
		{
			Id = Guid.NewGuid(),
			RequestNumber = "SR-F8H3Q2",
			Title = "Park Maintenance",
			Description = "Grass needs cutting in public park",
			Category = "Parks",
			Location = "Sunset Boulevard, Port Elizabeth",
			ReportedBy = "Lisa Anderson",
			ContactInfo = "lisa.a@email.com | 0836547890",
			Priority = RequestPriority.Low,
			Status = RequestStatus.New,
			AssignedTo = "",
			CreatedDate = baseDate.AddDays(12),
			StatusUpdatedDate = null
		};
		requests.Add(req6);

		var req7 = new ServiceRequest
		{
			Id = Guid.NewGuid(),
			RequestNumber = "SR-G2V7N4",
			Title = "Graffiti Removal",
			Description = "Graffiti on public building",
			Category = "Maintenance",
			Location = "Market Street, Polokwane",
			ReportedBy = "James Taylor",
			ContactInfo = "jtaylor@email.com | 0719876543",
			Priority = RequestPriority.Low,
			Status = RequestStatus.Resolved,
			AssignedTo = "Maintenance Team",
			CreatedDate = baseDate.AddDays(2),
			StatusUpdatedDate = baseDate.AddDays(15)
		};
		requests.Add(req7);

		var req8 = new ServiceRequest
		{
			Id = Guid.NewGuid(),
			RequestNumber = "SR-H5M9K6",
			Title = "Power Outage",
			Description = "Power outage affecting street lights",
			Category = "Utilities",
			Location = "Industrial Area, Nelspruit",
			ReportedBy = "Rachel Davis",
			ContactInfo = "rachel.d@email.com | 0827654321",
			Priority = RequestPriority.High,
			Status = RequestStatus.InProgress,
			AssignedTo = "Electrical Department",
			CreatedDate = baseDate.AddDays(14),
			StatusUpdatedDate = baseDate.AddDays(15)
		};
		requests.Add(req8);

		var req9 = new ServiceRequest
		{
			Id = Guid.NewGuid(),
			RequestNumber = "SR-J7R4P8",
			Title = "Traffic Light Repair",
			Description = "Traffic light malfunction",
			Category = "Roads",
			Location = "School Road, Kimberley",
			ReportedBy = "Peter Moore",
			ContactInfo = "pmoore@email.com | 0734567890",
			Priority = RequestPriority.High,
			Status = RequestStatus.Assigned,
			AssignedTo = "Traffic Department",
			CreatedDate = baseDate.AddDays(16),
			StatusUpdatedDate = baseDate.AddDays(17),
			Dependencies = new List<Guid> { req8.Id }
		};
		requests.Add(req9);

		var req10 = new ServiceRequest
		{
			Id = Guid.NewGuid(),
			RequestNumber = "SR-K3L8N2",
			Title = "Storm Drain Repair",
			Description = "Blocked storm drain",
			Category = "Sanitation",
			Location = "River Road, East London",
			ReportedBy = "Sophie Clark",
			ContactInfo = "sophie.c@email.com | 0845678901",
			Priority = RequestPriority.Medium,
			Status = RequestStatus.Closed,
			AssignedTo = "Drainage Team",
			CreatedDate = baseDate.AddDays(5),
			StatusUpdatedDate = baseDate.AddDays(18)
		};
		requests.Add(req10);

		return requests;
	}
}

