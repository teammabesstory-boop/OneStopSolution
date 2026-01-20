using OneStop.Application.Common.Interfaces;
using ReactiveUI;
using System.Collections.ObjectModel;

namespace OneStop.UI.ViewModels;

public class MenuItem
{
    public string Label { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    // FIX: Tambahkan tanda tanya (?) agar boleh null sementara
    public ViewModelBase? ViewModel { get; set; }
}

public class MainWindowViewModel : ViewModelBase
{
    // FIX: Tambahkan '= null!' (Janji ke compiler ini pasti diisi di constructor)
    private ViewModelBase _currentPage = null!;
    private MenuItem _selectedMenuItem = null!;

    public MainWindowViewModel(IInventoryService inventoryService)
    {
        MenuItems = new ObservableCollection<MenuItem>
        {
            new MenuItem
            {
                Label = "Dashboard",
                ViewModel = new DashboardViewModel()
            },
            new MenuItem
            {
                Label = "Inventory",
                ViewModel = new InventoryViewModel(inventoryService)
            },
            new MenuItem
            {
                Label = "Production",
                ViewModel = new ProductionViewModel()
            },
        };

        SelectedMenuItem = MenuItems[0];
    }

    public ObservableCollection<MenuItem> MenuItems { get; }

    public ViewModelBase CurrentPage
    {
        get => _currentPage;
        private set => this.RaiseAndSetIfChanged(ref _currentPage, value);
    }

    public MenuItem SelectedMenuItem
    {
        get => _selectedMenuItem;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedMenuItem, value);

            // Cek null sebelum assign
            if (value?.ViewModel != null)
            {
                CurrentPage = value.ViewModel;
            }
        }
    }
}