namespace Shared.Contracts.DTOs.Auth;

public class UpdateProfileRequest
{
    public string Name { get; set; } = string.Empty;
    public string Apaterno { get; set; } = string.Empty;
    public string Amaterno { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
}