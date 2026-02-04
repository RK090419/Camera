namespace Core.Interfaces;

public partial interface INavigationService
{
    bool Navigate<TFrom, TTo>(object? navigationParam = null)
        where TFrom : IViewBase
        where TTo : IViewBase;

    bool Navigate<TTo>(object? navigationKey = null) where TTo : IViewBase;

}