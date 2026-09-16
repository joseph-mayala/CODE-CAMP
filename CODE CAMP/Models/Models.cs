namespace WinFormsApp.Models;

public enum UserRole
{
    Admin = 0,
    Trainer = 1,
    Lecturer = 2,
    Student = 3
}

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = "";
    public string PasswordHash { get; set; } = "";
    public string FullName { get; set; } = "";
    public string Email { get; set; } = "";
    public string Contact { get; set; } = "";
    public UserRole Role { get; set; }
    public string? TPNumber { get; set; }
    public string? Address { get; set; }
    public string? StudyLevel { get; set; }
}

public class Class
{
    public int Id { get; set; }
    public string ModuleName { get; set; } = "";
    public string Level { get; set; } = "";
    public decimal Charges { get; set; }
    public string Schedule { get; set; } = "";
    public int? TrainerId { get; set; }
    public string? TrainerName { get; set; }
}

public class Enrollment
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public string? StudentName { get; set; }
    public string? TPNumber { get; set; }
    public int ClassId { get; set; }
    public string? ModuleName { get; set; }
    public string? Level { get; set; }
    public string? EnrolledMonth { get; set; }
    public bool IsPaid { get; set; }
    public bool IsCompleted { get; set; }
}

public class Payment
{
    public int Id { get; set; }
    public int EnrollmentId { get; set; }
    public int StudentId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public string Status { get; set; } = "";
}

public class Feedback
{
    public int Id { get; set; }
    public int TrainerId { get; set; }
    public string? TrainerName { get; set; }
    public string Message { get; set; } = "";
    public DateTime CreatedAt { get; set; }
}

public class StudentRequest
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public string? StudentName { get; set; }
    public string ModuleName { get; set; } = "";
    public string Level { get; set; } = "";
    public string Status { get; set; } = "";
    public DateTime CreatedAt { get; set; }
}