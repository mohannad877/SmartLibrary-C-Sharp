using System;

namespace smartLibraryForC_.Models
{
    /// <summary>
    /// نموذج ربط المستخدم بالكتاب مع حالة القراءة
    /// </summary>
    public class UserBook
    {
        public int UserBookId { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }
        public ReadingStatus Status { get; set; } = ReadingStatus.Downloaded;
        public DateTime AddedAt { get; set; } = DateTime.Now;
        public DateTime? LastReadAt { get; set; }
        public int? CurrentPage { get; set; }
        public double? ProgressPercentage { get; set; }
    }

    /// <summary>
    /// حالات قراءة الكتاب
    /// </summary>
    public enum ReadingStatus
    {
        Downloaded = 0,
        Reading = 1,
        Finished = 2
    }
}
