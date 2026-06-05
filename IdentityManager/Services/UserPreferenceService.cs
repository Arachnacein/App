namespace IdentityManager.Services;

public class UserPreferenceService : IUserPreferenceService
{
    private readonly ApplicationDbContext _dbContext;

    public UserPreferenceService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UserPreferenceModel?> GetAsync(string userId, CancellationToken ct = default)
    {
        var preference = await _dbContext.UserPreferences
                                   .AsNoTracking()
                                   .FirstOrDefaultAsync(x => x.UserId == userId, ct);
        if (preference is null)
            return null;

        return new UserPreferenceModel
        {
            UserId = preference.UserId,
            IsDarkMode = preference.IsDarkMode,
            RecurringTransactionTheme = preference.RecurringTransactionTheme
        };
    }

    public async Task UpsertAsync(UserPreferenceModel model, CancellationToken ct = default)
    {
        var existing = await _dbContext.UserPreferences
                                       .FirstOrDefaultAsync(x => x.UserId == model.UserId, ct);
        if (existing is null)
        {
            await _dbContext.UserPreferences.AddAsync(new UserPreference
            {
                UserId = model.UserId,
                IsDarkMode = model.IsDarkMode,
                RecurringTransactionTheme = model.RecurringTransactionTheme
            }, ct);
        }
        else
        {
            existing.IsDarkMode = model.IsDarkMode;
            existing.RecurringTransactionTheme = model.RecurringTransactionTheme;
            _dbContext.UserPreferences.Update(existing);
        }
        await _dbContext.SaveChangesAsync(ct);
    }
}
public interface IUserPreferenceService
{
    Task<UserPreferenceModel?> GetAsync(string userId, CancellationToken ct = default);
    Task UpsertAsync(UserPreferenceModel model, CancellationToken ct = default);
}