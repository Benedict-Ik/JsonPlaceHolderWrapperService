namespace JsonPlaceHolderWrapperService.Models
{
    public class AddressDto
    {
        public string Street { get; set; } = "";
        public string City { get; set; } = "";
        public string Zipcode { get; set; } = "";
    }

    public class CompanyDto
    {
        public string Name { get; set; } = "";
        public string CatchPhrase { get; set; } = "";
        public string Bs { get; set; } = "";
    }

    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Username { get; set; } = "";
        public string Email { get; set; } = "";
        public AddressDto Address { get; set; } = new AddressDto();
        public string Phone { get; set; } = "";
        public string Website { get; set; } = "";
        public CompanyDto Company { get; set; } = new CompanyDto();
    }
}
