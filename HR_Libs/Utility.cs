using HR_Management.Model;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

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

        public static Storyboard ShowUserControlAnimate(UserControl userControl, Grid container)
        {
            userControl.RenderTransform = new TranslateTransform();
            container.Children.Clear();
            container.Children.Add(userControl);

            Storyboard sb = new Storyboard();
            DoubleAnimation slide = new DoubleAnimation();

            slide.To = 0;
            slide.From = container.ActualWidth;
            slide.Duration = new Duration(TimeSpan.FromMilliseconds(400));

            Storyboard.SetTarget(slide, userControl);
            Storyboard.SetTargetProperty(slide, new PropertyPath("RenderTransform.(TranslateTransform.X)"));

            sb.Children.Add(slide);
            return sb;
        }

        static public class GLOBAL_VARIABLE
        {
            public static string DEFAULT_GROUP_CODE { get; set; } = "GROUP00001";
            public static string DEFAULT_GROUP_NAME { get; set; } = "Utility Group";
            public static long GROUP_PARTION_OWNER { get; set; } = 1;
            public static long GROUP_PARTION_VICE_OWNER { get; set; } = 2;
            public static long GROUP_PARTION_MEMBER { get; set; } = 3;

            // saving account after login
            public static Account ACCOUNT_CACHED { get; set; }
        }

    }
}
