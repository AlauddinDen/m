namespace WebApplication1.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime AdmissionDate { get; set; }
        public bool IsActive { get; set; }
        public string? ImageUrl { get; set; }
        public List<Address> Addresses { get; set; }=new List<Address>();

    }

    public class Address
    {
        public int Id { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public int StudentId { get; set; }

    }
}
