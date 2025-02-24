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
        private readonly string directory;
        private readonly string absolutePath;

        public static SettingsService Instance { get; set; } =
            new(Path.Join(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "LabelGenGUI"),
                "settings.json");

        public Settings Settings { get; private set; }

        public SettingsService(string directory, string file)
        {
            this.directory = directory;
            absolutePath = Path.Join(directory, file);
            Settings = Load();
        }

        public void Save()
        {
            Directory.CreateDirectory(directory);
            using var stream = File.Open(absolutePath, FileMode.Create);
            JsonSerializer.Serialize(stream, Settings, SettingsContext.Default.Settings);
        }

        public Settings Load()
        {
            if (File.Exists(absolutePath))
            {
                using var stream = File.OpenRead(absolutePath);
                if (stream.Length != 0)
                {
                    try
                    {
                        return JsonSerializer.Deserialize(stream, SettingsContext.Default.Settings)!;
                    }
                    catch { Console.WriteLine("Settings file is not valid, will be overwritten on close!"); return new(); }
                }
            }
            else
            {
                Directory.CreateDirectory(directory);
                File.Create(absolutePath).Close();
                return new();
            }
            return new();
        }
    }
}
