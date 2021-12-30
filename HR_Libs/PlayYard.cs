using HR_Management.ViewModel.HR_UserControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_Management.HR_Libs
{
    public class PlayYard
    {
        public enum PAGE
        {
            DASHBOARD,
            EMPLOYEE,
            EMPLOYEE_DETAIL,
            REQUEST,
            RECRUITMENT,
            ONBOARDING,
            OFFBOARDING,
            UNSUPPORTED,
        };
        private static readonly object _lock = new object();
        private PlayYard()
        {

        }

        private EmployeeViewModel _employeeViewModel;
        public PAGE SelectedPageGlobal { get; set; } = PAGE.DASHBOARD;
        public IStrategyOpeningDialogHost openingStrategy { get; set; }

        private static PlayYard _instance;

        public static PlayYard Instance()
        {
            if (_instance == null)
            {
                lock(_lock)
                {

                    if (_instance == null)
                    {
                        _instance = new PlayYard();
                    }
                }
            }

            return _instance;
        }













        public void SetEmployeeViewModel(EmployeeViewModel employeeViewModel)
        {
            this._employeeViewModel = employeeViewModel;
        }

        public EmployeeViewModel GetEmployeeViewModel()
        {
            return this._employeeViewModel;
        }
    }
}
