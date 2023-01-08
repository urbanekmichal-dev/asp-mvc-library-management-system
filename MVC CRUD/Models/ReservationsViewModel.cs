using System.ComponentModel.DataAnnotations.Schema;
using MVC_CRUD.Models.Domain;

namespace MVC_CRUD.Models
{
    public class ReservationsViewModel
    {
        public Guid ReservationId { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public int daysLeft { get; set; }
        public bool is_finished { get; set; }

        public User User { get; set; }

        public Book Book { get; set; }
    }
}
