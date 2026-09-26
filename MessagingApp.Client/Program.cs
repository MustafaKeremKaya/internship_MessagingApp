using System;
using System.Text;
using System.Threading.Tasks;
using MessagingApp.Client.Services;
using MessagingApp.Client.UI;
using MessagingApp.Core.Constants;

namespace MessagingApp.Client
{

    internal class Program
    {
        private static async Task Main(string[] args)
        {

            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(@"
========================================================================
BASIT MESAJLASMA UYGULAMASI (TERMINAL CLIENT)
========================================================================");
            Console.ResetColor();

            string currentUser = string.Empty;
            while (string.IsNullOrWhiteSpace(currentUser))
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("\nKullanici Adinizi Girin (Maks 50 karakter): ");
                Console.ResetColor();

                var input = Console.ReadLine()?.Trim();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[UYARI] " + Messages.SenderEmpty);
                    Console.ResetColor();
                    continue;
                }

                if (input.Length > 50)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[UYARI] " + Messages.SenderTooLong);
                    Console.ResetColor();
                    continue;
                }

                currentUser = input;
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("\nSunucu Adresi (Varsayilan icin Enter'a basin [http://localhost:5000]): ");
            Console.ResetColor();

            var serverInput = Console.ReadLine()?.Trim();
            string serverUrl = "http://localhost:5000";

            if (!string.IsNullOrWhiteSpace(serverInput))
            {
                serverUrl = serverInput;
                if (!serverUrl.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
                    !serverUrl.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                {
                    serverUrl = "http://" + serverUrl;
                }
            }

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"\n[BILGI] Sunucuya baglaniliyor: {serverUrl}...");
            Console.ResetColor();

            var apiClient = new ApiClient(serverUrl);

            var connectionTest = await apiClient.GetConversationsAsync();
            if (!connectionTest.Success)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n" + connectionTest.Message);
                Console.WriteLine("Ipucu: Eger ayni bilgisyardaysaniz Web API'nin acik oldugundan emin olun.");
                Console.Write("\nYine de devam etmek istiyor musunuz? (E/H): ");
                Console.ResetColor();

                var retryChoice = Console.ReadLine()?.Trim();
                if (!string.Equals(retryChoice, "E", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Uygulama kapatiliyor.");
                    return;
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("[BILGI] Sunucuya basariyla baglanildi!");
                Console.ResetColor();
            }

            var menu = new ConversationMenu(apiClient);
            await menu.ShowAsync(currentUser);
        }
    }
}
