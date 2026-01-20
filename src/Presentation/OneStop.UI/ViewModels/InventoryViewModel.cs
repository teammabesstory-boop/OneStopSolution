using OneStop.Application.Common.Interfaces;
using OneStop.Domain.Modules.Production;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System; // <-- Tambah ini

namespace OneStop.UI.ViewModels;

public class InventoryViewModel : ViewModelBase
{
    private readonly IInventoryService _inventoryService;
    private ObservableCollection<Ingredient> _ingredients;

    public InventoryViewModel(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
        _ingredients = new ObservableCollection<Ingredient>();

        LoadDataCommand = ReactiveCommand.CreateFromTask(LoadDataAsync);

        // 1. DAFTARKAN COMMAND BARU
        SeedDataCommand = ReactiveCommand.CreateFromTask(SeedDataAsync);

        // Load awal
        LoadDataCommand.Execute().Subscribe();
    }

    public override string Title => "Warehouse Management";

    public ObservableCollection<Ingredient> Ingredients
    {
        get => _ingredients;
        set => this.RaiseAndSetIfChanged(ref _ingredients, value);
    }

    public ReactiveCommand<System.Reactive.Unit, System.Reactive.Unit> LoadDataCommand { get; }

    // 2. PROPERTY COMMAND BARU
    public ReactiveCommand<System.Reactive.Unit, System.Reactive.Unit> SeedDataCommand { get; }

    private async Task LoadDataAsync()
    {
        var data = await _inventoryService.GetAllIngredientsAsync();
        Ingredients = new ObservableCollection<Ingredient>(data);
    }

    // 3. LOGIC GENERATE DATA
    private async Task SeedDataAsync()
    {
        // Cek jika data sudah ada, jangan spam
        if (Ingredients.Count > 0) return;

        // Kita buat data dummy pakai Factory Method dari Domain
        // Ingat: Create mengembalikan Result<T>, jadi ambil .Value
        var tepung = Ingredient.Create("Tepung Terigu Protein Sedang", "RM-001", Unit.Kilogram, 13000).Value;
        tepung.AdjustStock(50); // Punya 50 KG

        var gula = Ingredient.Create("Gula Pasir Premium", "RM-002", Unit.Kilogram, 16000).Value;
        gula.AdjustStock(25); // Punya 25 KG

        var telur = Ingredient.Create("Telur Ayam Negeri", "RM-003", Unit.Pcs, 2000).Value;
        telur.AdjustStock(100); // Punya 100 Butir

        var susu = Ingredient.Create("Susu UHT Full Cream", "RM-004", Unit.Liter, 18500).Value;
        susu.AdjustStock(12); // Punya 12 Liter

        var ragi = Ingredient.Create("Ragi Instan (Fermipan)", "RM-005", Unit.Gram, 500).Value;
        ragi.AdjustStock(500); // Punya 500 Gram

        // Simpan satu per satu ke Database
        await _inventoryService.AddIngredientAsync(tepung);
        await _inventoryService.AddIngredientAsync(gula);
        await _inventoryService.AddIngredientAsync(telur);
        await _inventoryService.AddIngredientAsync(susu);
        await _inventoryService.AddIngredientAsync(ragi);

        // Refresh Tabel
        await LoadDataAsync();
    }
}