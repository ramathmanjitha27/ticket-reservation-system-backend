namespace TrainReservationsApi.Models
{
    public class AuthResponseModel
    {
        public string Message { get; set; } = string.Empty;
        public int Status {  get; set; } = 404;
        public bool Success { get; set; } = false;
        public string Token { get; set; } = string.Empty;

        public Staff User { get; set; } = new Staff();
    }
}
