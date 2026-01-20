using ReactiveUI;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace OneStop.UI.ViewModels;

// Class pembantu untuk Menu Item di Sidebar
public class MenuItem
{
    public string Label { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty; // Nanti bisa pakai FontIcon
    public ViewModelBase ViewModel { get; set; } // Halaman tujuan
}

public class MainWindowViewModel : ViewModelBase
{
    private ViewModelBase _currentPage;
    private MenuItem _selectedMenuItem;

    public MainWindowViewModel()
    {
        // 1. Definisi Halaman & Menu
        // Di aplikasi real, ini bisa di-inject via DI Factory
        MenuItems = new ObservableCollection<MenuItem>
        {
            new MenuItem { Label = "Dashboard", ViewModel = new DashboardViewModel() },
            new MenuItem { Label = "Inventory", ViewModel = new InventoryViewModel() },
            new MenuItem { Label = "Production", ViewModel = new ProductionViewModel() },
        };

        // 2. Set Halaman Awal
        SelectedMenuItem = MenuItems[0];
    }

    // List Menu untuk Sidebar
    public ObservableCollection<MenuItem> MenuItems { get; }

    // Halaman yang sedang aktif (ditampilkan di tengah)
    public ViewModelBase CurrentPage
    {
        get => _currentPage;
        private set => this.RaiseAndSetIfChanged(ref _currentPage, value);
    }

    // Handling saat user klik menu di Sidebar
    public MenuItem SelectedMenuItem
    {
        get => _selectedMenuItem;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedMenuItem, value);
            if (value != null)
            {
                CurrentPage = value.ViewModel;
            }
        }
    }
}