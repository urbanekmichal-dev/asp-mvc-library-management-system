using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC_CRUD.Models.Domain
{
    public class Reservation
    {
        [Key]
        public Guid ReservationId { get; set; }
        public DateTime startDate   { get; set; }
        public DateTime endDate { get; set; }
        public int daysLeft { get; set; }
        public bool is_finished { get; set; }
        public BookState BookState { get; set; }
     
        public Guid BookId { get; set; } 
        public Guid UserId { get; set; }
        [ForeignKey("Id")]
        public virtual User User { get; set; }
        [ForeignKey("BookId")]

        public virtual Book Book { get; set; }

    }

    public enum BookState
    {
        Ongoing,
        Pending,
        Finished,
    }
}
