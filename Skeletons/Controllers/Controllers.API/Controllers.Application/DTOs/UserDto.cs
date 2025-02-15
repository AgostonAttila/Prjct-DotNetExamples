namespace Controllers.Application.DTOs
{
    public class UserDto
    {
        public string Id { get; set; }
        public required string UserName { get; set; }
        public required string Email { get; set; }
    }
}
