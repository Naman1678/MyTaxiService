namespace MyTaxiService.Models
{
    public class Ride
    {
     public int Id { get; set; }
        public int UserID { get; set; }
        public int DriverID { get; set; }
        public required String PickupLocation { get; set; }
        public required String DropLocation { get; set; }
        public DateTime RideTime { get; set; }
        public decimal Amount { get; set; }
        public bool IsCancelled { get; set; }

    }
}
