using HR_Management.View.HR_UserControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace HR_Management.ViewModel.HR_Window
{
    public class MainViewModel : BaseViewModel
    {
        private enum PAGE
        {
            DASHBOARD,
            EMPLOYEE,
            REQUEST,
            RECRUITMENT,
            ONBOARDING,
            OFFBOARDING,
            UNSUPPORTED,

        };

        private PAGE selectedPage { get; set; }

        private Storyboard ShowUserControlAnimate(UserControl userControl, Grid container)
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

        public ICommand LoadDashboardCommand { get; set; }
        public ICommand LoadEmployeeCommand { get; set; }
        public ICommand LoadRequestCommand { get; set; }
        public ICommand LoadRecruitmentCommand { get; set; }
        public ICommand LoadOnboardingCommand { get; set; }
        public ICommand LoadOffboardingCommand { get; set; }

        public ICommand LoadUnsupportedCommand { get; set; }

        public MainViewModel()
        {
            this.selectedPage = PAGE.DASHBOARD;
            LoadDashboardCommand = new RelayTripleParamCommand<Grid, Label, Label>((p, t, s) => { return true;  }, (p, t, s) =>
            {
                if (p == null || t == null || s == null)
                {
                    // TODO: Handle if params is null
                    return false;
                }
                else if (this.selectedPage != PAGE.DASHBOARD)
                {
                    t.Content = s.Content;
                    UserControl showView = new Dashboard();
                    this.ShowUserControlAnimate(showView, p).Begin();
                    this.selectedPage = PAGE.DASHBOARD;

                    return true;
                }

                return true;
            });

            LoadEmployeeCommand = new RelayCommand<Grid>((p) => { return true; }, (p) =>
            {
                if (p == null)
                {
                    // TODO: Handle if params is null
                    return;
                }
                else if (this.selectedPage != PAGE.EMPLOYEE)
                {
                    UserControl showView = new Employee();
                    this.ShowUserControlAnimate(showView, p).Begin();
                    this.selectedPage = PAGE.EMPLOYEE;
                }
            });

            LoadRequestCommand = new RelayCommand<Grid>((p) => { return true; }, (p) =>
            {
                if (p == null)
                {
                    // TODO: Handle if params is null
                    return;
                }
                else if (this.selectedPage != PAGE.REQUEST)
                {
                    Request showView = new Request();
                    p.Children.Clear();
                    p.Children.Add(showView);
                    this.selectedPage = PAGE.REQUEST;
                }
            });

            LoadRecruitmentCommand = new RelayCommand<Grid>((p) => { return true; }, (p) =>
            {
                if (p == null)
                {
                    // TODO: Handle if params is null
                    return;
                }
                else if (this.selectedPage != PAGE.RECRUITMENT)
                {
                    Recruitment showView = new Recruitment();
                    p.Children.Clear();
                    p.Children.Add(showView);
                    this.selectedPage = PAGE.RECRUITMENT;
                }
            });

            LoadOnboardingCommand = new RelayCommand<Grid>((p) => { return true; }, (p) =>
            {
                if (p == null)
                {
                    // TODO: Handle if params is null
                    return;
                }
                else if (this.selectedPage != PAGE.ONBOARDING)
                {
                    Onboarding showView = new Onboarding();
                    p.Children.Clear();
                    p.Children.Add(showView);
                    this.selectedPage = PAGE.ONBOARDING;
                }
            });

            LoadOffboardingCommand = new RelayCommand<Grid>((p) => { return true; }, (p) =>
            {
                if (p == null)
                {
                    // TODO: Handle if params is null
                    return;
                }
                else if (this.selectedPage != PAGE.OFFBOARDING)
                {
                    Offboarding showView = new Offboarding();
                    p.Children.Clear();
                    p.Children.Add(showView);
                    this.selectedPage = PAGE.OFFBOARDING;
                }
            });

            LoadUnsupportedCommand = new RelayCommand<Grid>((p) => { return true; }, (p) =>
            {
                if (p == null)
                {
                    // TODO: Handle if params is null
                    return;
                }
                else if (this.selectedPage != PAGE.UNSUPPORTED)
                {
                    Unsupported showView = new Unsupported();
                    p.Children.Clear();
                    p.Children.Add(showView);
                    this.selectedPage = PAGE.UNSUPPORTED;
                }
            });
        }
    }
}
