namespace DotnetAPI.Dtos
{
    public partial class UserForLoginConfirmationDTO
    {
        public byte[] PasswordHash { get; set; } = Array.Empty<byte>();
        public byte[] PasswordSalt { get; set; } = Array.Empty<byte>();
    }
}
