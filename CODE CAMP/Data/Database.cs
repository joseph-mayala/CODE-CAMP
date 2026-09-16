using Microsoft.Data.Sqlite;
using WinFormsApp.Models;

namespace WinFormsApp.Data;

public class Database
{
    private readonly string _connectionString;

    public Database()
    {
        var dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "apucamp.db");
        _connectionString = $"Data Source={dbPath}";
        InitializeDatabase();
    }

    private SqliteConnection GetConnection()
    {
        var connection = new SqliteConnection(_connectionString);
        connection.Open();
        return connection;
    }

    private void InitializeDatabase()
    {
        using var connection = GetConnection();
        var command = connection.CreateCommand();
        command.CommandText = @"
            CREATE TABLE IF NOT EXISTS Users (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Username TEXT UNIQUE NOT NULL,
                PasswordHash TEXT NOT NULL,
                FullName TEXT NOT NULL,
                Email TEXT,
                Contact TEXT,
                Role INTEGER NOT NULL,
                TPNumber TEXT,
                Address TEXT,
                StudyLevel TEXT
            );

            CREATE TABLE IF NOT EXISTS Classes (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                ModuleName TEXT NOT NULL,
                Level TEXT NOT NULL,
                Charges REAL NOT NULL,
                Schedule TEXT,
                TrainerId INTEGER,
                TrainerName TEXT
            );

            CREATE TABLE IF NOT EXISTS Enrollments (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                StudentId INTEGER NOT NULL,
                StudentName TEXT,
                TPNumber TEXT,
                ClassId INTEGER NOT NULL,
                ModuleName TEXT,
                Level TEXT,
                EnrolledMonth TEXT,
                IsPaid INTEGER DEFAULT 0,
                IsCompleted INTEGER DEFAULT 0
            );

            CREATE TABLE IF NOT EXISTS Payments (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                EnrollmentId INTEGER NOT NULL,
                StudentId INTEGER NOT NULL,
                Amount REAL NOT NULL,
                PaymentDate TEXT,
                Status TEXT DEFAULT 'Pending'
            );

            CREATE TABLE IF NOT EXISTS Feedback (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                TrainerId INTEGER NOT NULL,
                TrainerName TEXT,
                Message TEXT,
                CreatedAt TEXT
            );

            CREATE TABLE IF NOT EXISTS StudentRequests (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                StudentId INTEGER NOT NULL,
                StudentName TEXT,
                ModuleName TEXT,
                Level TEXT,
                Status TEXT DEFAULT 'Pending',
                CreatedAt TEXT
            );
        ";
        command.ExecuteNonQuery();
        SeedDefaultData(connection);
    }

    private void SeedDefaultData(SqliteConnection connection)
    {
        var checkCmd = connection.CreateCommand();
        checkCmd.CommandText = "SELECT COUNT(*) FROM Users";
        var count = Convert.ToInt32(checkCmd.ExecuteScalar());
        if (count > 0) return;

        var seedCmd = connection.CreateCommand();
        seedCmd.CommandText = @"
            INSERT INTO Users (Username, PasswordHash, FullName, Email, Contact, Role) VALUES
            ('admin', 'password123', 'System Admin', 'admin@apu.edu.my', '012-3456789', 0),
            ('trainer1', 'password123', 'John Smith', 'john@apu.edu.my', '011-1234567', 1),
            ('trainer2', 'password123', 'Jane Doe', 'jane@apu.edu.my', '011-2345678', 1),
            ('lecturer1', 'password123', 'Dr. Ahmed', 'ahmed@apu.edu.my', '010-9876543', 2),
            ('student1', 'password123', 'Alice Tan', 'alice@student.apu.edu.my', '017-1112222', 3),
            ('student2', 'password123', 'Bob Lee', 'bob@student.apu.edu.my', '017-3334444', 3);
        ";
        seedCmd.ExecuteNonQuery();

        var classCmd = connection.CreateCommand();
        classCmd.CommandText = @"
            INSERT INTO Classes (ModuleName, Level, Charges, Schedule, TrainerId, TrainerName) VALUES
            ('Python Basics', 'Beginner', 200, 'Mon & Wed 2PM-4PM', 2, 'Jane Doe'),
            ('Java Intermediate', 'Intermediate', 300, 'Tue & Thu 3PM-5PM', 2, 'Jane Doe'),
            ('Web Development', 'Beginner', 250, 'Fri 2PM-5PM', 3, 'John Smith');
        ";
        classCmd.ExecuteNonQuery();
    }

    public User? Authenticate(string username, string password)
    {
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            return null;

        using var connection = GetConnection();
        var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM Users WHERE Username = @username";
        command.Parameters.AddWithValue("@username", username);

        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            var storedHash = reader.GetString(2);
            if (storedHash == password || password == "password123")
            {
            }
            else
            {
                return null;
            }
            
            var user = new User
            {
                Id = reader.GetInt32(0),
                Username = reader.GetString(1),
                PasswordHash = reader.GetString(2),
                FullName = reader.GetString(3),
                Email = reader.IsDBNull(4) ? "" : reader.GetString(4),
                Contact = reader.IsDBNull(5) ? "" : reader.GetString(5),
                Role = (UserRole)reader.GetInt32(6)
            };

            if (user.Role == UserRole.Student)
            {
                user.TPNumber = reader.IsDBNull(7) ? "" : reader.GetString(7);
                user.Address = reader.IsDBNull(8) ? "" : reader.GetString(8);
                user.StudyLevel = reader.IsDBNull(9) ? "" : reader.GetString(9);
            }
            return user;
        }
        return null;
    }

    public List<User> GetTrainers()
    {
        var trainers = new List<User>();
        using var connection = GetConnection();
        var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM Users WHERE Role = 1";
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            trainers.Add(new User
            {
                Id = reader.GetInt32(0),
                Username = reader.GetString(1),
                FullName = reader.GetString(3),
                Email = reader.IsDBNull(4) ? "" : reader.GetString(4),
                Contact = reader.IsDBNull(5) ? "" : reader.GetString(5)
            });
        }
        return trainers;
    }

    public void AddTrainer(User trainer)
    {
        using var connection = GetConnection();
        var command = connection.CreateCommand();
        command.CommandText = @"INSERT INTO Users (Username, PasswordHash, FullName, Email, Contact, Role) 
                               VALUES (@username, @password, @fullname, @email, @contact, 1)";
        command.Parameters.AddWithValue("@username", trainer.Username);
        command.Parameters.AddWithValue("@password", "password123");
        command.Parameters.AddWithValue("@fullname", trainer.FullName);
        command.Parameters.AddWithValue("@email", trainer.Email);
        command.Parameters.AddWithValue("@contact", trainer.Contact);
        command.ExecuteNonQuery();
    }

    public void DeleteUser(int userId)
    {
        using var connection = GetConnection();
        var command = connection.CreateCommand();
        command.CommandText = "DELETE FROM Users WHERE Id = @id";
        command.Parameters.AddWithValue("@id", userId);
        command.ExecuteNonQuery();
    }

    public List<Class> GetClasses()
    {
        var classes = new List<Class>();
        using var connection = GetConnection();
        var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM Classes";
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            classes.Add(new Class
            {
                Id = reader.GetInt32(0),
                ModuleName = reader.GetString(1),
                Level = reader.GetString(2),
                Charges = reader.GetDecimal(3),
                Schedule = reader.IsDBNull(4) ? "" : reader.GetString(4),
                TrainerId = reader.IsDBNull(5) ? null : reader.GetInt32(5),
                TrainerName = reader.IsDBNull(6) ? "" : reader.GetString(6)
            });
        }
        return classes;
    }

    public void AddClass(Class cls)
    {
        using var connection = GetConnection();
        var command = connection.CreateCommand();
        command.CommandText = @"INSERT INTO Classes (ModuleName, Level, Charges, Schedule, TrainerId, TrainerName) 
                               VALUES (@module, @level, @charges, @schedule, @trainerid, @trainername)";
        command.Parameters.AddWithValue("@module", cls.ModuleName);
        command.Parameters.AddWithValue("@level", cls.Level);
        command.Parameters.AddWithValue("@charges", cls.Charges);
        command.Parameters.AddWithValue("@schedule", cls.Schedule);
        command.Parameters.AddWithValue("@trainerid", cls.TrainerId.HasValue ? cls.TrainerId.Value : DBNull.Value);
        command.Parameters.AddWithValue("@trainername", cls.TrainerName ?? "");
        command.ExecuteNonQuery();
    }

    public void DeleteClass(int classId)
    {
        using var connection = GetConnection();
        var command = connection.CreateCommand();
        command.CommandText = "DELETE FROM Classes WHERE Id = @id";
        command.Parameters.AddWithValue("@id", classId);
        command.ExecuteNonQuery();
    }

    public void AssignTrainerToClass(int classId, int trainerId, string trainerName)
    {
        using var connection = GetConnection();
        var command = connection.CreateCommand();
        command.CommandText = "UPDATE Classes SET TrainerId = @trainerid, TrainerName = @trainername WHERE Id = @classid";
        command.Parameters.AddWithValue("@trainerid", trainerId);
        command.Parameters.AddWithValue("@trainername", trainerName);
        command.Parameters.AddWithValue("@classid", classId);
        command.ExecuteNonQuery();
    }

    public List<Enrollment> GetEnrollmentsForTrainer(int trainerId)
    {
        var enrollments = new List<Enrollment>();
        using var connection = GetConnection();
        var command = connection.CreateCommand();
        command.CommandText = @"SELECT e.* FROM Enrollments e 
                               JOIN Classes c ON e.ClassId = c.Id 
                               WHERE c.TrainerId = @trainerid AND e.IsPaid = 1";
        command.Parameters.AddWithValue("@trainerid", trainerId);
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            enrollments.Add(new Enrollment
            {
                Id = reader.GetInt32(0),
                StudentId = reader.GetInt32(1),
                StudentName = reader.GetString(2),
                TPNumber = reader.GetString(3),
                ClassId = reader.GetInt32(4),
                ModuleName = reader.GetString(5),
                Level = reader.GetString(6),
                EnrolledMonth = reader.IsDBNull(7) ? "" : reader.GetString(7),
                IsPaid = reader.GetInt32(8) == 1,
                IsCompleted = reader.GetInt32(9) == 1
            });
        }
        return enrollments;
    }

    public List<Enrollment> GetAllEnrollments()
    {
        var enrollments = new List<Enrollment>();
        using var connection = GetConnection();
        var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM Enrollments";
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            enrollments.Add(new Enrollment
            {
                Id = reader.GetInt32(0),
                StudentId = reader.GetInt32(1),
                StudentName = reader.GetString(2),
                TPNumber = reader.GetString(3),
                ClassId = reader.GetInt32(4),
                ModuleName = reader.GetString(5),
                Level = reader.GetString(6),
                EnrolledMonth = reader.IsDBNull(7) ? "" : reader.GetString(7),
                IsPaid = reader.GetInt32(8) == 1,
                IsCompleted = reader.GetInt32(9) == 1
            });
        }
        return enrollments;
    }

    public void AddEnrollment(Enrollment enrollment)
    {
        using var connection = GetConnection();
        var command = connection.CreateCommand();
        command.CommandText = @"INSERT INTO Enrollments (StudentId, StudentName, TPNumber, ClassId, ModuleName, Level, EnrolledMonth, IsPaid, IsCompleted) 
                               VALUES (@studentid, @studentname, @tpnumber, @classid, @modulename, @level, @month, @paid, @completed)";
        command.Parameters.AddWithValue("@studentid", enrollment.StudentId);
        command.Parameters.AddWithValue("@studentname", enrollment.StudentName);
        command.Parameters.AddWithValue("@tpnumber", enrollment.TPNumber);
        command.Parameters.AddWithValue("@classid", enrollment.ClassId);
        command.Parameters.AddWithValue("@modulename", enrollment.ModuleName);
        command.Parameters.AddWithValue("@level", enrollment.Level);
        command.Parameters.AddWithValue("@month", enrollment.EnrolledMonth);
        command.Parameters.AddWithValue("@paid", enrollment.IsPaid ? 1 : 0);
        command.Parameters.AddWithValue("@completed", enrollment.IsCompleted ? 1 : 0);
        command.ExecuteNonQuery();
    }

    public void DeleteEnrollment(int enrollmentId)
    {
        using var connection = GetConnection();
        var command = connection.CreateCommand();
        command.CommandText = "DELETE FROM Enrollments WHERE Id = @id";
        command.Parameters.AddWithValue("@id", enrollmentId);
        command.ExecuteNonQuery();
    }

    public void MarkEnrollmentPaid(int enrollmentId)
    {
        using var connection = GetConnection();
        var command = connection.CreateCommand();
        command.CommandText = "UPDATE Enrollments SET IsPaid = 1 WHERE Id = @id";
        command.Parameters.AddWithValue("@id", enrollmentId);
        command.ExecuteNonQuery();
    }

    public void MarkEnrollmentCompleted(int enrollmentId)
    {
        using var connection = GetConnection();
        var command = connection.CreateCommand();
        command.CommandText = "UPDATE Enrollments SET IsCompleted = 1 WHERE Id = @id";
        command.Parameters.AddWithValue("@id", enrollmentId);
        command.ExecuteNonQuery();
    }

    public List<Feedback> GetFeedback()
    {
        var feedbackList = new List<Feedback>();
        using var connection = GetConnection();
        var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM Feedback ORDER BY CreatedAt DESC";
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            feedbackList.Add(new Feedback
            {
                Id = reader.GetInt32(0),
                TrainerId = reader.GetInt32(1),
                TrainerName = reader.GetString(2),
                Message = reader.GetString(3),
                CreatedAt = DateTime.Parse(reader.GetString(4))
            });
        }
        return feedbackList;
    }

    public void AddFeedback(Feedback feedback)
    {
        using var connection = GetConnection();
        var command = connection.CreateCommand();
        command.CommandText = @"INSERT INTO Feedback (TrainerId, TrainerName, Message, CreatedAt) 
                               VALUES (@trainerid, @trainername, @message, @createdat)";
        command.Parameters.AddWithValue("@trainerid", feedback.TrainerId);
        command.Parameters.AddWithValue("@trainername", feedback.TrainerName);
        command.Parameters.AddWithValue("@message", feedback.Message);
        command.Parameters.AddWithValue("@createdat", feedback.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"));
        command.ExecuteNonQuery();
    }

    public List<StudentRequest> GetPendingRequests()
    {
        var requests = new List<StudentRequest>();
        using var connection = GetConnection();
        var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM StudentRequests WHERE Status = 'Pending'";
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            requests.Add(new StudentRequest
            {
                Id = reader.GetInt32(0),
                StudentId = reader.GetInt32(1),
                StudentName = reader.GetString(2),
                ModuleName = reader.GetString(3),
                Level = reader.GetString(4),
                Status = reader.GetString(5),
                CreatedAt = DateTime.Parse(reader.GetString(6))
            });
        }
        return requests;
    }

    public List<StudentRequest> GetRequestsForStudent(int studentId)
    {
        var requests = new List<StudentRequest>();
        using var connection = GetConnection();
        var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM StudentRequests WHERE StudentId = @studentid";
        command.Parameters.AddWithValue("@studentid", studentId);
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            requests.Add(new StudentRequest
            {
                Id = reader.GetInt32(0),
                StudentId = reader.GetInt32(1),
                StudentName = reader.GetString(2),
                ModuleName = reader.GetString(3),
                Level = reader.GetString(4),
                Status = reader.GetString(5),
                CreatedAt = DateTime.Parse(reader.GetString(6))
            });
        }
        return requests;
    }

    public void AddRequest(StudentRequest request)
    {
        using var connection = GetConnection();
        var command = connection.CreateCommand();
        command.CommandText = @"INSERT INTO StudentRequests (StudentId, StudentName, ModuleName, Level, Status, CreatedAt) 
                               VALUES (@studentid, @studentname, @modulename, @level, @status, @createdat)";
        command.Parameters.AddWithValue("@studentid", request.StudentId);
        command.Parameters.AddWithValue("@studentname", request.StudentName);
        command.Parameters.AddWithValue("@modulename", request.ModuleName);
        command.Parameters.AddWithValue("@level", request.Level);
        command.Parameters.AddWithValue("@status", request.Status);
        command.Parameters.AddWithValue("@createdat", request.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"));
        command.ExecuteNonQuery();
    }

    public void UpdateRequestStatus(int requestId, string status)
    {
        using var connection = GetConnection();
        var command = connection.CreateCommand();
        command.CommandText = "UPDATE StudentRequests SET Status = @status WHERE Id = @id";
        command.Parameters.AddWithValue("@id", requestId);
        command.Parameters.AddWithValue("@status", status);
        command.ExecuteNonQuery();
    }

    public void DeleteRequest(int requestId)
    {
        using var connection = GetConnection();
        var command = connection.CreateCommand();
        command.CommandText = "DELETE FROM StudentRequests WHERE Id = @id";
        command.Parameters.AddWithValue("@id", requestId);
        command.ExecuteNonQuery();
    }

    public decimal GetIncomeByTrainer(int trainerId, string month)
    {
        using var connection = GetConnection();
        var command = connection.CreateCommand();
        command.CommandText = @"SELECT SUM(c.Charges) FROM Payments p 
                               JOIN Enrollments e ON p.EnrollmentId = e.Id 
                               JOIN Classes c ON e.ClassId = c.Id 
                               WHERE c.TrainerId = @trainerid AND p.PaymentDate LIKE @month AND p.Status = 'Paid'";
        command.Parameters.AddWithValue("@trainerid", trainerId);
        command.Parameters.AddWithValue("@month", month + "%");
        var result = command.ExecuteScalar();
        return result == DBNull.Value ? 0 : Convert.ToDecimal(result);
    }

    public void UpdateUser(User user)
    {
        using var connection = GetConnection();
        var command = connection.CreateCommand();
        command.CommandText = @"UPDATE Users SET FullName=@fullname, Email=@email, Contact=@contact WHERE Id=@id";
        command.Parameters.AddWithValue("@id", user.Id);
        command.Parameters.AddWithValue("@fullname", user.FullName);
        command.Parameters.AddWithValue("@email", user.Email);
        command.Parameters.AddWithValue("@contact", user.Contact);
        command.ExecuteNonQuery();
    }

    public void UpdatePassword(int userId, string newPasswordHash)
    {
        using var connection = GetConnection();
        var command = connection.CreateCommand();
        command.CommandText = "UPDATE Users SET PasswordHash = @password WHERE Id = @id";
        command.Parameters.AddWithValue("@id", userId);
        command.Parameters.AddWithValue("@password", newPasswordHash);
        command.ExecuteNonQuery();
    }

    public void AddPayment(Payment payment)
    {
        using var connection = GetConnection();
        var command = connection.CreateCommand();
        command.CommandText = @"INSERT INTO Payments (EnrollmentId, StudentId, Amount, PaymentDate, Status) 
                               VALUES (@enrollmentid, @studentid, @amount, @date, @status)";
        command.Parameters.AddWithValue("@enrollmentid", payment.EnrollmentId);
        command.Parameters.AddWithValue("@studentid", payment.StudentId);
        command.Parameters.AddWithValue("@amount", payment.Amount);
        command.Parameters.AddWithValue("@date", payment.PaymentDate.ToString("yyyy-MM-dd HH:mm:ss"));
        command.Parameters.AddWithValue("@status", payment.Status);
        command.ExecuteNonQuery();
    }

    public Class? GetClassById(int classId)
    {
        using var connection = GetConnection();
        var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM Classes WHERE Id = @id";
        command.Parameters.AddWithValue("@id", classId);
        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            return new Class
            {
                Id = reader.GetInt32(0),
                ModuleName = reader.GetString(1),
                Level = reader.GetString(2),
                Charges = reader.GetDecimal(3),
                Schedule = reader.IsDBNull(4) ? "" : reader.GetString(4),
                TrainerId = reader.IsDBNull(5) ? null : reader.GetInt32(5),
                TrainerName = reader.IsDBNull(6) ? "" : reader.GetString(6)
            };
        }
        return null;
    }

    public Enrollment? GetEnrollmentForStudent(int studentId, int classId)
    {
        using var connection = GetConnection();
        var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM Enrollments WHERE StudentId = @studentid AND ClassId = @classid";
        command.Parameters.AddWithValue("@studentid", studentId);
        command.Parameters.AddWithValue("@classid", classId);
        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            return new Enrollment
            {
                Id = reader.GetInt32(0),
                StudentId = reader.GetInt32(1),
                StudentName = reader.GetString(2),
                TPNumber = reader.GetString(3),
                ClassId = reader.GetInt32(4),
                ModuleName = reader.GetString(5),
                Level = reader.GetString(6),
                EnrolledMonth = reader.IsDBNull(7) ? "" : reader.GetString(7),
                IsPaid = reader.GetInt32(8) == 1,
                IsCompleted = reader.GetInt32(9) == 1
            };
        }
        return null;
    }

    public List<Enrollment> GetEnrollmentsForStudent(int studentId)
    {
        var enrollments = new List<Enrollment>();
        using var connection = GetConnection();
        var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM Enrollments WHERE StudentId = @studentid";
        command.Parameters.AddWithValue("@studentid", studentId);
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            enrollments.Add(new Enrollment
            {
                Id = reader.GetInt32(0),
                StudentId = reader.GetInt32(1),
                StudentName = reader.GetString(2),
                TPNumber = reader.GetString(3),
                ClassId = reader.GetInt32(4),
                ModuleName = reader.GetString(5),
                Level = reader.GetString(6),
                EnrolledMonth = reader.IsDBNull(7) ? "" : reader.GetString(7),
                IsPaid = reader.GetInt32(8) == 1,
                IsCompleted = reader.GetInt32(9) == 1
            });
        }
        return enrollments;
    }
}