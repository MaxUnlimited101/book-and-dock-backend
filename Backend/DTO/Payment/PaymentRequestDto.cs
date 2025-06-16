namespace Backend.DTO.Payment
{
    public class PaymentRequestDto
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "usd";
        public string SuccessUrl { get; set; }
        public string CancelUrl { get; set; }
    }

}
