
namespace Shared.Contracts.DTOs.Auth;

public class RecoverRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
