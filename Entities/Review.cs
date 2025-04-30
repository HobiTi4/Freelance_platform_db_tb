using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelance_Platform.Entities
{
    public class Review
    {
        public int ReviewId { get; set; }
        public int SenderId { get; set; }
        public User Sender { get; set; }
        public int ReceiverId { get; set; }
        public User Receiver { get; set; }
        public int Rating { get; set; }  
        public string Comment { get; set; }
        public DateTime Date { get; set; }
    }

}
