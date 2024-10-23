
using MaterialDesignThemes.Wpf;
namespace Home_budget_graphic.Domain
{

    public class NavItem
    {
        public string? Title { get; set; }
        public PackIconKind SelectedIcon { get; set; }
        public PackIconKind UnselectedIcon { get; set; }
    }
}