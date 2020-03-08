using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Z_Toolbar.MainLogic
{
    [Serializable]
    public class ApplicationItem
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public bool IsFolder { get; set; }

        [JsonIgnore]
        public BitmapSource Icon { get; set; }

        public ICommand delCommand;
        public ICommand runCommand;

        [JsonIgnore]
        public ICommand DelCommand
        {
            get
            {
                return delCommand = new DelCommand();
            }

            set
            {
                delCommand = value;
            }
        }

        [JsonIgnore]
        public ICommand RunCommand
        {
            get
            {
                return runCommand = new RunCommand();
            }

            set
            {
                runCommand = value;
            }
        }
    }
}
