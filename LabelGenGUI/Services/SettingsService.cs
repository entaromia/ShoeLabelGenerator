using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace LabelGenGUI.Services
{
    public class Settings
    {
        public List<Printer> Printers { get; set; } = [];
        public Printer? CurrentPrinter
        {
            get => Printers.Count > 0 ? Printers[currentPrinterIndex] : null;
            set
            {
                currentPrinterIndex = value is null ? 0 : Printers.IndexOf(value);
            }
        }
        private int currentPrinterIndex = 0;
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
                "settings.txt"));

        public Settings Settings { get; private set; }

        public SettingsService(string filePath)
        {
            this.filePath = filePath;
            Settings = Load();
        }

        public void Save()
        {
            using var stream = File.Open(filePath, FileMode.Create);
            JsonSerializer.Serialize(stream, Settings);
        }

        public Settings Load()
        {
            if (File.Exists(filePath))
            {
                using var stream = File.OpenRead(filePath);
                if (stream.Length != 0)
                {
                    try { return JsonSerializer.Deserialize<Settings>(stream)!; }
                    catch { Console.WriteLine("Settings file is not valid, will be overwritten on close!"); return new(); }
                }
            }
            return new();
        }
    }
}
