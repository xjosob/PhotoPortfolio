namespace PhotoPortfolio.Models
{
    public class Album
    {
        public int AlbumID { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? ThumbnailPath { get; set; }
        public ICollection<Photo>? Photos { get; set; }
    }
}
