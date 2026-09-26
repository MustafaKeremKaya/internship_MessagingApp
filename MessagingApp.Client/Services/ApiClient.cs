using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using MessagingApp.Core.Constants;
using MessagingApp.Entities.DTOs;

namespace MessagingApp.Client.Services
{

    public class ApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public ApiClient(string baseUrl = "http://localhost:5000")
        {

            var formattedBaseUrl = baseUrl.TrimEnd('/') + "/";

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(formattedBaseUrl),
                Timeout = TimeSpan.FromSeconds(10) 
            };

            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true 
            };
        }

        public async Task<ApiResponse<List<ConversationSummaryResponse>>> GetConversationsAsync()
        {
            try
            {

                var response = await _httpClient.GetAsync("api/conversations");

                if (!response.IsSuccessStatusCode)
                {
                    return await HandleErrorResponseAsync<List<ConversationSummaryResponse>>(response);
                }

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<ConversationSummaryResponse>>>(_jsonOptions);
                return result ?? new ApiResponse<List<ConversationSummaryResponse>> { Success = false, Message = Messages.EmptyResponse };
            }
            catch (HttpRequestException)
            {
                return new ApiResponse<List<ConversationSummaryResponse>>
                {
                    Success = false,
                    Message = Messages.ServerConnectionError
                };
            }
            catch (TaskCanceledException)
            {
                return new ApiResponse<List<ConversationSummaryResponse>>
                {
                    Success = false,
                    Message = Messages.ServerTimeout
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<ConversationSummaryResponse>>
                {
                    Success = false,
                    Message = $"Beklenmedik bir hata oluştu: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<ConversationSummaryResponse>> CreateConversationAsync(string title)
        {
            try
            {
                var request = new CreateConversationRequest { Title = title };

                var response = await _httpClient.PostAsJsonAsync("api/conversations", request);

                if (!response.IsSuccessStatusCode)
                {
                    return await HandleErrorResponseAsync<ConversationSummaryResponse>(response);
                }

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<ConversationSummaryResponse>>(_jsonOptions);
                return result ?? new ApiResponse<ConversationSummaryResponse> { Success = false, Message = Messages.EmptyResponse };
            }
            catch (HttpRequestException)
            {
                return new ApiResponse<ConversationSummaryResponse>
                {
                    Success = false,
                    Message = Messages.ServerConnectionError
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<ConversationSummaryResponse>
                {
                    Success = false,
                    Message = $"Sohbet oluşturulurken hata: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<ConversationDetailResponse>> GetConversationDetailAsync(int conversationId)
        {
            try
            {

                var response = await _httpClient.GetAsync($"api/conversations/{conversationId}");

                if (!response.IsSuccessStatusCode)
                {
                    return await HandleErrorResponseAsync<ConversationDetailResponse>(response);
                }

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<ConversationDetailResponse>>(_jsonOptions);
                return result ?? new ApiResponse<ConversationDetailResponse> { Success = false, Message = Messages.EmptyResponse };
            }
            catch (HttpRequestException)
            {
                return new ApiResponse<ConversationDetailResponse>
                {
                    Success = false,
                    Message = Messages.ServerConnectionError
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<ConversationDetailResponse>
                {
                    Success = false,
                    Message = $"Detay çekilirken hata: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<List<MessageResponse>>> GetMessagesAsync(int conversationId)
        {
            try
            {

                var response = await _httpClient.GetAsync($"api/conversations/{conversationId}/messages");

                if (!response.IsSuccessStatusCode)
                {
                    return await HandleErrorResponseAsync<List<MessageResponse>>(response);
                }

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<MessageResponse>>>(_jsonOptions);
                return result ?? new ApiResponse<List<MessageResponse>> { Success = false, Message = Messages.EmptyMessageList };
            }
            catch (HttpRequestException)
            {
                return new ApiResponse<List<MessageResponse>>
                {
                    Success = false,
                    Message = Messages.ServerConnectionLost
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<MessageResponse>>
                {
                    Success = false,
                    Message = $"Mesajlar alınırken hata: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<MessageResponse>> SendMessageAsync(int conversationId, string sender, string content)
        {
            try
            {
                var request = new SendMessageRequest
                {
                    Sender = sender,
                    Content = content
                };

                var response = await _httpClient.PostAsJsonAsync($"api/conversations/{conversationId}/messages", request);

                if (!response.IsSuccessStatusCode)
                {
                    return await HandleErrorResponseAsync<MessageResponse>(response);
                }

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<MessageResponse>>(_jsonOptions);
                return result ?? new ApiResponse<MessageResponse> { Success = false, Message = Messages.EmptyResponse };
            }
            catch (HttpRequestException)
            {
                return new ApiResponse<MessageResponse>
                {
                    Success = false,
                    Message = Messages.MessageSendConnectionError
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<MessageResponse>
                {
                    Success = false,
                    Message = $"Mesaj gönderme hatası: {ex.Message}"
                };
            }
        }

        private async Task<ApiResponse<T>> HandleErrorResponseAsync<T>(HttpResponseMessage response)
        {
            try
            {

                var errorResult = await response.Content.ReadFromJsonAsync<ApiResponse<T>>(_jsonOptions);
                if (errorResult != null && !string.IsNullOrWhiteSpace(errorResult.Message))
                {
                    return errorResult;
                }
            }
            catch
            {

            }

            var message = response.StatusCode switch
            {
                System.Net.HttpStatusCode.NotFound => Messages.NotFound404,
                System.Net.HttpStatusCode.BadRequest => Messages.BadRequest400,
                System.Net.HttpStatusCode.InternalServerError => Messages.InternalServerError500,
                _ => $"Sunucu hatası: {response.StatusCode} ({(int)response.StatusCode})"
            };

            return new ApiResponse<T> { Success = false, Message = message };
        }
    }

    public class ApiResponse<T>
    {

        public bool Success { get; set; }

        public string? Message { get; set; }

        public T? Data { get; set; }
    }
}
