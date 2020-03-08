using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace Z_Toolbar.MainLogic
{
    public static class Serializer
    {
        public async static Task Serialize(ObservableCollection<ApplicationItem> t)
        {
            var json = JsonSerializer.Serialize<ObservableCollection<ApplicationItem>>(t);

            using (StreamWriter sw = new StreamWriter(MainWindow.ConfigLocation, false))
            {
                await sw.WriteAsync(json);
            }

        }

        public async static Task<ObservableCollection<ApplicationItem>> Deserialize()
        {
            ObservableCollection<ApplicationItem> loa;
            using (StreamReader sr = new StreamReader(MainWindow.ConfigLocation))
            {
                var json = await sr.ReadToEndAsync();
                loa = JsonSerializer.Deserialize<ObservableCollection<ApplicationItem>>(json);
            }

            return loa;
        }
    }
}
