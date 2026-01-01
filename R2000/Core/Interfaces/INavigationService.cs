namespace Core.Interfaces;

public partial interface INavigationService
{
    event EventHandler CanGoBackChanged;

    void ClearNavigationHistory();

    bool Navigate(Type dst);

    bool Navigate<TFrom, TTo>(object? navigationParam = null)
        where TFrom : IViewBase
        where TTo : IViewBase;

    bool Navigate<TTo>(object? navigationKey = null) where TTo : IViewBase;

    void Clear();
}