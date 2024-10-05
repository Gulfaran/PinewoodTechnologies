namespace Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Forename { get; set; }
        public string Surname { get; set; }
        public string? Organisationname { get; set; }
        public string Contactnumber { get; set; }
        public string Email { get; set; }
        public Address address { get; set; }
    }
}
