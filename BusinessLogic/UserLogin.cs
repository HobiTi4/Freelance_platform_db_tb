using Freelance_Platform.Entities;
using Microsoft.EntityFrameworkCore;

namespace Freelance_Platform.Logic
{
        public static class UserLogin
        {
        public static User LoginUser()
        {
            using (var context = new FreelanceDbContext())
            {
                Console.WriteLine("\n=== User Login ===");

                Console.Write("Enter your Email: ");
                string email = Console.ReadLine();

                var user = context.Users
                    .Include(u => u.Role) 
                    .FirstOrDefault(u => u.Email == email);

                if (user != null)
                {
                    Console.WriteLine($"\nWelcome, {user.Name}!");
                    Console.WriteLine($"Your role is: {user.Role?.Name}");
                    return user;
                }
                else
                {
                    Console.WriteLine("\nUser not found. Please check your Email or register first.");
                    return null;
                }
            }
        }

    }
}
        
    
