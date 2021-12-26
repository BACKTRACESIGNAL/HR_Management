﻿using HR_Management.HR_Libs;
using HR_Management.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HR_Management.ViewModel.HR_UserControl
{
    public class DashboardViewModel : BaseViewModel
    {
        public ObservableCollection<EmployeeInfoModel> EmployeeSourceData { get; set; }

        public DashboardViewModel()
        {
            this.EmployeeSourceData = new ObservableCollection<EmployeeInfoModel>();
            //List<EmployeeInfoModel> employees = MongodbRequest.Instance().StartDbSession(define.HR_DATA_DB).GetMany<EmployeeInfoModel>(define.HR_EMPOYEE_INFO_COLLECTION);

            //this.EmployeeSourceData = new ObservableCollection<EmployeeInfoModel>(employees);
        }
    }
}
