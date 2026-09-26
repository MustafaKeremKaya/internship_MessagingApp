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

    public class ChatView
    {
        private readonly ApiClient _apiClient;
        private readonly object _consoleLock = new object();

        public ChatView(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task StartChatAsync(int conversationId, string conversationTitle, string currentUser)
        {
            Console.Clear();
            PrintRoomHeader(conversationId, conversationTitle, currentUser);

            int lastMessageId = 0;
            var initialResult = await _apiClient.GetMessagesAsync(conversationId);

            if (initialResult.Success && initialResult.Data != null)
            {
                foreach (var msg in initialResult.Data.OrderBy(m => m.SentAt))
                {
                    PrintMessage(msg, currentUser);
                    if (msg.Id > lastMessageId)
                    {
                        lastMessageId = msg.Id;
                    }
                }
            }
            else
            {
                PrintSystemMessage("[UYARI] Gecmis mesajlar yuklenemedi: " + initialResult.Message, ConsoleColor.DarkYellow);
            }

            using var cts = new CancellationTokenSource();

            var pollingTask = Task.Run(async () =>
            {
                while (!cts.Token.IsCancellationRequested)
                {
                    try
                    {
                        await Task.Delay(2000, cts.Token);

                        var response = await _apiClient.GetMessagesAsync(conversationId);
                        if (response.Success && response.Data != null)
                        {
                            var newMessages = response.Data
                                .Where(m => m.Id > lastMessageId)
                                .OrderBy(m => m.SentAt)
                                .ToList();

                            foreach (var newMsg in newMessages)
                            {
                                PrintMessage(newMsg, currentUser);
                                if (newMsg.Id > lastMessageId)
                                {
                                    lastMessageId = newMsg.Id;
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
                string? input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    continue;
                }

                var trimmed = input.Trim();

                if (trimmed.Equals(":çıkış", StringComparison.OrdinalIgnoreCase) || 
                    trimmed.Equals(":cikis", StringComparison.OrdinalIgnoreCase) ||
                    trimmed.Equals(":q", StringComparison.OrdinalIgnoreCase) || 
                    trimmed.Equals("q", StringComparison.OrdinalIgnoreCase))
                {
                    cts.Cancel(); 
                    break;
                }

                if (trimmed.Length > 500)
                {
                    PrintSystemMessage(Messages.ContentTooLong, ConsoleColor.Red);
                    continue;
                }

                var sendResult = await _apiClient.SendMessageAsync(conversationId, currentUser, trimmed);
                if (sendResult.Success && sendResult.Data != null)
                {

                    if (sendResult.Data.Id > lastMessageId)
                    {
                        lastMessageId = sendResult.Data.Id;
                    }

                    lock (_consoleLock)
                    {
                        try
                        {
                            if (!Console.IsOutputRedirected && Console.CursorTop > 0)
                            {
                                int windowWidth = Console.WindowWidth > 0 ? Console.WindowWidth : 80;
                                Console.SetCursorPosition(0, Console.CursorTop - 1);
                                Console.Write(new string(' ', Math.Max(0, windowWidth - 1)));
                                Console.SetCursorPosition(0, Console.CursorTop);
                            }
                        }
                        catch
                        {

                        }

                        PrintMessage(sendResult.Data, currentUser);
                    }
                }
                else
                {
                    PrintSystemMessage("Mesaj gönderilemedi: " + sendResult.Message, ConsoleColor.Red);
                }
            }

            try
            {
                await pollingTask;
            }
            catch (Exception)
            {

            }

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\nSohbet odasından çıkıldı. Ana menüye dönülüyor...");
            Console.ResetColor();
            await Task.Delay(1000);
        }

        private void PrintMessage(MessageResponse message, string currentUser)
        {
            lock (_consoleLock)
            {
                bool isMe = message.Sender.Equals(currentUser, StringComparison.OrdinalIgnoreCase);

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write($"[{message.SentAt.ToLocalTime():HH:mm:ss}] ");

                if (isMe)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("Sen");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write(message.Sender);
                }

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($": {message.Content}");
                Console.ResetColor();
            }
        }

        private void PrintSystemMessage(string message, ConsoleColor color)
        {
            lock (_consoleLock)
            {
                Console.ForegroundColor = color;
                Console.WriteLine($"[SİSTEM] {message}");
                Console.ResetColor();
            }
        }

        private void PrintRoomHeader(int conversationId, string title, string currentUser)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("========================================================================");
            Console.WriteLine($"ODA: #{conversationId} - {title.ToUpper()}");
            Console.WriteLine($"Kullanici: {currentUser} | Cikmak icin ':cikis' yazip Enter'a basin");
            Console.WriteLine("========================================================================");
            Console.ResetColor();
        }
    }
}
