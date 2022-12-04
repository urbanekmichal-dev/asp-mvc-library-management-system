namespace MVC_CRUD.Models.Domain
{
    public class Book
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string PublishingHouse { get; set; }
        public int YearOfPublishment { get; set; }
    }
}
