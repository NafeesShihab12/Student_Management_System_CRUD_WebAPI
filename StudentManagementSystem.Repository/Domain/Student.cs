namespace StudentManagementSystem.Repository.Domain
{
    public class Student
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateOnly DOB { get; set; }
        public string? Gender { get; set; }
        public double CGPA { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }

    }
}
