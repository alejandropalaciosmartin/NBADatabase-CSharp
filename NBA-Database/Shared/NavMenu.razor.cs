namespace NBA_Database.Shared;

public partial class NavMenu
{
    private bool collapseNavMenu = true;
    public bool CollapseNavMenu { get => collapseNavMenu; set => collapseNavMenu = value; }
    public string? NavMenuCssClass => collapseNavMenu ? "collapse" : null;
    private void ToggleNavMenu()
    {
        collapseNavMenu = !collapseNavMenu;
    }
}