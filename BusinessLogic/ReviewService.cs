using Freelance_Platform.Entities;
using System;
using System.Linq;

namespace Freelance_Platform.Logic
{
    public class ReviewService
    {
        private readonly FreelanceDbContext _context;

        public ReviewService(FreelanceDbContext context)
        {
            _context = context;
        }

        public void LeaveReview(User currentUser)
        {
            if (currentUser == null)
            {
                Console.WriteLine("No user logged in.");
                return;
            }

            Console.WriteLine("\n=== Leave a Review ===");

            Console.WriteLine("Enter the ID of the user you want to leave a review for:");
            if (!int.TryParse(Console.ReadLine(), out int receiverId))
            {
                Console.WriteLine("Invalid ID.");
                return;
            }

            if (receiverId == currentUser.UserID)
            {
                Console.WriteLine("You cannot leave a review for yourself.");
                return;
            }

            var receiver = _context.Users.FirstOrDefault(u => u.UserID == receiverId);

            if (receiver == null)
            {
                Console.WriteLine("Receiver not found.");
                return;
            }

            Console.WriteLine("Enter rating (1-5):");
            if (!int.TryParse(Console.ReadLine(), out int rating) || rating < 1 || rating > 5)
            {
                Console.WriteLine("Invalid rating.");
                return;
            }

            Console.WriteLine("Enter a comment (optional):");
            string comment = Console.ReadLine();

            var review = new Review
            {
                SenderId = currentUser.UserID,
                ReceiverId = receiverId,
                Rating = rating,
                Comment = comment,
                Date = DateTime.UtcNow
            };

            _context.Reviews.Add(review);
            _context.SaveChanges();

            Console.WriteLine("Review submitted successfully!");
        }
    }
}
