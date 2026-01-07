namespace FastFood.KitchenFlow.Infra.Auth
{
    public sealed record JwtOptions(string Issuer, string Audience, string SecretKey, int ExpiresInMinutes);
}
