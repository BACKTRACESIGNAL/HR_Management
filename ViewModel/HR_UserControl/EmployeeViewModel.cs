using HR_Management.HR_Libs;
using HR_Management.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace HR_Management.ViewModel.HR_UserControl
{
    public class EmployeeViewModel : BaseViewModel
    {
        private ProgressBar p_progressBar;

        public ObservableCollection<EmployeeInfoModel> EmployeeSourceData { get; set; }

        public EmployeeViewModel(ProgressBar progressBar)
        {
            this.p_progressBar = progressBar;
            this.EmployeeSourceData = new ObservableCollection<EmployeeInfoModel>();
            _ = LoadDataAsync();
            
        }

        private async Task LoadDataAsync()
        {
            await Task.Run(() => LoadData());
        }

        private void LoadData()
        {
            this.p_progressBar.Dispatcher.Invoke(new Action(() => { this.p_progressBar.Visibility = Visibility.Visible; }));
            MongoCRUD crud = MongodbRequest.Instance().StartDbSession(MongoDefine.DATABASE.HR_DATA_DB);
            List<EmployeeInfoModel> employees = crud.GetMany<EmployeeInfoModel>(MongoDefine.COLLECTION.HR_EMPOYEE_INFO_COLLECTION, "{}");

            foreach(EmployeeInfoModel employee in employees)
            {
                App.Current.Dispatcher.Invoke(() => {
                    this.EmployeeSourceData.Add(employee);
                });
            }

            this.p_progressBar.Dispatcher.Invoke(new Action(() => { this.p_progressBar.Visibility = Visibility.Hidden; }));
        }
    }
}
