namespace task_manager.DTO
{
    public class AuthResponseDto
    {
        public string Token { get; set; }

        public UserDto User { get; set; }

        public class UserDto
        {
            public int Id { get; set; }
            public string Email { get; set; }
        }
    }
}
