using Freelance_Platform.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelance_Platform.Logic
{
    public class FreelancerSelectionService
    {
        private readonly FreelanceDbContext _context;

        public FreelancerSelectionService(FreelanceDbContext context)
        {
            _context = context;
        }

        public void SelectFreelancer(User currentUser)
        {
            if (currentUser == null)
            {
                Console.WriteLine("No user logged in.");
                return;
            }

            if (currentUser.Role?.Name != "Client")
            {
                Console.WriteLine("Only clients can select freelancers.");
                return;
            }

            Console.WriteLine("Enter your Project ID:");
            if (!int.TryParse(Console.ReadLine(), out int projectId))
            {
                Console.WriteLine("Invalid Project ID.");
                return;
            }

            var project = _context.Projects.FirstOrDefault(p => p.ProjectId == projectId && p.ClientId == currentUser.UserID);

            if (project == null)
            {
                Console.WriteLine("Project not found or you are not the client of this project.");
                return;
            }

            var proposals = _context.Proposals
                .Where(p => p.ProjectId == projectId)
                .Select(p => new
                {
                    p.ProposalId,
                    Freelancer = p.Freelancer,
                    p.ProposedPrice,
                    p.SubmittedAt
                })
                .ToList();

            if (!proposals.Any())
            {
                Console.WriteLine("No proposals for this project.");
                return;
            }

            Console.WriteLine("=== Proposals ===");
            foreach (var proposal in proposals)
            {
                Console.WriteLine($"\nProposal ID: {proposal.ProposalId}");
                Console.WriteLine($"Freelancer Name: {proposal.Freelancer.Name}");
                Console.WriteLine($"Email: {proposal.Freelancer.Email}");
                Console.WriteLine($"Balance: {proposal.Freelancer.Rating}");
                Console.WriteLine($"Proposed Price: {proposal.ProposedPrice}");
                Console.WriteLine($"Submitted At: {proposal.SubmittedAt.ToShortDateString()}");
               
            }

            Console.WriteLine("\nEnter Proposal ID to select freelancer:");
            if (!int.TryParse(Console.ReadLine(), out int proposalId))
            {
                Console.WriteLine("Invalid Proposal ID.");
                return;
            }

            var selectedProposal = _context.Proposals.FirstOrDefault(p => p.ProposalId == proposalId && p.ProjectId == projectId);

            if (selectedProposal == null)
            {
                Console.WriteLine("Proposal not found.");
                return;
            }

            project.FreelancerId = selectedProposal.FreelancerId;
            project.Status = ProjectStatus.InProgress;

            _context.SaveChanges();

            Console.WriteLine($"Freelancer {selectedProposal.Freelancer.Name} selected successfully! Project is now In Progress.");
        }
    }


}
