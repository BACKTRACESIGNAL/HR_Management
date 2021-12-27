using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HR_Management.HR_Libs
{
    public class Utility
    {
        static public DialogHost GetMainForm<T>(T control)
        {
            FrameworkElement parent = control as FrameworkElement, grandParent = parent.Parent as FrameworkElement;

            while (grandParent != null && grandParent.Parent != null)
            {
                parent = grandParent;
                grandParent = parent.Parent as FrameworkElement;
            }

            return parent as DialogHost;
        }
    }
}
