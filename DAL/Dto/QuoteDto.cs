namespace DAL.Dto
{
    public class QuoteDto
    {
        public int? Id { get; set; }

        public string Quote { get; set; }

        public int? AuthorId { get; set; }
    }
}
