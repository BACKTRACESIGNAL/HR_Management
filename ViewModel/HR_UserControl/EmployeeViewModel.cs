using HR_Management.HR_Libs;
using HR_Management.Model;
using HR_Management.View.HR_UserControl;
using HR_Management.View.HR_UserControl.Models;
using MaterialDesignThemes.Wpf;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HR_Management.ViewModel.HR_UserControl
{
    public class EmployeeViewModel : BaseViewModel
    {
        public ObservableCollection<EmployeeInfo> EmployeeSourceData { get; set; }

        private String _searchKey { get; set; }
        private EmployeeInfo _selectedEmployee { get; set; }

        public String SearchKey { get => this._searchKey; set { this._searchKey = value; OnPropertyChanged(); HandleSearchPrivateEvent(); } }
        public EmployeeInfo MSelectedEmployee { get => this._selectedEmployee; set { this._selectedEmployee = value; OnPropertyChanged(); } }

        public ICommand LoadMainFormCommand { get; set; }
        public ICommand LoadEmployeeCommand { get; set; }
        public ICommand HandleLoadEmployeeDetailCommand { get; set; }

        public EmployeeViewModel()
        {
            PlayYard.Instance().SetEmployeeViewModel(this);

            // Initial data
            this.EmployeeSourceData = new ObservableCollection<EmployeeInfo>();

            // Register event
            // command
            LoadMainFormCommand = new AsyncCommand<Grid>((p) => { return true; }, (p) =>
            {
                DialogHost dialogHost = Utility.GetParentDialogHost<Grid>(p);
                if (dialogHost == null)
                {
                    return;
                }

                dialogHost.Dispatcher.Invoke(new Action(() => { dialogHost.DialogContent = new EmployeeForm(); }));
            });

            // command
            LoadEmployeeCommand = new AsyncCommand<ProgressBar>((p) => { return true; }, (p) =>
            {
                p.Dispatcher.Invoke(() => { p.Visibility = Visibility.Visible; });
                FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Empty;
                MongoCRUD crud = MongodbRequest.Instance().StartDbSession(MongoDefine.DATABASE.HR_DATA_DB);
                List<EmployeeInfo> employees = crud.GetDistinct<EmployeeInfo>(MongoDefine.COLLECTION.HR_DEPARTMENT_COLLECTION, "EmployeeInfos", filter);

                foreach (EmployeeInfo employee in employees)
                {
                    App.Current.Dispatcher.Invoke(() => {
                        this.EmployeeSourceData.Add(employee);
                    });
                }

                p.Dispatcher.Invoke(() => { p.Visibility = Visibility.Hidden; });
            });

            // command
            HandleLoadEmployeeDetailCommand = new AsyncCommand<Grid>((p) => { return true; }, (p) =>
            {
                Grid mainViewElement = Utility.GetParentFrameworkElementBaseNameDispatch(p, "mainViewName") as Grid;

                App.Current.Dispatcher.Invoke(() =>
                {
                    UserControl showView = new EmployeeDetail(this._selectedEmployee);
                    mainViewElement.Children.Clear();
                    Utility.ShowUserControlAnimate(showView, mainViewElement);
                    PlayYard.Instance().SelectedPageGlobal = PlayYard.PAGE.EMPLOYEE_DETAIL;
                });
            });

        }

        ~EmployeeViewModel()
        {
            PlayYard.Instance().SetEmployeeViewModel(null);
        }

        private void HandleSearchPrivateEvent()
        {

        }
    }
}
