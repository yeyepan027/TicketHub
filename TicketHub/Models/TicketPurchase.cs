using System;
using System.ComponentModel.DataAnnotations;

namespace TicketHub.Models
{
    public class Purchase
    {
        //Primary key for the Purchase entity
        public int PurchaseID { get; set; }

        [Required]
        public int EventID { get; set; }  // Foreign key to Event

        [Required]
        public int Tickets { get; set; }

        [Required, StringLength(100)]
        public string CustomerName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, StringLength(16)]
        public string CreditCard { get; set; }

        public DateTime PurchaseDate { get; set; }

        // Navigation property
        public Show Show { get; set; }


        public Purchase()
        {
            PurchaseDate = DateTime.Now;
        }
    }
}