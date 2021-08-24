namespace Homework.Dotnet.Tasks.Documents
{
    public class BookSuggestResponse
    {
        public string Id { get; set; }

        public int BookId { get; set; }

        public string BookTitle { get; set; }

        public string AuthorEmail { get; set; }

        public string AuthorLastName { get; set; }

        public string SuggestName { get; set; }

        public double Score { get; set; }

        public long Frequency { get; set; }
    }
}
