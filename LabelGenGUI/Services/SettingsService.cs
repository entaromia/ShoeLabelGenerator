using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LabelGenGUI.Services
{
    public class Settings
    {
        public ObservableCollection<Printer> Printers { get; set; } = [];
        public Printer? CurrentPrinter
        {
            get => Printers.Count > 0 ? currentPrinter : null;
            set => currentPrinter = value;
        }
        private Printer? currentPrinter;
    }

    [JsonSerializable(typeof(Settings))]
    internal partial class SettingsContext : JsonSerializerContext
    {
    }

    public class Printer
    {
        public string? Name { get; set; }
        public string? Uri { get; set; }

        public override string ToString()
        {
            return $"Name: {Name}, Uri: {Uri}";
        }
    }

    public class SettingsService
    {
        private readonly string filePath;

        public static SettingsService Instance { get; set; } = 
            new(Path.Join(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "LabelGenGUI",
                "settings.json"));

        public Settings Settings { get; private set; }

        public SettingsService(string filePath)
        {
            this.filePath = filePath;
            Settings = Load();
        }

        public void Save()
        {
            using var stream = File.Open(filePath, FileMode.Create);
            JsonSerializer.Serialize(stream, Settings, SettingsContext.Default.Settings);
        }

        public Settings Load()
        {
            if (File.Exists(filePath))
            {
                using var stream = File.OpenRead(filePath);
                if (stream.Length != 0)
                {
                    try
                    {
                        return JsonSerializer.Deserialize(stream, SettingsContext.Default.Settings)!;
                    }
                    catch { Console.WriteLine("Settings file is not valid, will be overwritten on close!"); return new(); }
                }
            }
            return new();
        }
    }
}
