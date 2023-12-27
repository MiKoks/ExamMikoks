namespace Domain;
 
 public class Apartment : BaseEntity
 {
     public Guid PropertyId { get; set; }
     public Property? Property { get; set; }
     public int FloorNumber { get; set; }
     public int RoomCount { get; set; }
     public decimal MonthlyRent { get; set; }
     public bool Status { get; set; }
     public Guid? CurrentLeaseId { get; set; }
     public Lease? CurrentLease { get; set; }
 }