using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace RemnantOverseer.Utilities;

internal static class ItemDetailsCatalog
{
    private static readonly Lazy<Dictionary<string, string>> Catalog = new(() =>
    {
        try
        {
            var path = Path.Combine(AppContext.BaseDirectory, "item-details.json");
            return JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(path)) ?? new();
        }
        catch { return new(); }
    });

    private static readonly Lazy<Dictionary<string, string>> Translations = new(() =>
    {
        try { return JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "translations-uk.json"))) ?? new(); }
        catch { return new(); }
    });
    public static string Translate(string english) => string.IsNullOrWhiteSpace(english) ? string.Empty :
        Translations.Value.TryGetValue(english, out var translation) ? translation : "Переклад відсутній: " + english;

    private static readonly Lazy<Dictionary<string, string>> Reviewed = new(() =>
    {
        try { return JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "reviewed-uk.json"))) ?? new(); }
        catch { return new(); }
    });
    public static bool HasReviewed(string id) => Reviewed.Value.ContainsKey(id);
    public static string GetReviewed(string id) => Reviewed.Value.TryGetValue(id, out var value) ? value : string.Empty;
    public static string Get(string id) => Catalog.Value.TryGetValue(id, out var detail) ? detail : string.Empty;
}
