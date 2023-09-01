namespace BackendLib.Caching;

/// <summary>
/// Pre-configured cache expiration policies
/// </summary>
public static class ExpirationPolicies
{
    public static readonly IExpirationPolicy NoExpiration = new ExpirationPolicy(null);

    public static readonly IExpirationPolicy OneMinuteExpiration = new ExpirationPolicy(TimeSpan.FromMinutes(1));

    public static readonly IExpirationPolicy OneHourExpiration = new ExpirationPolicy(TimeSpan.FromHours(1));

    public static readonly IExpirationPolicy OneDayExpiration = new ExpirationPolicy(TimeSpan.FromDays(1));

    public static readonly IExpirationPolicy OneWeekExpiration = new ExpirationPolicy(TimeSpan.FromDays(7));

    public static readonly IExpirationPolicy TwoWeekExpiration = new ExpirationPolicy(TimeSpan.FromDays(14));

    public static readonly IExpirationPolicy OneMonthExpiration = new ExpirationPolicy(TimeSpan.FromDays(30));
}