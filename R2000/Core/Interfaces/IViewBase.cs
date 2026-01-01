namespace Core.Interfaces;
public interface IViewBase
{
    public object? GetViewModel();

    public Type ViewModelType { get; }

    public void SetViewModel(IViewModelBase viewModel);

}