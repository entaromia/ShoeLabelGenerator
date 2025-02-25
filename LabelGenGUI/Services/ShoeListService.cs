using Avalonia.Platform.Storage;
using ShoeLabelGen.Common;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LabelGenGUI.Services
{
    internal class ShoeListData
    {
        public int FileVersion { get; set; } = 2;
        public ObservableCollection<ShoeListItem> ShoeItems { get; set; } = [];
    }

    [JsonSerializable(typeof(ShoeListData))]
    internal partial class ShoeListDataContext : JsonSerializerContext
    {
    }

    public class ShoeListService
    {
        public IStorageFile? CurrentFile { get; set; }
        public static ShoeListService Instance { get; } = new();

        private readonly ShoeListData shoeListData = new();
        public ObservableCollection<ShoeListItem> GetItems() => shoeListData.ShoeItems;
        public int ItemCount => shoeListData.ShoeItems.Count;

        public bool ProjectOpen { get; set;}

        public void AddItem(ShoeListItem item)
        {
            shoeListData.ShoeItems.Add(item);
        }

        public void DeleteItem(int index)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(index);
            shoeListData.ShoeItems.RemoveAt(index);
        }

        public int GetTotalBox()
        {
            int total = 0;
            foreach (var item in shoeListData.ShoeItems)
            {
                total += item.Total;
            }
            return total;
        }

        public int GetTotalParcel()
        {
            int total = 0;
            foreach (var item in shoeListData.ShoeItems)
            {
                // If total is more than 12, there will be 2 parcel labels for that list
                total += item.Total > 12 ? 2 : 1;
            }
            return total;
        }

        public async Task SaveToFileAsync()
        {
            var stream = await CurrentFile!.OpenWriteAsync(); // opens in FileMode.Create
            await JsonSerializer.SerializeAsync(stream, shoeListData, ShoeListDataContext.Default.ShoeListData);
            stream.Close();
        }

        public async Task<bool> OpenFileAsync()
        {
            var stream = await CurrentFile!.OpenReadAsync();
            try
            {
                var temp = await JsonSerializer.DeserializeAsync(stream, ShoeListDataContext.Default.ShoeListData);
                if (temp is not null)
                {
                    shoeListData.FileVersion = temp.FileVersion > 2 ? temp.FileVersion : 2;
                    shoeListData.ShoeItems.Clear();
                    foreach (var item in temp.ShoeItems)
                    {
                        shoeListData.ShoeItems.Add(item);
                    }
                }
                stream.Close();
                ProjectOpen = true;
                return true;
            }
            catch
            {
                Debug.WriteLine("Unsupported json file, do nothing");
                return false;
            }
        }

        public void CloseProject()
        {
            shoeListData.ShoeItems.Clear();
            CurrentFile = null;
            ProjectOpen = false;
        }
    }
}
