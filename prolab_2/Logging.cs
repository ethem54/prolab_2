using System;
using System.IO;

namespace prolab_2
{
    public static class Logger
    {
        private static string filePath = "savunma_gunlugu.txt";

        public static void ClearLog()
        {
            // Oyun her başladığında dosyayı temizle
            File.WriteAllText(filePath, string.Empty);
        }

        public static void Log(string message)
        {
            using (StreamWriter sw = File.AppendText(filePath))
            {
                sw.WriteLine(message);
            }
        }
    }
}