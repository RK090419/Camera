namespace Core.Interfaces;
public interface IViewProvider
{
    public IViewModelBase GetViewModel(Type viewType);

    public IViewBase GetView<T>(object? navigationParam = null) where T : IViewBase;
    public IViewBase? GetView(Type viewType, object? navigationParam = null);
}