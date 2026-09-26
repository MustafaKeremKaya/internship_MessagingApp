using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MessagingApp.Client.Services;
using MessagingApp.Core.Constants;
using MessagingApp.Entities.DTOs;

namespace MessagingApp.Client.UI
{

    public class ConversationMenu
    {
        private readonly ApiClient _apiClient;
        private readonly ChatView _chatView;
        private readonly object _consoleLock = new object();

        public ConversationMenu(ApiClient apiClient)
        {
            _apiClient = apiClient;
            _chatView = new ChatView(apiClient);
        }

        public async Task ShowAsync(string currentUser)
        {
            bool isRunning = true;

            while (isRunning)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n==================================================");
                Console.WriteLine($"ANA MENU (Kullanici: {currentUser})");
                Console.WriteLine("==================================================");
                Console.ResetColor();

                Console.WriteLine(" [1] Sohbet Odalarini Listele & Odaya Gir");
                Console.WriteLine(" [2] Yeni Sohbet Odasi Ac");
                Console.WriteLine(" [3] Cikis (:cikis)");
                Console.Write("\nSeciminiz (1-3 veya :cikis): ");

                var choice = Console.ReadLine()?.Trim();

                if (IsExitCommand(choice) || choice == "3")
                {
                    isRunning = false;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\nUygulamadan cikiliyor. Iyi gunler dileriz!");
                    Console.ResetColor();
                    break;
                }

                switch (choice)
                {
                    case "1":
                        await ListAndJoinConversationAsync(currentUser);
                        break;
                    case "2":
                        await CreateNewConversationAsync(currentUser);
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("[UYARI] Gecersiz secim! Lutfen 1, 2 veya 3 yazip Enter'a basiniz.");
                        Console.ResetColor();
                        break;
                }
            }
        }

        private async Task ListAndJoinConversationAsync(string currentUser)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("\n[BILGI] Sohbet odalari sunucudan yukleniyor...");
            Console.ResetColor();

            var response = await _apiClient.GetConversationsAsync();

            if (!response.Success || response.Data == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[HATA] Sohbetler yuklenemedi: " + response.Message);
                Console.ResetColor();
                return;
            }

            var knownConversations = new List<ConversationSummaryResponse>(response.Data);
            var knownIds = new HashSet<int>(knownConversations.Select(c => c.Id));

            PrintConversationList(knownConversations);

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("[BILGI] Bu ekran canlidir. Baska biri yeni oda actiginda otomatik bildirim duser.");
            Console.ResetColor();
            Console.Write("\nKatilmak istediginiz Oda ID'sini girin (Geri donmek icin :cikis): ");

            using var cts = new CancellationTokenSource();

            var pollingTask = Task.Run(async () =>
            {
                while (!cts.Token.IsCancellationRequested)
                {
                    try
                    {
                        await Task.Delay(2000, cts.Token);
                        var latestResponse = await _apiClient.GetConversationsAsync();
                        if (latestResponse.Success && latestResponse.Data != null)
                        {
                            var newRooms = latestResponse.Data
                                .Where(c => !knownIds.Contains(c.Id))
                                .OrderBy(c => c.Id)
                                .ToList();

                            if (newRooms.Count > 0)
                            {
                                lock (_consoleLock)
                                {
                                    foreach (var newRoom in newRooms)
                                    {
                                        knownIds.Add(newRoom.Id);
                                        knownConversations.Add(newRoom);

                                        Console.ForegroundColor = ConsoleColor.Yellow;
                                        Console.WriteLine($"\n[BILDIRIM] YENI ODA ACILDI: [{newRoom.Id}] {newRoom.Title} (Olusturulma: {newRoom.CreatedAt.ToLocalTime():T})");
                                        Console.ResetColor();
                                    }
                                    Console.Write("Katilmak istediginiz Oda ID'si (Geri icin :cikis): ");
                                }
                            }
                        }
                    }
                    catch (TaskCanceledException)
                    {
                        break;
                    }
                    catch
                    {

                    }
                }
            }, cts.Token);

            while (true)
            {
                var input = Console.ReadLine()?.Trim();
                cts.Cancel(); 

                if (IsExitCommand(input))
                {
                    return;
                }

                if (!int.TryParse(input, out int selectedId))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[UYARI] Gecersiz ID! Lutfen oda numarasini yaziniz.");
                    Console.ResetColor();
                    Console.Write("Tekrar deneyin (Geri icin :cikis): ");
                    continue;
                }

                var selectedConv = knownConversations.FirstOrDefault(c => c.Id == selectedId);
                if (selectedConv == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[UYARI] #{selectedId} numarali bir sohbet odasi bulunamadi.");
                    Console.ResetColor();
                    Console.Write("Tekrar deneyin (Geri icin :cikis): ");
                    continue;
                }

                await _chatView.StartChatAsync(selectedConv.Id, selectedConv.Title, currentUser);
                break;
            }
        }

        private void PrintConversationList(List<ConversationSummaryResponse> conversations)
        {
            if (conversations.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("\n[BILGI] Henuz acilmis hicbir sohbet odasi yok. 2'ye basarak ilk odayi siz acabilirsiniz!");
                Console.ResetColor();
                return;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n--- MEVCUT SOHBET ODALARI ---");
            Console.ResetColor();

            foreach (var conv in conversations.OrderBy(c => c.Id))
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($" [{conv.Id}] ");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write($"{conv.Title}");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($" (Olusturulma: {conv.CreatedAt.ToLocalTime():g})");
                Console.ResetColor();
            }
        }

        private async Task CreateNewConversationAsync(string currentUser)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n--- YENI SOHBET ODASI OLUSTUR ---");
            Console.ResetColor();

            Console.Write("Oda Basligini Girin (Iptal etmek icin :cikis): ");
            var title = Console.ReadLine()?.Trim();

            if (IsExitCommand(title) || string.IsNullOrWhiteSpace(title))
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Oda olusturma islemi iptal edildi. Ana menuye donuluyor...");
                Console.ResetColor();
                await Task.Delay(600);
                return;
            }

            if (title.Length > 100)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[UYARI] " + Messages.ConversationTitleTooLong);
                Console.ResetColor();
                return;
            }

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("[BILGI] Oda olusturuluyor...");
            Console.ResetColor();

            var result = await _apiClient.CreateConversationAsync(title);

            if (result.Success && result.Data != null)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"[BILGI] '{result.Data.Title}' odasi basariyla acildi! (ID: {result.Data.Id})");
                Console.WriteLine("Simdi odaya giris yapiliyor...");
                Console.ResetColor();
                await Task.Delay(1000);

                await _chatView.StartChatAsync(result.Data.Id, result.Data.Title, currentUser);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[HATA] Oda olusturulamadi: " + result.Message);
                Console.ResetColor();
            }
        }

        private static bool IsExitCommand(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return false;
            }

            var trimmed = input.Trim();
            return trimmed.Equals(":çıkış", StringComparison.OrdinalIgnoreCase) ||
                   trimmed.Equals(":cikis", StringComparison.OrdinalIgnoreCase) ||
                   trimmed.Equals(":ÇIKIŞ", StringComparison.OrdinalIgnoreCase) ||
                   trimmed.Equals(":CIKIS", StringComparison.OrdinalIgnoreCase) ||
                   trimmed == "0" ||
                   trimmed.Equals(":q", StringComparison.OrdinalIgnoreCase) ||
                   trimmed.Equals("q", StringComparison.OrdinalIgnoreCase);
        }
    }
}
