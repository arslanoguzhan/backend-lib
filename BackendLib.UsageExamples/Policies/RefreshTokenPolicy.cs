using BackendLib.Tokens;

namespace BackendLib.UsageExamples.Policies;

public class RefreshTokenPolicy : ITokenPolicy
{
    public TimeSpan LifeSpan => TimeSpan.FromDays(3650);
}