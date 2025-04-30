using System;
using Freelance_Platform;
using Freelance_Platform.Entities;
using Freelance_Platform.Logic;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== Welcome to Freelance Platform ===\n");

        User currentUser = null;

        while (currentUser == null)
        {
            Console.WriteLine("1. Register");
            Console.WriteLine("2. Login");
            Console.WriteLine("0. Exit");

            Console.Write("\nChoose an option: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    UserRegistration.RegisterUser();
                    break;
                case "2":
                    currentUser = UserLogin.LoginUser();
                    break;
                case "0":
                    Console.WriteLine("Goodbye!");
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.\n");
                    break;
            }
        }

        using (var context = new FreelanceDbContext())
        {
            var projectService = new ProjectService(context);
            var proposalService = new ProposalService(context);
            var chatService = new ChatService(context);
            var transactionService = new TransactionService(context);
            var reviewService = new ReviewService(context);
            var freelancerSelectionService = new FreelancerSelectionService(context);


            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("\n=== Main Menu ===");
                Console.WriteLine("1. View Open Projects");
                if (currentUser.Role?.Name == "Client")
                {
                    Console.WriteLine("2. Create Project");
                    Console.WriteLine("3. Deposit Money");
                    Console.WriteLine("4. Pay Freelancer");
                    Console.WriteLine("5. Select Freelancer for Project");


                }
                Console.WriteLine("6. Send Message");
                Console.WriteLine("7. Withdraw Money");
                Console.WriteLine("8. Leave Review");
                if (currentUser.Role?.Name == "Freelancer")
                {
                    Console.WriteLine("9. Submit Proposal");
                    Console.WriteLine("10. View My Projects and Close");
                }
                Console.WriteLine("0. Logout");

                Console.Write("\nChoose an option: ");
                string action = Console.ReadLine();

                switch (action)
                {
                    case "1":
                        var openProjects = context.Projects
                            .Where(p => p.Status == ProjectStatus.Open)
                            .ToList();
                        Console.WriteLine("\n=== Open Projects ===");
                        foreach (var project in openProjects)
                        {
                            Console.WriteLine($"ID: {project.ProjectId} | Name: {project.Name} | Budget: {project.Budget} | Category: {project.Сategory}");
                        }
                        if (!openProjects.Any())
                        {
                            Console.WriteLine("No open projects available.");
                        }
                        break;

                    case "2":
                        if (currentUser.Role?.Name == "Client")
                        {
                            projectService.CreateProject(currentUser);
                        }
                        else
                        {
                            Console.WriteLine("Only clients can create projects.");
                        }
                        break;
                    case "3":
                        if (currentUser.Role?.Name == "Client")
                        {
                            transactionService.Deposit(currentUser);
                        }
                        else
                        {
                            Console.WriteLine("Only clients can deposit money.");
                        }
                        break;

                    case "4":
                        if (currentUser.Role?.Name == "Client")
                        {
                            transactionService.PayFreelancer(currentUser);
                        }
                        else
                        {
                            Console.WriteLine("Only clients can pay freelancers.");
                        }
                        break;

                    case "5":
                        if (currentUser.Role?.Name == "Client")
                        {
                            freelancerSelectionService.SelectFreelancer(currentUser);
                        }
                        else
                        {
                            Console.WriteLine("Only clients can select freelancers.");
                        }
                        break;

                    case "6":
                        chatService.SendMessage();
                        break;

                    case "7":
                        transactionService.Withdraw(currentUser);
                        break;

                    case "8":
                        reviewService.LeaveReview(currentUser);
                        break;

                    case "9":
                        if (currentUser.Role?.Name == "Freelancer")
                        {
                            proposalService.SubmitProposal(currentUser);
                        }
                        else
                        {
                            Console.WriteLine("Only freelancers can submit proposals.");
                        }
                        break;

                    case "10":
                        if (currentUser.Role?.Name == "Freelancer")
                        {
                            var freelancerService = new FreelancerProjectService(context);
                            freelancerService.ViewAndCloseProjects(currentUser);
                        }
                        else
                        {
                            Console.WriteLine("Only freelancers can access this option.");
                        }
                        break;

                    case "0":
                        Console.WriteLine("Logging out...");
                        exit = true;
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        Console.WriteLine("Session ended. Goodbye!");
    }
}
