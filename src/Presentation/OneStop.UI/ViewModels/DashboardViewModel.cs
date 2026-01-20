namespace OneStop.UI.ViewModels;

public class DashboardViewModel : ViewModelBase
{
    // Gunakan 'override' karena ViewModelBase sudah memiliki properti virtual Title
    public override string Title => "Executive Dashboard";

    // Di sini nanti kita akan tambahkan logic untuk:
    // - Total Penjualan Hari Ini
    // - Stok Menipis
    // - Grafik Produksi
}