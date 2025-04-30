using Freelance_Platform.Entities;
using System;
using System.Linq;

namespace Freelance_Platform.Logic
{
    public class ProposalService
    {
        private readonly FreelanceDbContext _context;

        public ProposalService(FreelanceDbContext context)
        {
            _context = context;
        }

        public void SubmitProposal(User currentUser)
        {
            if (currentUser == null)
            {
                Console.WriteLine("No user logged in.");
                return;
            }

            if (currentUser.Role?.Name != "Freelancer")
            {
                Console.WriteLine("Only freelancers can submit proposals.");
                return;
            }

            Console.WriteLine("\nAvailable Open Projects:");
            var openProjects = _context.Projects
                .Where(p => p.Status == ProjectStatus.Open)
                .ToList();

            if (openProjects.Count == 0)
            {
                Console.WriteLine("There are currently no open projects.");
                return;
            }

            foreach (var project in openProjects)
            {
                Console.WriteLine($"ID: {project.ProjectId} | Name: {project.Name} | Budget: {project.Budget} | Category: {project.Сategory}");
            }

            Console.WriteLine("\nEnter the Project ID you want to submit a proposal for:");
            if (!int.TryParse(Console.ReadLine(), out int projectId))
            {
                Console.WriteLine("Invalid Project ID.");
                return;
            }

            var selectedProject = openProjects.FirstOrDefault(p => p.ProjectId == projectId);

            if (selectedProject == null)
            {
                Console.WriteLine("Project not found or is no longer open.");
                return;
            }

            Console.WriteLine($"Enter your proposed price (Project Budget: {selectedProject.Budget}):");
            if (!decimal.TryParse(Console.ReadLine(), out decimal proposedPrice))
            {
                Console.WriteLine("Invalid price.");
                return;
            }

            if (proposedPrice <= 0)
            {
                Console.WriteLine("Price must be greater than zero.");
                return;
            }

            var existingProposal = _context.Proposals
                .FirstOrDefault(p => p.ProjectId == projectId && p.FreelancerId == currentUser.UserID);

            if (existingProposal != null)
            {
                Console.WriteLine("You have already submitted a proposal for this project.");
                return;
            }

            var proposal = new Proposal
            {
                FreelancerId = currentUser.UserID,
                ProjectId = projectId,
                ProposedPrice = proposedPrice,
                SubmittedAt = DateTime.UtcNow
            };

            _context.Proposals.Add(proposal);
            _context.SaveChanges();

            Console.WriteLine("Proposal submitted successfully!");
        }

    }
}
