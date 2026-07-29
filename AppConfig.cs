using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmsApp
{
    public static class AppConfig
    {
#if DEBUG
        // =======================================================
        // ENVIRONMENT: DEVELOPMENT (Mode Pengujian di Laptop/Emulator)
        // =======================================================

        // Gunakan ini jika menjalankan aplikasi Windows Machine
        public static readonly string BaseUrl = "https://localhost:7278/";

        // (Atau) Gunakan ini jika menjalankan Android Emulator
        // public static readonly string BaseUrl = "https://10.0.2.2:7278/";
#else
        // =======================================================
        // ENVIRONMENT: PRODUCTION (Saat dirilis jadi APK/Live)
        // =======================================================
        
        // IP Server / Cloud API Anda yang sebenarnya
        public static readonly string BaseUrl = "https://api.gudang-perusahaan.com/";
#endif
    }
}