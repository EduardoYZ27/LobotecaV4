using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Loboteca.Services
{
    public class GoogleBooksService
    {
        private readonly HttpClient _httpClient;

        public GoogleBooksService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<BookSuggestion>> GetBookSuggestionsAsync(string query, string language = "es")
        {
            string url = $"https://www.googleapis.com/books/v1/volumes?q={Uri.EscapeDataString(query)}&langRestrict={language}&maxResults=5";
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var jsonDoc = JsonDocument.Parse(content);

                var results = new List<BookSuggestion>();

                foreach (var item in jsonDoc.RootElement.GetProperty("items").EnumerateArray())
                {
                    var volumeInfo = item.GetProperty("volumeInfo");

                    string title = volumeInfo.GetProperty("title").GetString();
                    string description = volumeInfo.TryGetProperty("description", out var descElement) ? descElement.GetString() : "Sin descripción disponible.";
                    string previewLink = volumeInfo.TryGetProperty("previewLink", out var previewElement) ? previewElement.GetString() : null;

                    // Generar enlace a Google Docs (simulado)
                    string googleDocsLink = previewLink != null ? $"{previewLink}&output=docs" : null;

                    results.Add(new BookSuggestion
                    {
                        Title = title,
                        Description = Regex.Replace(description, "<.*?>", string.Empty),
                        GoogleDocsLink = googleDocsLink
                    });
                }

                return results;
            }

            return new List<BookSuggestion>();
        }
    }

    public class BookSuggestion
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string GoogleDocsLink { get; set; } = string.Empty;
    }
}
