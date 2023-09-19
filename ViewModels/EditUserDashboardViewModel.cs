namespace WebAppCarClub.ViewModels
{
    public class EditUserDashboardViewModel
    {
        public string Id { get; set; }
        public int? Speed { get; set; }
        public int? Mileage { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public IFormFile? Image { get; set; }
        public string? Bio { get; set;}
    }
}
