using Freelance_Platform.Entities;
using System;
using System.Linq;

namespace Freelance_Platform.Logic
{
    public class ChatService
    {
        private readonly FreelanceDbContext _context;

        public ChatService(FreelanceDbContext context)
        {
            _context = context;
        }

        public void SendMessage()
        {
            Console.WriteLine("Enter your User ID (Sender): ");
            if (!int.TryParse(Console.ReadLine(), out int senderId))
            {
                Console.WriteLine("Invalid User ID.");
                return;
            }

            var sender = _context.Users.FirstOrDefault(u => u.UserID == senderId);

            if (sender == null)
            {
                Console.WriteLine("Sender not found.");
                return;
            }

            Console.WriteLine("Enter Receiver's User ID: ");
            if (!int.TryParse(Console.ReadLine(), out int receiverId))
            {
                Console.WriteLine("Invalid Receiver ID.");
                return;
            }

            var receiver = _context.Users.FirstOrDefault(u => u.UserID == receiverId);

            if (receiver == null)
            {
                Console.WriteLine("Receiver not found.");
                return;
            }

            if (senderId == receiverId)
            {
                Console.WriteLine("You cannot send a message to yourself.");
                return;
            }

            Console.WriteLine("\nEnter your message:");
            string content = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(content))
            {
                Console.WriteLine("Message cannot be empty.");
                return;
            }

            var chatMessage = new Chat
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Content = content,
                SentAt = DateTime.UtcNow
            };

            _context.Chats.Add(chatMessage);
            _context.SaveChanges();

            Console.WriteLine("Message sent successfully!");
        }
    }
}
