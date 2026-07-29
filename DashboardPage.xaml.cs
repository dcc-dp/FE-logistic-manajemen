namespace WmsApp;

public partial class DashboardPage : ContentPage
{
    public DashboardPage()
    {
        InitializeComponent();
    }

    private async void OnMenuClicked(object sender, EventArgs e)
    {
        var tombol = (Button)sender;
        await DisplayAlert("Info", $"Anda memilih menu: {tombol.Text}\n(Halaman belum dibuat)", "OK");
    }

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        bool konfirmasi = await DisplayAlert("Logout", "Apakah Anda yakin ingin keluar?", "Ya", "Batal");
        if (konfirmasi)
        {
            var tombol = (Button)sender;
            tombol.Text = "MEMPROSES...";
            tombol.IsEnabled = false;

            try
            {
                // 1. Tembak API Logout agar Backend menghancurkan Cookie di sisi server
                if (MainPage.ApiClient != null)
                {
                    await MainPage.ApiClient.PostAsync("api/Auth/Logout", null);
                }
            }
            catch (Exception)
            {
                // Jika gagal terhubung ke server (misal mati lampu/hilang sinyal), 
                // abaikan saja error-nya agar user tetap bisa keluar dari aplikasi.
            }
            finally
            {
                // 2. Hancurkan HttpClient lokal dan kembali ke layar Login
                MainPage.ApiClient = null;
                Application.Current.MainPage = new NavigationPage(new MainPage());
            }
        }
    }
}