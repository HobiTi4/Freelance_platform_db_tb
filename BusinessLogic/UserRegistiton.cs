using Freelance_Platform.Entities;

namespace Freelance_Platform.Logic
{
    public class UserRegistration
    {
        public static void RegisterUser()
        {
            using (var context = new FreelanceDbContext())
            {
                Console.WriteLine("Enter your name:");
                string name = Console.ReadLine();

                Console.WriteLine("Enter your email:");
                string email = Console.ReadLine();

                Console.WriteLine("Select your role:");
                var roles = context.Roles.ToList();
                for (int i = 0; i < roles.Count; i++)
                {
                    Console.WriteLine($"{roles[i].RoleID}. {roles[i].Name}");
                }

                int selectedRoleId;
                while (true)
                {
                    Console.Write("Enter the Role ID: ");
                    if (int.TryParse(Console.ReadLine(), out selectedRoleId) && roles.Any(r => r.RoleID == selectedRoleId))
                    {
                        break;
                    }
                    Console.WriteLine("Invalid Role ID. Please try again.");
                }

                var selectedRole = context.Roles.First(r => r.RoleID == selectedRoleId);

                User newUser = new User
                {
                    Name = name,
                    Email = email,
                    Role = selectedRole,
                    Balance = 0, 
                    Rating = 0  
                };

                context.Users.Add(newUser);
                context.SaveChanges();

                Console.WriteLine("User registered successfully!");
            }
        }
    }
}
