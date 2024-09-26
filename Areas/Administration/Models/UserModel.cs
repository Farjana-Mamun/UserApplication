namespace UserApplication.Areas.Administration.Models
{
    public class UserModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime RegistrationTime { get; set; }
        public string Status { get; set; }
    }
}
