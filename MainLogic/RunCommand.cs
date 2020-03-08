using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Z_Toolbar.UiComponents;

namespace Z_Toolbar.MainLogic
{
    internal class RunCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            try
            {
                var path = parameter.ToString();

                if (path.Contains(".exe") || path.Contains(".bat"))
                {
                    if (File.Exists(path))
                    {
                        Process.Start(path);
                    }
                    else
                    {
                        CustomMessageBox cmb = new CustomMessageBox("It Appears that the file you requested no longer exists", MessageBoxIcon.Error);
                        cmb.ShowDialog();
                    }
                }
                else
                {
                    if (Directory.Exists(path))
                    {
                        Process.Start(path);
                    }
                    else
                    {
                        CustomMessageBox cmb = new CustomMessageBox("It Appears that the folder you requested no longer exists", MessageBoxIcon.Error);
                        cmb.ShowDialog();
                    }
                }


            }
            catch (Win32Exception)
            {
                //If the user denied the UAC prompt an exception will be thrown "Operation cancelled by the user"         
            }
        }
    }
}
