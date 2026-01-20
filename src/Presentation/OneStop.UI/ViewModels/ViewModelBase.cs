using ReactiveUI;

namespace OneStop.UI.ViewModels;

public class ViewModelBase : ReactiveObject
{
    // Tambahkan ini sebagai nilai default virtual
    public virtual string Title => string.Empty;
}