using System.Data.SQLite;
using System.Security.Cryptography;
using System.Text;

namespace Core.Data;

public static class DataBase
{
    private static string DbFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "R2000DB.db");
    private static string ConnectionString => $"Data Source={DbFilePath};Version=3;";

    // Call this at app startup to ensure DB exists
    public static void Initialize()
    {
        if (!File.Exists(DbFilePath))
        {
            SQLiteConnection.CreateFile(DbFilePath);
            using var conn = new SQLiteConnection(ConnectionString);
            conn.Open();

            string createTable = @"
                CREATE TABLE Users (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    UserName TEXT NOT NULL UNIQUE,
                    PasswordHash TEXT NOT NULL,
                    Email TEXT,
                    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
                );";

            using var cmd = new SQLiteCommand(createTable, conn);
            cmd.ExecuteNonQuery();
        }
    }

    // Returns an open connection
    public static SQLiteConnection GetConnection()
    {
        var conn = new SQLiteConnection(ConnectionString);
        conn.Open();
        return conn;
    }

    // Hash password using SHA256
    private static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        byte[] bytes = Encoding.UTF8.GetBytes(password);
        byte[] hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }

    // Sign Up a new user
    public static bool SignUp(string userName, string password, string email = null)
    {
        string passwordHash = HashPassword(password);

        using var conn = GetConnection();
        string sql = "INSERT INTO Users (UserName, PasswordHash, Email) VALUES (@username, @passwordHash, @email);";

        using var cmd = new SQLiteCommand(sql, conn);
        cmd.Parameters.AddWithValue("@username", userName);
        cmd.Parameters.AddWithValue("@passwordHash", passwordHash);
        cmd.Parameters.AddWithValue("@email", string.IsNullOrEmpty(email) ? DBNull.Value : email);

        try
        {
            int result = cmd.ExecuteNonQuery();
            return result > 0;
        }
        catch (SQLiteException ex)
        {
            // Unique username violation or other DB errors
            Console.WriteLine("Error: " + ex.Message);
            return false;
        }
    }

    // Sign In: returns true if credentials are correct
    public static bool SignIn(string userName, string password)
    {
        string passwordHash = HashPassword(password);

        using var conn = GetConnection();
        string sql = "SELECT PasswordHash FROM Users WHERE UserName = @username AND IsActive = 1;";
        using var cmd = new SQLiteCommand(sql, conn);
        cmd.Parameters.AddWithValue("@username", userName);

        var result = cmd.ExecuteScalar();
        if (result == null) return false;

        string storedHash = result.ToString();
        return storedHash == passwordHash;
    }
}
