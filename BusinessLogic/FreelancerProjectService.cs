using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Freelance_Platform.Entities;

namespace Freelance_Platform.Logic
{
    public class FreelancerProjectService
    {
        private readonly FreelanceDbContext _context;

        public FreelancerProjectService(FreelanceDbContext context)
        {
            _context = context;
        }

        public void ViewAndCloseProjects(User currentUser)
        {
            if (currentUser == null)
            {
                Console.WriteLine("No user logged in.");
                return;
            }

            if (currentUser.Role?.Name != "Freelancer")
            {
                Console.WriteLine("Only freelancers can access this menu.");
                return;
            }

            var myProjects = _context.Projects
                .Where(p => p.FreelancerId == currentUser.UserID)
                .ToList();

            if (!myProjects.Any())
            {
                Console.WriteLine("You are not assigned to any projects.");
                return;
            }

            Console.WriteLine("\n=== My Projects ===");
            foreach (var project in myProjects)
            {
                Console.WriteLine($"Project ID: {project.ProjectId} | Name: {project.Name} | Status: {project.Status}");
            }

            Console.WriteLine("\nEnter Project ID to close (or press Enter to cancel):");
            string input = Console.ReadLine();
            if (string.IsNullOrEmpty(input)) return;

            if (!int.TryParse(input, out int projectId))
            {
                Console.WriteLine("Invalid Project ID.");
                return;
            }

            var selectedProject = _context.Projects.FirstOrDefault(p => p.ProjectId == projectId && p.FreelancerId == currentUser.UserID);

            if (selectedProject == null)
            {
                Console.WriteLine("Project not found or you are not assigned to it.");
                return;
            }

            if (selectedProject.Status == ProjectStatus.Completed)
            {
                Console.WriteLine("Project is already completed.");
                return;
            }

            selectedProject.Status = ProjectStatus.Completed;
            _context.SaveChanges();

            Console.WriteLine($"Project '{selectedProject.Name}' marked as Completed!");

            LeaveReviewForClient(currentUser, selectedProject.ClientId);
        }

        private void LeaveReviewForClient(User sender, int clientId)
        {
            var reviewService = new ReviewService(_context);

            Console.WriteLine("\nNow you can leave a review for your client:");
            Console.WriteLine($"(Client ID: {clientId})");

            reviewService.LeaveReview(sender);
        }

    }
}
