using HR_Management.HR_Libs;
using HR_Management.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HR_Management.ViewModel.HR_UserControl
{
    public class EmployeeViewModel : BaseViewModel
    {
        public ObservableCollection<EmployeeInfoModel> EmployeeSourceData { get; set; }

        public EmployeeViewModel()
        {
            this.EmployeeSourceData = new ObservableCollection<EmployeeInfoModel>();
            _ = LoadDataAsync();
            
        }

        private async Task LoadDataAsync()
        {
            await Task.Run(() => LoadData());
        }

        private void LoadData()
        {
            MongoDefine define = new MongoDefine();
            MongoCRUD crud = MongodbRequest.Instance().StartDbSession(define.HR_DATA_DB);
            List<EmployeeInfoModel> employees = crud.GetMany<EmployeeInfoModel>(define.HR_EMPOYEE_INFO_COLLECTION, "{}");

            foreach(EmployeeInfoModel employee in employees)
            {
                App.Current.Dispatcher.Invoke(() => {
                    this.EmployeeSourceData.Add(employee);
                });
            }
            //this.EmployeeSourceData = new ObservableCollection<EmployeeInfoModel>(employees);
            MessageBox.Show(this.EmployeeSourceData.Count.ToString());
        }
    }
}
