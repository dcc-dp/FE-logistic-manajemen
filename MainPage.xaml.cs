using System.Net.Http.Json;

namespace WmsApp
{
    public partial class MainPage : ContentPage
    {
        public static HttpClient ApiClient;

        public MainPage()
        {
            InitializeComponent();
            SetupHttpClient();
        }

        private void SetupHttpClient()
        {
            if (ApiClient == null)
            {
                var handler = new HttpClientHandler { UseCookies = true };
                ApiClient = new HttpClient(handler)
                {
                    BaseAddress = new Uri(AppConfig.BaseUrl) // Ganti dengan port API Anda
                };
            }
        }

        // --- EFEK HOVER/FOCUS ALA REACT ---
        private void OnEntryFocused(object sender, FocusEventArgs e)
        {
            var entry = sender as Entry;
            if (entry == EmailEntry) EmailBorder.Stroke = Color.FromArgb("#2563eb");
            if (entry == PasswordEntry) PasswordBorder.Stroke = Color.FromArgb("#2563eb");
        }

        private void OnEntryUnfocused(object sender, FocusEventArgs e)
        {
            var entry = sender as Entry;
            if (entry == EmailEntry) EmailBorder.Stroke = Color.FromArgb("#2e3350");
            if (entry == PasswordEntry) PasswordBorder.Stroke = Color.FromArgb("#2e3350");
        }

        // --- TOGGLE PASSWORD ---
        private void OnTogglePasswordClicked(object sender, EventArgs e)
        {
            PasswordEntry.IsPassword = !PasswordEntry.IsPassword;
            // Catatan: Jika Anda punya gambar mata terbuka/tertutup, bisa ubah Source di sini:
             EyeIcon.Source = PasswordEntry.IsPassword ? "eye_closed.png" : "eye_open.png";
        }

        // --- LOGIC LOGIN ---
        private async void OnLoginClicked(object sender, EventArgs e)
        {
            string email = EmailEntry.Text;
            string password = PasswordEntry.Text;

            // Validasi Kosong (Menampilkan Alert Box Merah ala React)
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ErrorLabel.Text = "Email dan password wajib diisi.";
                ErrorBox.IsVisible = true;
                return;
            }
            
            ErrorBox.IsVisible = false;

            // UI State: Loading
            LoginBtn.IsEnabled = false;
            LoginBtnText.Text = "Memverifikasi...";
            LoadingSpinner.IsRunning = true;
            LoadingSpinner.IsVisible = true;
            LoginBtn.Shadow.Opacity = 0; // Hilangkan bayangan saat diklik/loading

            try
            {
                var dataLogin = new { email = email, password = password };
                var response = await ApiClient.PostAsJsonAsync("api/Auth/Login", dataLogin);

                // Simulasi delay tambahan agar efek loading terlihat mulus seperti React
                await Task.Delay(900); 

                if (response.IsSuccessStatusCode)
                {
                    // Nanti kita arahkan ke Halaman Dashboard
                    // Application.Current.MainPage = new NavigationPage(new DashboardPage());
                    await DisplayAlert("Sukses", "Login berhasil!", "OK");
                }
                else
                {
                    ErrorLabel.Text = "Email atau password yang Anda masukkan salah.";
                    ErrorBox.IsVisible = true;
                }
            }
            catch (Exception)
            {
                ErrorLabel.Text = "Gagal terhubung ke server. Periksa jaringan Anda.";
                ErrorBox.IsVisible = true;
            }
            finally
            {
                // UI State: Normal
                LoginBtn.IsEnabled = true;
                LoginBtnText.Text = "LOGIN";
                LoadingSpinner.IsRunning = false;
                LoadingSpinner.IsVisible = false;
                LoginBtn.Shadow.Opacity = 0.4f; // Kembalikan bayangan
            }
        }

        private async void OnLupaPasswordClicked(object sender, TappedEventArgs e)
        {
            await DisplayAlert("Lupa Password", "Silakan hubungi IT Support atau Supervisor Anda untuk mereset password.", "Tutup");
        }
    }
}