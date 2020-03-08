using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Z_Toolbar.Tools;

namespace Z_Toolbar.MainLogic
{
    internal class DelCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            string tag = parameter as string;
            var app = MainWindow.loa.Where(x => x.Path == tag).FirstOrDefault();
            if (app == null) return;
            MainWindow.loa.Remove(app);
            await Serializer.Serialize(MainWindow.loa).ConfigureAwait(false);
        }
    }
}
