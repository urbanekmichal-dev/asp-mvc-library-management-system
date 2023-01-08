namespace MVC_CRUD.Models
{
    public class BookViewModel
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public DateTime Published { get; set; }
        public string Publisher { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public IFormFile Image { get; set; }
        public bool Visible { get; set; }
    }
}
