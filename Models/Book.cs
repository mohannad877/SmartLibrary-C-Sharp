using System;

namespace smartLibraryForC_.Models
{
    /// <summary>
    /// نموذج بيانات الكتاب
    /// </summary>
    public class Book
    {
        public int BookId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Description { get; set; }
        public string CoverUrl { get; set; }
        public string DownloadUrl { get; set; }
        public System.Collections.Generic.List<string> AlternativeDownloadUrls { get; set; } = new System.Collections.Generic.List<string>();
        public string LocalPath { get; set; }
        public string Isbn { get; set; }
        public int? PageCount { get; set; }
        public DateTime? PublishedDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsFavorite { get; set; } = false;
    }
}
