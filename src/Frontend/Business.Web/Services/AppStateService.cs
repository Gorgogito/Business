namespace Business.Web.Services;

using Business.Web.Models;

public class AppStateService
{
    public UserSession? CurrentUser { get; private set; }
    public event Action? OnChange;

    public void SetUser(UserSession user)
    {
        CurrentUser = user;
        NotifyStateChanged();
    }

    public void ClearUser()
    {
        CurrentUser = null;
        NotifyStateChanged();
    }

    public bool IsAuthenticated => CurrentUser?.IsAuthenticated == true;

    /// <summary>Indica si el usuario actual tiene el permiso indicado (p. ej. "sales.manage").</summary>
    public bool HasPermission(string code) => CurrentUser?.Permissions.Contains(code) == true;

    public bool IsDarkMode { get; private set; }

    public void SetDarkMode(bool isDark)
    {
        if (IsDarkMode == isDark) return;
        IsDarkMode = isDark;
        NotifyStateChanged();
    }

    public void ToggleDarkMode()
    {
        IsDarkMode = !IsDarkMode;
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}
