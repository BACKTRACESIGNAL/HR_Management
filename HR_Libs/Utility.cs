using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace HR_Management.HR_Libs
{
    public class Utility
    {
        static public DialogHost GetParentDialogHost<T>(T control)
        {
            FrameworkElement parent = control as FrameworkElement, grandParent = parent.Parent as FrameworkElement;

            while (grandParent != null && grandParent.Parent != null)
            {
                parent = grandParent;
                grandParent = parent.Parent as FrameworkElement;
            }

            return parent as DialogHost;
        }

        static public FrameworkElement GetParentFrameworkElementBaseNameDispatch<T>(T control, string name)
        {
            FrameworkElement parent = control as FrameworkElement;
            while (parent != null && parent.Parent != null)
            {
                bool ok = false;
                App.Current.Dispatcher.Invoke(() =>
                {
                    if (parent.Name == name)
                    {
                        ok = true;
                    }
                });

                if (ok) return parent;
                parent = parent.Parent as FrameworkElement;
            }

            return null;
        }
    }
}
