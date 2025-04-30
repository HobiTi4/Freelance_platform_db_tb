namespace Freelance_Platform.Entities
{
    public class User
    {
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int RoleID { get; set; }
        public Role Role { get; set; }
        public decimal? Balance { get; set; }
        public int Rating { get; set; }

    }
}
