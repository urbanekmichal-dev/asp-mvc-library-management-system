using System.ComponentModel.DataAnnotations;

namespace MVC_CRUD.Models.Domain
{
    public class Book
    {
        [Key]
        public Guid BookId { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        [DataType(DataType.Date)]
        public DateTime Published { get; set; }
        public string Publisher { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public bool Visible { get;set; }

    }
}
