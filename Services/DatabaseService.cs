using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Microsoft.Data.Sqlite;
using smartLibraryForC_.Models;
using System.Linq;

namespace smartLibraryForC_.Services
{
    /// <summary>
    /// خدمة إدارة قاعدة البيانات SQLite
    /// </summary>
    public class DatabaseService
    {
        private readonly string _connectionString;
        private readonly string _dbPath;

        public DatabaseService()
        {
            _dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SmartLibrary.db");
            _connectionString = $"Data Source={_dbPath}";
            InitializeDatabase();
        }

        /// <summary>
        /// تهيئة قاعدة البيانات وإنشاء الجداول
        /// </summary>
        private void InitializeDatabase()
        {
            // التأكد من وجود المجلد
            var directory = Path.GetDirectoryName(_dbPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                // إنشاء جدول الكتب
                var createBooksTable = @"
                    CREATE TABLE IF NOT EXISTS Books (
                        BookId INTEGER PRIMARY KEY AUTOINCREMENT,
                        Title TEXT NOT NULL,
                        Author TEXT NOT NULL,
                        Description TEXT,
                        CoverUrl TEXT,
                        DownloadUrl TEXT,
                        LocalPath TEXT,
                        Isbn TEXT,
                        PageCount INTEGER,
                        PublishedDate TEXT,
                        CreatedAt TEXT NOT NULL
                    )";
                ExecuteNonQuery(connection, createBooksTable);

                // إنشاء جدول ربط المستخدمين بالكتب
                var createUserBooksTable = @"
                    CREATE TABLE IF NOT EXISTS UserBooks (
                        UserBookId INTEGER PRIMARY KEY AUTOINCREMENT,
                        BookId INTEGER NOT NULL,
                        Status INTEGER NOT NULL DEFAULT 0,
                        AddedAt TEXT NOT NULL,
                        LastReadAt TEXT,
                        CurrentPage INTEGER,
                        ProgressPercentage REAL,
                        FOREIGN KEY (BookId) REFERENCES Books(BookId)
                    )";
                ExecuteNonQuery(connection, createUserBooksTable);

                // إنشاء جدول السجلات
                var createLogsTable = @"
                    CREATE TABLE IF NOT EXISTS Logs (
                        LogId INTEGER PRIMARY KEY AUTOINCREMENT,
                        ActionType INTEGER NOT NULL,
                        Description TEXT NOT NULL,
                        Details TEXT,
                        BookId INTEGER,
                        CreatedAt TEXT NOT NULL
                    )";
                ExecuteNonQuery(connection, createLogsTable);

                // إنشاء جدول سجلات الطقس
                var createWeatherLogsTable = @"
                    CREATE TABLE IF NOT EXISTS WeatherLogs (
                        WeatherId INTEGER PRIMARY KEY AUTOINCREMENT,
                        Temperature REAL NOT NULL,
                        Condition TEXT NOT NULL,
                        Time TEXT NOT NULL,
                        CreatedAt TEXT NOT NULL
                    )";
                ExecuteNonQuery(connection, createWeatherLogsTable);

                // إنشاء فهارس لتحسين الأداء
                ExecuteNonQuery(connection, "CREATE INDEX IF NOT EXISTS idx_userbooks_bookid ON UserBooks(BookId)");
                ExecuteNonQuery(connection, "CREATE INDEX IF NOT EXISTS idx_logs_createdat ON Logs(CreatedAt)");
                
                // Migration: Add IsFavorite column if not exists
                try 
                {
                    ExecuteNonQuery(connection, "ALTER TABLE Books ADD COLUMN IsFavorite INTEGER DEFAULT 0");
                }
                catch { /* Column likely exists */ }
            }
        }

        private void ExecuteNonQuery(SqliteConnection connection, string commandText)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = commandText;
                command.ExecuteNonQuery();
            }
        }

        #region Books Operations

        /// <summary>
        /// إضافة كتاب جديد
        /// </summary>
        public int AddBook(Book book)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = @"
                    INSERT INTO Books (Title, Author, Description, CoverUrl, DownloadUrl, LocalPath, Isbn, PageCount, PublishedDate, CreatedAt, IsFavorite)
                    VALUES (@Title, @Author, @Description, @CoverUrl, @DownloadUrl, @LocalPath, @Isbn, @PageCount, @PublishedDate, @CreatedAt, @IsFavorite);
                    SELECT last_insert_rowid();";

                command.Parameters.AddWithValue("@Title", book.Title);
                command.Parameters.AddWithValue("@Author", book.Author);
                command.Parameters.AddWithValue("@Description", book.Description ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@CoverUrl", book.CoverUrl ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@DownloadUrl", book.DownloadUrl ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@LocalPath", book.LocalPath ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Isbn", book.Isbn ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@PageCount", book.PageCount ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@PublishedDate", book.PublishedDate?.ToString("yyyy-MM-dd") ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@CreatedAt", book.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"));
                command.Parameters.AddWithValue("@IsFavorite", book.IsFavorite ? 1 : 0);

                var result = command.ExecuteScalar();
                return Convert.ToInt32(result);
            }
        }

        /// <summary>
        /// تحديث مسار الملف المحلي
        /// </summary>
        public void UpdateBookLocalPath(int bookId, string localPath)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = "UPDATE Books SET LocalPath = @LocalPath WHERE BookId = @BookId";
                command.Parameters.AddWithValue("@BookId", bookId);
                command.Parameters.AddWithValue("@LocalPath", localPath);
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// الحصول على كتاب بالمعرف
        /// </summary>
        public Book GetBookById(int bookId)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Books WHERE BookId = @BookId";
                command.Parameters.AddWithValue("@BookId", bookId);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return ReadBookFromReader(reader);
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// الحصول على جميع الكتب
        /// </summary>
        public List<Book> GetAllBooks()
        {
            var books = new List<Book>();

            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Books ORDER BY CreatedAt DESC";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        books.Add(ReadBookFromReader(reader));
                    }
                }
            }

            return books;
        }

        /// <summary>
        /// حذف كتاب
        /// </summary>
        public void DeleteBook(int bookId)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                // حذف السجلات المرتبطة أولاً
                var deleteUserBooks = connection.CreateCommand();
                deleteUserBooks.CommandText = "DELETE FROM UserBooks WHERE BookId = @BookId";
                deleteUserBooks.Parameters.AddWithValue("@BookId", bookId);
                deleteUserBooks.ExecuteNonQuery();

                // حذف الكتاب
                var deleteBook = connection.CreateCommand();
                deleteBook.CommandText = "DELETE FROM Books WHERE BookId = @BookId";
                deleteBook.Parameters.AddWithValue("@BookId", bookId);
                deleteBook.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// تبديل حالة المفضلة للكتاب
        /// </summary>
        public void ToggleFavorite(int bookId, bool isFavorite)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "UPDATE Books SET IsFavorite = @IsFavorite WHERE BookId = @BookId";
                command.Parameters.AddWithValue("@BookId", bookId);
                command.Parameters.AddWithValue("@IsFavorite", isFavorite ? 1 : 0);
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// الحصول على الكتب المفضلة
        /// </summary>
        public List<Book> GetFavoriteBooks()
        {
            var books = new List<Book>();
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Books WHERE IsFavorite = 1 ORDER BY CreatedAt DESC";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        books.Add(ReadBookFromReader(reader));
                    }
                }
            }
            return books;
        }

        private Book ReadBookFromReader(SqliteDataReader reader)
        {
            return new Book
            {
                BookId = reader.GetInt32(0),
                Title = reader.GetString(1),
                Author = reader.GetString(2),
                Description = reader.IsDBNull(3) ? null : reader.GetString(3),
                CoverUrl = reader.IsDBNull(4) ? null : reader.GetString(4),
                DownloadUrl = reader.IsDBNull(5) ? null : reader.GetString(5),
                LocalPath = reader.IsDBNull(6) ? null : reader.GetString(6),
                Isbn = reader.IsDBNull(7) ? null : reader.GetString(7),
                PageCount = reader.IsDBNull(8) ? null : (int?)reader.GetInt64(8),
                PublishedDate = reader.IsDBNull(9) ? null : (DateTime?)DateTime.Parse(reader.GetString(9)),
                CreatedAt = DateTime.Parse(reader.GetString(10)),
                IsFavorite = !reader.IsDBNull(11) && reader.GetInt32(11) == 1
            };
        }

        #endregion

        #region UserBooks Operations

        /// <summary>
        /// إضافة كتاب إلى مكتبة المستخدم
        /// </summary>
        public int AddToUserLibrary(int bookId, ReadingStatus status = ReadingStatus.Downloaded)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = @"
                    INSERT INTO UserBooks (BookId, Status, AddedAt, LastReadAt, CurrentPage, ProgressPercentage)
                    VALUES (@BookId, @Status, @AddedAt, @LastReadAt, @CurrentPage, @ProgressPercentage);
                    SELECT last_insert_rowid();";

                command.Parameters.AddWithValue("@BookId", bookId);
                command.Parameters.AddWithValue("@Status", (int)status);
                command.Parameters.AddWithValue("@AddedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                command.Parameters.AddWithValue("@LastReadAt", DBNull.Value);
                command.Parameters.AddWithValue("@CurrentPage", DBNull.Value);
                command.Parameters.AddWithValue("@ProgressPercentage", DBNull.Value);

                var result = command.ExecuteScalar();
                return Convert.ToInt32(result);
            }
        }

        /// <summary>
        /// الحصول على الكتب في مكتبة المستخدم مع تفاصيل الكتب
        /// </summary>
        public List<UserBook> GetUserLibraryBooks()
        {
            var userBooks = new List<UserBook>();

            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = @"
                    SELECT ub.*, b.* FROM UserBooks ub
                    INNER JOIN Books b ON ub.BookId = b.BookId
                    ORDER BY ub.AddedAt DESC";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var userBook = new UserBook
                        {
                            UserBookId = reader.GetInt32(0),
                            BookId = reader.GetInt32(1),
                            Status = (ReadingStatus)reader.GetInt32(2),
                            AddedAt = DateTime.Parse(reader.GetString(3)),
                            LastReadAt = reader.IsDBNull(4) ? null : (DateTime?)DateTime.Parse(reader.GetString(4)),
                            CurrentPage = reader.IsDBNull(5) ? null : (int?)reader.GetInt64(5),
                            ProgressPercentage = reader.IsDBNull(6) ? null : (double?)reader.GetDouble(6),
                            Book = new Book
                            {
                                BookId = reader.GetInt32(7),
                                Title = reader.GetString(8),
                                Author = reader.GetString(9),
                                Description = reader.IsDBNull(10) ? null : reader.GetString(10),
                                CoverUrl = reader.IsDBNull(11) ? null : reader.GetString(11),
                                DownloadUrl = reader.IsDBNull(12) ? null : reader.GetString(12),
                                LocalPath = reader.IsDBNull(13) ? null : reader.GetString(13),
                                Isbn = reader.IsDBNull(14) ? null : reader.GetString(14),
                                PageCount = reader.IsDBNull(15) ? null : (int?)reader.GetInt64(15),
                                PublishedDate = reader.IsDBNull(16) ? null : (DateTime?)DateTime.Parse(reader.GetString(16)),
                                CreatedAt = DateTime.Parse(reader.GetString(17))
                            }
                        };

                        userBooks.Add(userBook);
                    }
                }
            }

            return userBooks;
        }

        /// <summary>
        /// تحديث حالة القراءة
        /// </summary>
        public void UpdateReadingStatus(int userBookId, ReadingStatus status, int? currentPage = null, double? progress = null)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = @"
                    UPDATE UserBooks
                    SET Status = @Status, CurrentPage = @CurrentPage, ProgressPercentage = @Progress, LastReadAt = @LastReadAt
                    WHERE UserBookId = @UserBookId";

                command.Parameters.AddWithValue("@UserBookId", userBookId);
                command.Parameters.AddWithValue("@Status", (int)status);
                command.Parameters.AddWithValue("@CurrentPage", currentPage ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Progress", progress ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@LastReadAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// حذف كتاب من مكتبة المستخدم
        /// </summary>
        public void RemoveFromUserLibrary(int userBookId)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = "DELETE FROM UserBooks WHERE UserBookId = @UserBookId";
                command.Parameters.AddWithValue("@UserBookId", userBookId);
                command.ExecuteNonQuery();
            }
        }

        #endregion

        #region Logs Operations

        /// <summary>
        /// إضافة سجل جديد
        /// </summary>
        public void AddLog(LogActionType actionType, string description, int? bookId = null, string details = null)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = @"
                    INSERT INTO Logs (ActionType, Description, Details, BookId, CreatedAt)
                    VALUES (@ActionType, @Description, @Details, @BookId, @CreatedAt)";

                command.Parameters.AddWithValue("@ActionType", (int)actionType);
                command.Parameters.AddWithValue("@Description", description);
                command.Parameters.AddWithValue("@Details", details ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@BookId", bookId ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@CreatedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// الحصول على السجلات الأخيرة
        /// </summary>
        public List<SystemLog> GetRecentLogs(int count = 50)
        {
            var logs = new List<SystemLog>();

            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = $"SELECT * FROM Logs ORDER BY CreatedAt DESC LIMIT {count}";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        logs.Add(new SystemLog
                        {
                            LogId = reader.GetInt32(0),
                            ActionType = (LogActionType)reader.GetInt32(1),
                            Description = reader.GetString(2),
                            Details = reader.IsDBNull(3) ? null : reader.GetString(3),
                            BookId = reader.IsDBNull(4) ? null : (int?)reader.GetInt64(4),
                            CreatedAt = DateTime.Parse(reader.GetString(5))
                        });
                    }
                }
            }

            return logs;
        }

        #endregion

        #region Weather Logs Operations

        /// <summary>
        /// حفظ قراءة الطقس
        /// </summary>
        public void SaveWeatherLog(WeatherInfo weather)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = @"
                    INSERT INTO WeatherLogs (Temperature, Condition, Time, CreatedAt)
                    VALUES (@Temperature, @Condition, @Time, @CreatedAt)";

                command.Parameters.AddWithValue("@Temperature", weather.Temperature);
                command.Parameters.AddWithValue("@Condition", weather.Condition);
                command.Parameters.AddWithValue("@Time", weather.Time.ToString("yyyy-MM-dd HH:mm:ss"));
                command.Parameters.AddWithValue("@CreatedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                command.ExecuteNonQuery();
            }
        }

        #endregion
    }
}
