using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelance_Platform.Entities
{
    public class Project
    {
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public decimal Budget { get; set; }

        public ProjectStatus Status { get; set; }

        public string Сategory { get; set; }

        public int ClientId { get; set; }

        public User Client { get; set; }

        public int? FreelancerId { get; set; }
        public User Freelancer { get; set; }
    }
    public enum ProjectStatus
    {
        Open,
        InProgress,
        Completed
    }
}
