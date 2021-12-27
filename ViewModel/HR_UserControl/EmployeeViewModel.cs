using HR_Management.HR_Libs;
using HR_Management.Model;
using HR_Management.View.HR_UserControl.Models;
using MaterialDesignThemes.Wpf;
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
        public ObservableCollection<EmployeeInfoModel> EmployeeSourceData { get; set; }

        public ICommand LoadMainFormCommand { get; set; }
        public ICommand LoadMainListViewName { get; set; }

        public EmployeeViewModel()
        {
            // Initial data
            this.EmployeeSourceData = new ObservableCollection<EmployeeInfoModel>();

            // Command
            // Load main form content
            LoadMainFormCommand = new AsyncCommand<Grid>((p) => { return true; }, (p) =>
            {
                DialogHost dialogHost = Utility.GetMainForm<Grid>(p);
                if (dialogHost == null)
                {
                    return;
                }

                dialogHost.Dispatcher.Invoke(new Action(() => { dialogHost.DialogContent = new EmployeeForm(); }));
            });

            // Load main list view
            LoadMainListViewName = new AsyncCommand<ProgressBar>((p) => { return true; }, (p) =>
            {
                p.Dispatcher.Invoke(() => { p.Visibility = Visibility.Visible; });
                MongoCRUD crud = MongodbRequest.Instance().StartDbSession(MongoDefine.DATABASE.HR_DATA_DB);
                List<EmployeeInfoModel> employees = crud.GetMany<EmployeeInfoModel>(MongoDefine.COLLECTION.HR_EMPOYEE_INFO_COLLECTION, "{}");

                foreach (EmployeeInfoModel employee in employees)
                {
                    App.Current.Dispatcher.Invoke(() => {
                        this.EmployeeSourceData.Add(employee);
                    });
                }

                p.Dispatcher.Invoke(() => { p.Visibility = Visibility.Hidden; });
            });

        }
    }
}
