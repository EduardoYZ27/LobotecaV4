using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Loboteca.Services
{
    public class MediaWikiService
    {
        private readonly HttpClient _httpClient;

        public MediaWikiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<SearchResult>> SearchArticlesAsync(string query)
        {
            string url = $"https://es.wikipedia.org/w/api.php?action=query&list=search&format=json&srsearch={Uri.EscapeDataString(query)}";
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var jsonDoc = JsonDocument.Parse(content);

                var results = new List<SearchResult>();

                foreach (var item in jsonDoc.RootElement.GetProperty("query").GetProperty("search").EnumerateArray())
                {
                    var title = item.GetProperty("title").GetString();
                    var snippet = item.GetProperty("snippet").GetString();

                    // Eliminar etiquetas HTML del snippet
                    snippet = Regex.Replace(snippet, "<.*?>", string.Empty);

                    results.Add(new SearchResult
                    {
                        Title = title,
                        Snippet = snippet
                    });
                }

                return results;
            }

            return new List<SearchResult>();
        }
    }

    public class SearchResult
    {
        public string Title { get; set; } = string.Empty;
        public string Snippet { get; set; } = string.Empty;
        public string Url => $"https://es.wikipedia.org/wiki/{Uri.EscapeDataString(Title)}";
    }
}
