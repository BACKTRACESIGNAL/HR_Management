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
        private PlayYard()
        {

        }

        private EmployeeViewModel _employeeViewModel;


        private static PlayYard _instance;

        public static PlayYard Instance()
        {
            if (_instance == null)
            {
                _instance = new PlayYard();
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
