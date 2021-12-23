using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HR_Management.ViewModel.HR_UserControl.Models
{
    public class EmployeeFormViewModel : BaseViewModel
    {
        private bool _canAdd = true;

        private String p_fullName;
        private String p_email;
        private String p_phone;
        private String p_gender;
        private String p_province;
        private String p_district;
        private String p_detailAddress;
        private String p_department;
        private String p_position;
        private String p_status;

        public String MFullName { get => p_fullName; set { p_fullName = value; OnPropertyChanged(); } }
        public String MEmail { get => p_email; set { p_email = value; OnPropertyChanged(); } }
        public String MPhone { get => p_phone; set { p_phone = value; OnPropertyChanged(); } } 
        public String MGender { get => p_gender; set { p_gender = value; OnPropertyChanged(); } }
        public String MProvince { get => p_province; set { p_province = value; OnPropertyChanged(); } }
        public String MDistrict { get => p_district; set { p_district = value; OnPropertyChanged(); } }
        public String MDetailAddress { get => p_detailAddress; set { p_detailAddress = value; OnPropertyChanged(); } }
        public String MDepartment { get => p_department; set { p_department = value; OnPropertyChanged(); } }
        public String MPosition { get => p_position; set { p_position = value; OnPropertyChanged(); } }
        public String MStatus { get => p_status; set { p_status = value; OnPropertyChanged(); } }

        public ICommand AddNewEmployeeCommand { get; set; }

        public EmployeeFormViewModel()
        {
            AddNewEmployeeCommand = new AsyncDoubleParamCommand<Button, Button>((p, v) => { return this._canAdd; }, (p, v) =>
            {
                this._canAdd = false;

                Thread.Sleep(1000);

                this._canAdd = true;

                return true;
            });
        }
    }
}
