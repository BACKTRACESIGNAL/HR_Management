using HR_Management.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HR_Management.ViewModel.HR_UserControl
{
    public class EmployeeDetailViewModel : BaseViewModel
    {
        private EmployeeInfo _employeeInfo { get; set; }

        private String _employeeCode { get; set; }
        private String _fullName { get; set; }
        private String _email { get; set; }
        private String _phone { get; set; }
        private long _gender { get; set; }
        private String _detailAddress { get; set; }

        private String _province { get; set; }
        private String _district { get; set; }
        private String _ward { get; set; }
        private String _department { get; set; }
        private String _position { get; set; }

        public String MEmployeeCode { get => this._employeeCode; set { this._employeeCode = value; OnPropertyChanged(); } }
        public String MFullName { get => this._fullName; set { this._fullName = value; OnPropertyChanged(); } }
        public String MEmail { get => this._email; set { this._email = value; OnPropertyChanged(); } }
        public String MPhone { get => this._phone; set { this._phone = value; OnPropertyChanged(); } }
        public long MGender { get => this._gender; set { this._gender = value; OnPropertyChanged(); } }
        public String MDetailAddress { get => this._detailAddress; set { this._detailAddress = value; OnPropertyChanged(); } }

        public String MProvince { get => this._province; set { this._province = value; OnPropertyChanged(); } }
        public String MDistrict { get => this._district; set { this._district = value; OnPropertyChanged(); } }
        public String MWard { get => this._ward; set { this._ward = value; OnPropertyChanged(); } }
        public String MDepartment { get => this._department; set { this._department = value; OnPropertyChanged(); } }
        public String MPosition { get => this._position; set { this._position = value; OnPropertyChanged(); } }

        public EmployeeDetailViewModel(EmployeeInfo employeeInfo)
        {
            this._employeeInfo = employeeInfo;

            this._employeeCode = employeeInfo.EmployeeCode;
            this._fullName = employeeInfo.FullName;
            this._email = employeeInfo.Email; 
            this._phone = employeeInfo.Phone;
            this._gender = (long)employeeInfo.Gender;
            this._detailAddress = employeeInfo.DetailAddress;

            this._province = employeeInfo.Province.ProvinceName;
            this._district = employeeInfo.District.DistrictName;
            this._ward = employeeInfo.Ward.WardName;
            this._department = employeeInfo.Department.DepartmentName;
            this._position = employeeInfo.Position.PositionName;


        }
    }
}
