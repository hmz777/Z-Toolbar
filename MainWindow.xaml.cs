using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using Z_Toolbar.MainLogic;
using System.Linq;
using Z_Toolbar.Tools;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Windows.Media;
using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Threading.Tasks;
using Z_Toolbar.UiComponents;

namespace Z_Toolbar
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Globar Variables
        internal static ObservableCollection<ApplicationItem> loa = new ObservableCollection<ApplicationItem>();
        internal static string MainExecutable = Assembly.GetExecutingAssembly().Location;
        internal static string MainLocation = Path.GetDirectoryName(MainExecutable); //MainExecutable.Remove(MainExecutable.LastIndexOf('\\'), (MainExecutable.Length - MainExecutable.LastIndexOf('\\')))
        internal static string ConfigLocation = Path.Combine(MainLocation, "Apps.json");
        //Globar Variables

        public MainWindow()
        {
            InitializeComponent();

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) //Window drag event handler via the drag button
            {
                window.DragMove();
                e.Handled = true;
            }
        }

        private async void MenuItem_Click_plus(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                AddExtension = true,
                CheckPathExists = true,
                DefaultExt = "exe",
                Filter = "Applications (*.exe)|*.exe|Batch Files (*.bat)|*.bat",
                Multiselect = true,
                Title = "Add Shortcuts",
                ValidateNames = true
            };

            if (ofd.ShowDialog() == true)
            {
                foreach (var item in ofd.FileNames)
                {
                    var app = new ApplicationItem()
                    {
                        Name = item.Remove(0, item.LastIndexOf('\\') + 1),
                        Path = item,
                        IconBytes = UITools.GetIcon(item, false),
                        IsFolder = false
                    };

                    if (!loa.AppExists(app)) //Check if app exists in the list
                    {
                        loa.Add(app);
                    }
                }

                lv.ItemsSource = loa;

                await DataLogic.SaveDataAsync(loa).ConfigureAwait(false);  //Save apps to Apps.json
            }
        }

        private async void window_Initialized(object sender, EventArgs e)
        {
            if (File.Exists(ConfigLocation))
            {
                try
                {
                    loa = await DataLogic.LoadDataAsync(); //Get apps from Apps.json
                    loa = UITools.PopulateIcons(loa);
                    lv.ItemsSource = loa;
                    await DataLogic.SaveDataAsync(loa).ConfigureAwait(false);
                }
                catch (Exception)
                {
                    CustomMessageBox cmb = new CustomMessageBox("It Appears that app data is either missing or corrupt, if the \"Apps.json\" file is still present try deleting it and restarting the program.", MessageBoxIcon.Error);
                    cmb.ShowDialog();
                }
            }
        }

        private void window_MouseEnter(object sender, MouseEventArgs e)
        {
            Window win = sender as Window;

            if (win.Top < 0) //Simple slide bottom animation
            {
                while (win.Top < 0)
                {
                    win.Top += 0.5; //Adjust for smoother animation

                    if (win.Opacity < 1)
                    {
                        win.Opacity += 0.1;
                    }
                }
            }

            e.Handled = true;
        }

        private void window_MouseLeave(object sender, MouseEventArgs e)
        {
            Window win = sender as Window;

            if (win.Top >= 0) //Simple slide top animation
            {
                while (win.Top > -135)
                {
                    win.Top -= 0.5; //Adjust for smoother animation

                    if (win.Opacity > 0.2)
                    {
                        win.Opacity -= 0.1;
                    }
                }
            }

            e.Handled = true;
        }

        private void MenuItem_Click_Prop(object sender, RoutedEventArgs e)
        {
            UiComponents.Settings s = new UiComponents.Settings();
            s.ShowDialog();
        }

        private void lv_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Decorator border = VisualTreeHelper.GetChild((ListView)sender, 0) as Decorator;
            ScrollViewer scrollviewer = border.Child as ScrollViewer;

            if (e.Delta < 0)
            {
                scrollviewer.PageRight();
            }
            else
            {
                scrollviewer.PageLeft();
            }

            e.Handled = true;
        }

        private async void MenuItem_Click_Folder(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog cofd = new CommonOpenFileDialog()
            {
                IsFolderPicker = true,
                EnsureFileExists = true,
                EnsurePathExists = true,
                Multiselect = true,
                Title = "Pick a folder",
                EnsureValidNames = true
            };

            if (cofd.ShowDialog() == CommonFileDialogResult.Ok)
            {
                foreach (var item in cofd.FileNames)
                {

                    var app = new ApplicationItem()
                    {
                        Name = item.Remove(0, item.LastIndexOf('\\') + 1),
                        Path = item,
                        IconBytes = UITools.GetIcon(item, true),
                        IsFolder = true
                    };

                    if (!loa.AppExists(app)) //Check if app exists in the list
                    {
                        loa.Add(app);
                    }
                }

                lv.ItemsSource = loa;

                await DataLogic.SaveDataAsync(loa).ConfigureAwait(false); //Save apps to Apps.json
            }

            cofd.Dispose();
        }
    }
}
