using Freelance_Platform.Entities;
using System;
using System.Linq;

namespace Freelance_Platform.Logic
{
    public class ProjectService
    {
        private readonly FreelanceDbContext _context;

        public ProjectService(FreelanceDbContext context)
        {
            _context = context;
        }

        public void CreateProject(User currentUser)
        {
            if (currentUser == null)
            {
                Console.WriteLine("No user logged in.");
                return;
            }

            if (currentUser.Role?.Name != "Client")
            {
                Console.WriteLine("Only clients can create new projects.");
                return;
            }

            Console.WriteLine("\nEnter Project Name:");
            string name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Project name cannot be empty.");
                return;
            }

            Console.WriteLine("\nEnter Project Description:");
            string description = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(description))
            {
                Console.WriteLine("Project description cannot be empty.");
                return;
            }

            Console.WriteLine("\nEnter Project Budget:");
            if (!decimal.TryParse(Console.ReadLine(), out decimal budget))
            {
                Console.WriteLine("Invalid budget amount.");
                return;
            }

            if (budget <= 0)
            {
                Console.WriteLine("Budget must be greater than zero.");
                return;
            }

            Console.WriteLine("\nEnter Project Category:");
            string category = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(category))
            {
                Console.WriteLine("Project category cannot be empty.");
                return;
            }

            var project = new Project
            {
                Name = name,
                Description = description,
                Budget = budget,
                Сategory = category,
                ClientId = currentUser.UserID,
                Status = ProjectStatus.Open
            };

            _context.Projects.Add(project);
            _context.SaveChanges();

            Console.WriteLine("Project created successfully!");
        }
    }
}
