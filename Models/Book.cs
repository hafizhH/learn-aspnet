namespace LearnAPI.Models
{
    public class Book
    {
        public int id { get; set; }
        public string? name { get; set; }
        public int year { get; set; }
        public string author { get; set; }
        public string summary { get; set; }
        public string publisher { get; set; }
        public int pageCount { get; set; }
        public int readPage { get; set; }
        public bool finished { get; set; }
        public bool reading { get; set; }
        public long insertedAt { get; set; }
        public long updatedAt { get; set; }
    }
}
