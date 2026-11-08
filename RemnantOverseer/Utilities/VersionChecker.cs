using System;
using System.Net.Http;
using System.Reflection;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace RemnantOverseer.Utilities;
internal static class VersionChecker
{
    internal static async Task<string?> TryGetNewVersion()
    {
        var currentVersion = Assembly.GetExecutingAssembly().GetName().Version!;
        using var http = new HttpClient();
        try
        {
            http.DefaultRequestHeaders.UserAgent.ParseAdd($"RemnantOverseer v{currentVersion}");
            var response = await http.GetAsync($"https://api.github.com/repos/angelore/remnant-two-overseer/releases/latest");
            var content = await response.Content.ReadAsStringAsync();
            var jnode = JsonNode.Parse(content);
            var latestVersionString = (string)jnode!["tag_name"]!; // Sic!
            var latestVersion = Version.Parse(latestVersionString.AsSpan(1));
            if (latestVersion > currentVersion)
            {
                return latestVersion.ToString(3);
            }
        }
        catch { }
        return null;
    }
}
