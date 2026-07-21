using System.Security.Cryptography;
using System.Text;

namespace Shared.Contracts.Services;

public static class EncryptionService
{
    public static string HashSHA256(string text)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(
            Encoding.UTF8.GetBytes(text));
        var sb = new StringBuilder();
        foreach (var b in bytes)
            sb.Append(b.ToString("x2"));
        return sb.ToString();
    }
}