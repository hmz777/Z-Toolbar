using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Z_Toolbar.MainLogic;

namespace Z_Toolbar.Tools
{
    public static class HelperMethods
    {
        internal static bool AppExists(this ObservableCollection<ApplicationItem> items, ApplicationItem app)
        {
            foreach (var item in items)
            {
                if (item.Path == app.Path)
                {
                    return true;
                }
            }

            return false;
        }

        internal static T FindAncestor<T>(DependencyObject child, Type type, string parentName) where T : DependencyObject
        {
            if (child == null)
                return null;

            DependencyObject currentParent = VisualTreeHelper.GetParent(child);
            FrameworkElement FE = null;
            T requestedParent = null;

            while (currentParent != null)
            {                
                FE = currentParent as FrameworkElement;
                
                if (FE is T && FE.Name == parentName)
                {
                    requestedParent = (T)currentParent;
                    break;
                }

                currentParent = VisualTreeHelper.GetParent(currentParent);
            }

            return requestedParent;
        }
    }
}
