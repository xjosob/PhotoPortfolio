namespace PhotoPortfolio.Models
{
    public class Photo
    {
        public int PhotoID { get; set; }
        public string? Title { get; set; }
        public string? FilePath { get; set; }
        public int AlbumID { get; set; }
        public Album? Album { get; set; }
    }
}
