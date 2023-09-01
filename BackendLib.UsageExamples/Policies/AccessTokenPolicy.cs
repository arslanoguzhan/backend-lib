using BackendLib.Tokens;

namespace BackendLib.UsageExamples.Policies;

public class AccessTokenPolicy : ITokenPolicy
{
    public TimeSpan LifeSpan => TimeSpan.FromMinutes(30);
}