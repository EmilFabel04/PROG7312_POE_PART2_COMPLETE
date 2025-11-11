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
			Id = new Guid("00000000-0000-0000-0000-000000000001"),
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
			Id = new Guid("00000000-0000-0000-0000-000000000002"),
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
			Dependencies = new List<Guid> { new Guid("00000000-0000-0000-0000-000000000001") }
		};
		requests.Add(req2);

		var req3 = new ServiceRequest
		{
			Id = new Guid("00000000-0000-0000-0000-000000000003"),
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
			Id = new Guid("00000000-0000-0000-0000-000000000004"),
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
			Id = new Guid("00000000-0000-0000-0000-000000000005"),
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
			Id = new Guid("00000000-0000-0000-0000-000000000006"),
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
			Id = new Guid("00000000-0000-0000-0000-000000000007"),
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
			Id = new Guid("00000000-0000-0000-0000-000000000008"),
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
			Id = new Guid("00000000-0000-0000-0000-000000000009"),
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
			Dependencies = new List<Guid> { new Guid("00000000-0000-0000-0000-000000000008") }
		};
		requests.Add(req9);

		var req10 = new ServiceRequest
		{
			Id = new Guid("00000000-0000-0000-0000-000000000010"),
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

		var req11 = new ServiceRequest
		{
			Id = new Guid("00000000-0000-0000-0000-000000000011"),
			RequestNumber = "SR-L6M4N8",
			Title = "Street Sign Repair",
			Description = "Damaged street sign needs replacement",
			Category = "Roads",
			Location = "Corner Street, Rustenburg",
			ReportedBy = "Tom Wilson",
			ContactInfo = "tom.w@email.com | 0712345678",
			Priority = RequestPriority.Low,
			Status = RequestStatus.New,
			AssignedTo = "",
			CreatedDate = baseDate.AddDays(20),
			StatusUpdatedDate = null
		};
		requests.Add(req11);

		var req12 = new ServiceRequest
		{
			Id = new Guid("00000000-0000-0000-0000-000000000012"),
			RequestNumber = "SR-M9P2Q5",
			Title = "Water Meter Reading",
			Description = "Faulty water meter needs inspection",
			Category = "Utilities",
			Location = "Residential Area, Witbank",
			ReportedBy = "Jane Adams",
			ContactInfo = "jane.a@email.com | 0798765432",
			Priority = RequestPriority.Medium,
			Status = RequestStatus.Assigned,
			AssignedTo = "Water Department",
			CreatedDate = baseDate.AddDays(21),
			StatusUpdatedDate = baseDate.AddDays(22)
		};
		requests.Add(req12);

		var req13 = new ServiceRequest
		{
			Id = new Guid("00000000-0000-0000-0000-000000000013"),
			RequestNumber = "SR-N3R7S1",
			Title = "Public Toilet Maintenance",
			Description = "Public toilet facility needs cleaning",
			Category = "Sanitation",
			Location = "City Center, Vanderbijlpark",
			ReportedBy = "Mark Thompson",
			ContactInfo = "mark.t@email.com | 0823456789",
			Priority = RequestPriority.Medium,
			Status = RequestStatus.InProgress,
			AssignedTo = "Sanitation Team",
			CreatedDate = baseDate.AddDays(23),
			StatusUpdatedDate = baseDate.AddDays(24)
		};
		requests.Add(req13);

		var req14 = new ServiceRequest
		{
			Id = new Guid("00000000-0000-0000-0000-000000000014"),
			RequestNumber = "SR-O8T4U6",
			Title = "Bus Stop Shelter Repair",
			Description = "Broken glass in bus stop shelter",
			Category = "Safety",
			Location = "Main Road, Secunda",
			ReportedBy = "Linda Roberts",
			ContactInfo = "linda.r@email.com | 0734567890",
			Priority = RequestPriority.High,
			Status = RequestStatus.Assigned,
			AssignedTo = "Safety Department",
			CreatedDate = baseDate.AddDays(25),
			StatusUpdatedDate = baseDate.AddDays(26),
			Dependencies = new List<Guid> { new Guid("00000000-0000-0000-0000-000000000002") }
		};
		requests.Add(req14);

		var req15 = new ServiceRequest
		{
			Id = new Guid("00000000-0000-0000-0000-000000000015"),
			RequestNumber = "SR-P5V9W2",
			Title = "Library Air Conditioning",
			Description = "Air conditioning system not working in library",
			Category = "Maintenance",
			Location = "Public Library, Middelburg",
			ReportedBy = "Carol Mitchell",
			ContactInfo = "carol.m@email.com | 0845678901",
			Priority = RequestPriority.Medium,
			Status = RequestStatus.New,
			AssignedTo = "",
			CreatedDate = baseDate.AddDays(27),
			StatusUpdatedDate = null
		};
		requests.Add(req15);

		return requests;
	}
}

