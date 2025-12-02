using System;
using System.IO;

namespace prolab_2
{
    public static class Logger
    {
        private static string filePath = "savunma_gunlugu.txt";

        // YENİ EKLENEN KISIM: Haberci Olayı
        // Bu olay, "Log" metodu her çağrıldığında tetiklenecek.
        public static event Action<string> OnLogReceived;

        public static void ClearLog()
        {
            File.WriteAllText(filePath, string.Empty);
        }

        public static void Log(string message)
        {
            // 1. Dosyaya yazma (Eski işlevi koruyoruz)
            using (StreamWriter sw = File.AppendText(filePath))
            {
                // Tarih/Saat eklemek okumayı kolaylaştırır
                string logLine = $"[{DateTime.Now:HH:mm:ss}] {message}";
                sw.WriteLine(logLine);

                // 2. Arayüze haber ver (Event'i tetikle)
                OnLogReceived?.Invoke(logLine);
            }
        }
    }
}