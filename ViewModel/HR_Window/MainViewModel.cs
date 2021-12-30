using HR_Management.HR_Libs;
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
            // Start up database instance
            StartUp startUp = new StartUp();
            bool ok = startUp.Load();

            // Assign Commands
            PlayYard.Instance().SelectedPageGlobal = PlayYard.PAGE.DASHBOARD;
            LoadDashboardCommand = new RelayCommand<Grid, Label, Label>((p, t, s) => { return true;  }, (p, t, s) =>
            {
                if (p == null || t == null || s == null)
                {
                    // TODO: Handle if params is null
                }
                else if (PlayYard.Instance().SelectedPageGlobal != PlayYard.PAGE.DASHBOARD)
                {
                    t.Content = s.Content;
                    UserControl showView = new Dashboard();
                    this.ShowUserControlAnimate(showView, p).Begin();
                    PlayYard.Instance().SelectedPageGlobal = PlayYard.PAGE.DASHBOARD;
                }
            });

            LoadEmployeeCommand = new RelayCommand<Grid, Label, Label>((p, t, s) => { return true; }, (p, t, s) =>
            {
                if (p == null || t == null || s == null)
                {
                    // TODO: Handle if params is null
                    return;
                }
                else if (PlayYard.Instance().SelectedPageGlobal != PlayYard.PAGE.EMPLOYEE)
                {
                    t.Content= s.Content;
                    UserControl showView = new Employee();
                    this.ShowUserControlAnimate(showView, p).Begin();
                    PlayYard.Instance().SelectedPageGlobal = PlayYard.PAGE.EMPLOYEE;
                }
            });

            LoadRequestCommand = new RelayCommand<Grid>((p) => { return true; }, (p) =>
            {
                if (p == null)
                {
                    // TODO: Handle if params is null
                    return;
                }
                else if (PlayYard.Instance().SelectedPageGlobal != PlayYard.PAGE.REQUEST)
                {
                    Request showView = new Request();
                    p.Children.Clear();
                    p.Children.Add(showView);
                    PlayYard.Instance().SelectedPageGlobal = PlayYard.PAGE.REQUEST;
                }
            });

            LoadRecruitmentCommand = new RelayCommand<Grid>((p) => { return true; }, (p) =>
            {
                if (p == null)
                {
                    // TODO: Handle if params is null
                    return;
                }
                else if (PlayYard.Instance().SelectedPageGlobal != PlayYard.PAGE.RECRUITMENT)
                {
                    Recruitment showView = new Recruitment();
                    p.Children.Clear();
                    p.Children.Add(showView);
                    PlayYard.Instance().SelectedPageGlobal = PlayYard.PAGE.RECRUITMENT;
                }
            });

            LoadOnboardingCommand = new RelayCommand<Grid>((p) => { return true; }, (p) =>
            {
                if (p == null)
                {
                    // TODO: Handle if params is null
                    return;
                }
                else if ( PlayYard.Instance().SelectedPageGlobal != PlayYard.PAGE.RECRUITMENT)
                {
                    Onboarding showView = new Onboarding();
                    p.Children.Clear();
                    p.Children.Add(showView);
                    PlayYard.Instance().SelectedPageGlobal = PlayYard.PAGE.RECRUITMENT;
                }
            });

            LoadOffboardingCommand = new RelayCommand<Grid>((p) => { return true; }, (p) =>
            {
                if (p == null)
                {
                    // TODO: Handle if params is null
                    return;
                }
                else if (PlayYard.Instance().SelectedPageGlobal != PlayYard.PAGE.OFFBOARDING)
                {
                    Offboarding showView = new Offboarding();
                    p.Children.Clear();
                    p.Children.Add(showView);
                    PlayYard.Instance().SelectedPageGlobal = PlayYard.PAGE.OFFBOARDING;
                }
            });

            LoadUnsupportedCommand = new RelayCommand<Grid>((p) => { return true; }, (p) =>
            {
                if (p == null)
                {
                    // TODO: Handle if params is null
                    return;
                }
                else if (PlayYard.Instance().SelectedPageGlobal != PlayYard.PAGE.UNSUPPORTED)
                {
                    Unsupported showView = new Unsupported();
                    p.Children.Clear();
                    p.Children.Add(showView);
                    PlayYard.Instance().SelectedPageGlobal = PlayYard.PAGE.UNSUPPORTED;
                }
            });
        }
    }
}
