using HR_Management.HR_Libs;
using HR_Management.View.HR_UserControl;
using HR_Management.View.HR_Window;
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
        public ICommand HandleDialogOpenedCommand { get; set; }
        public ICommand LoadDashboardCommand { get; set; }
        public ICommand LoadEmployeeCommand { get; set; }
        public ICommand LoadRequestCommand { get; set; }
        public ICommand LoadRecruitmentCommand { get; set; }
        public ICommand LoadOnboardingCommand { get; set; }
        public ICommand LoadOffboardingCommand { get; set; }
        public ICommand LoadUnsupportedCommand { get; set; }

        public ICommand HandleLogoutCommand { get; set; }

        public MainViewModel()
        {
            // Start up database instance   
            StartUp startUp = new StartUp();
            bool ok = startUp.Load();

            // -------------------------------------------------------------------------- //
            // @TODO: Handle start up fail
            // -------------------------------------------------------------------------- //

            // Login
            //_ = this.HandleLogin();


            // Assign Commands
            PlayYard.Instance().SelectedPageGlobal = PlayYard.PAGE.DASHBOARD;

            HandleDialogOpenedCommand = new RelayCommand<Label>((p) => { return true; }, (p) =>
            {
                PlayYard.Instance().openingStrategy.DoAlgorithm();
            });

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
                    Utility.ShowUserControlAnimate(showView, p).Begin();
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
                    Utility.ShowUserControlAnimate(showView, p).Begin();
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

            HandleLogoutCommand = new AsyncCommand<Window>((p) => { return true; }, (p) =>
            {
                p.Dispatcher.Invoke(() =>
                {
                    p.Hide();
                    bool retOk = this.HandleLogin();
                    if (retOk == ok)
                    {
                        p.Show();
                    }
                    else
                    {
                        Environment.Exit(0);
                    }
                });
            });
        }

        private bool HandleLogin()
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.ShowDialog();

            LoginViewModel loginViewModel = loginWindow.DataContext as LoginViewModel;
            if (loginViewModel == null)
            {
                // @TODO: Handle if login viewModel is null
                return false;
            }

            if (loginViewModel.LoginSuccess == false)
            {
                Environment.Exit(0);
                return false;
            }

            return true;
        }
    }
}
