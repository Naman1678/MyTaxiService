namespace MyTaxiService.Models
{
    public class CancellationLog
    {
        public int Id { get; set; }
        public int RideId { get; set; }
        public required string Reason { get; set; }
        public decimal CancellationFee { get; set; }
    }
}
