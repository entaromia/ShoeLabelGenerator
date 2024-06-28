using ShoeLabelGen.Common;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace LabelGenGUI.Services
{
    public class ShoeListService
    {
        public string? CurrentFile { get; set; }
        public static ShoeListService Instance { get; } = new();

        private class ShoeListData
        {
            public int FileVersion { get; set; } = 1;
            public string? ProjectName { get; set; }
            public ObservableCollection<ShoeListItem> ShoeItems { get; set; } = [];
        }

        private readonly ShoeListData shoeListData = new();
        public ObservableCollection<ShoeListItem> GetItems() => shoeListData.ShoeItems;
        public int ItemCount => shoeListData.ShoeItems.Count;

        public string? ProjectName
        {
            get => shoeListData.ProjectName;
            set => shoeListData.ProjectName = value;
        }

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
            var stream = File.Open(CurrentFile!, FileMode.Create);
            await JsonSerializer.SerializeAsync(stream, shoeListData);
            stream.Close();
        }

        public async Task OpenFileAsync()
        {
            var stream = File.Open(CurrentFile!, FileMode.OpenOrCreate);
            var temp = await JsonSerializer.DeserializeAsync<ShoeListData>(stream);
            if (temp is not null)
            {
                shoeListData.FileVersion = temp.FileVersion > 1 ? temp.FileVersion : 1;
                shoeListData.ProjectName = temp.ProjectName ?? "";
                shoeListData.ShoeItems.Clear();
                foreach (var item in temp.ShoeItems)
                {
                    shoeListData.ShoeItems.Add(item);
                }
            }
            stream.Close();
        }

        public void CloseProject()
        {
            shoeListData.ShoeItems.Clear();
            shoeListData.ProjectName = null;
            CurrentFile = null;
        }
    }
}
