using Freelance_Platform.Entities;
using System;
using System.Linq;

namespace Freelance_Platform.Logic
{
    public class TransactionService
    {
        private readonly FreelanceDbContext _context;

        public TransactionService(FreelanceDbContext context)
        {
            _context = context;
        }

        public void Deposit(User currentUser)
        {
            if (currentUser == null)
            {
                Console.WriteLine("No user logged in.");
                return;
            }

            if (currentUser.Role?.Name != "Client")
            {
                Console.WriteLine("Only clients can deposit money.");
                return;
            }

            Console.WriteLine("Enter amount to deposit:");
            if (!decimal.TryParse(Console.ReadLine(), out decimal amount) || amount <= 0)
            {
                Console.WriteLine("Invalid amount.");
                return;
            }

            currentUser.Balance += amount;

            var transaction = new Transaction
            {
                UserId = currentUser.UserID,
                Amount = amount,
                Type = TransactionType.Deposit,
                Date = DateTime.UtcNow
            };

            _context.Transactions.Add(transaction);
            _context.SaveChanges();

            Console.WriteLine($"Deposit successful. New balance: {currentUser.Balance}");
        }

        public void Withdraw(User currentUser)
        {
            if (currentUser == null)
            {
                Console.WriteLine("No user logged in.");
                return;
            }

            Console.WriteLine("Enter amount to withdraw:");
            if (!decimal.TryParse(Console.ReadLine(), out decimal amount) || amount <= 0)
            {
                Console.WriteLine("Invalid amount.");
                return;
            }

            if (currentUser.Balance < amount)
            {
                Console.WriteLine("Insufficient funds.");
                return;
            }

            currentUser.Balance -= amount;

            var transaction = new Transaction
            {
                UserId = currentUser.UserID,
                Amount = amount,
                Type = TransactionType.Withdrawal,
                Date = DateTime.UtcNow
            };

            _context.Transactions.Add(transaction);
            _context.SaveChanges();

            Console.WriteLine($"Withdrawal successful. New balance: {currentUser.Balance}");
        }

        public void PayFreelancer(User currentUser)
        {
            if (currentUser == null)
            {
                Console.WriteLine("No user logged in.");
                return;
            }

            if (currentUser.Role?.Name != "Client")
            {
                Console.WriteLine("Only clients can pay freelancers.");
                return;
            }

            Console.WriteLine("Enter Project ID to pay:");
            if (!int.TryParse(Console.ReadLine(), out int projectId))
            {
                Console.WriteLine("Invalid Project ID.");
                return;
            }

            var project = _context.Projects.FirstOrDefault(p => p.ProjectId == projectId);

            if (project == null)
            {
                Console.WriteLine("Project not found.");
                return;
            }

            if (project.ClientId != currentUser.UserID)
            {
                Console.WriteLine("You are not the client of this project.");
                return;
            }

            if (!project.FreelancerId.HasValue)
            {
                Console.WriteLine("No freelancer assigned to this project.");
                return;
            }

            var freelancer = _context.Users.FirstOrDefault(u => u.UserID == project.FreelancerId.Value);

            if (freelancer == null)
            {
                Console.WriteLine("Freelancer not found.");
                return;
            }

            if (currentUser.Balance < project.Budget)
            {
                Console.WriteLine("Insufficient balance to pay the freelancer.");
                return;
            }

            currentUser.Balance -= project.Budget;
            freelancer.Balance += project.Budget;

            var transaction = new Transaction
            {
                UserId = currentUser.UserID,
                Amount = project.Budget,
                Type = TransactionType.Payment,
                Date = DateTime.UtcNow
            };

            _context.Transactions.Add(transaction);
            _context.SaveChanges();

            Console.WriteLine($"Successfully paid {project.Budget} to freelancer {freelancer.Name}.");
        }

    }
}
