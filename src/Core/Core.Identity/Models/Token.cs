namespace Core.Identity.Models;

public struct Token
{
    public string Value { get; set; }

    public DateTime Expiration { get; set; }
}