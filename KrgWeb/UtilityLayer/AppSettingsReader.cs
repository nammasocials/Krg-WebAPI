using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.FileProviders;
using System.IO;
using System.Text.Json;

public static class AppSettingsReader
{
    private static IConfiguration Configuration { get; }

    static AppSettingsReader()
    {
        Configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory()) // Adjust if needed
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
    }

    /// <summary>
    /// Reads a single connection string by name.
    /// </summary>
    public static string GetConnectionString(string name)
    {
        return Configuration.GetConnectionString(name);
    }
    public static string GetSetting(string name)
    {
        return Configuration.GetValue<string>(name);
    }

    /// <summary>
    /// Reads an entire section and deserializes it into a generic object.
    /// </summary>
    public static T GetSection<T>(string sectionName)
    {
        var section = Configuration.GetSection(sectionName);
        return section.Get<T>();
    }
}
