using HR_Management.HR_Libs;
using HR_Management.Model;
using MaterialDesignThemes.Wpf;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HR_Management.ViewModel.HR_UserControl.Models
{
    public class EmployeeFormViewModel : BaseViewModel, IDataErrorInfo
    {
        private bool _canAdd = true;

        private String _employeeCode { get; set; }
        private String _fullName { get; set; }
        private String _email { get; set; }
        private String _phone { get; set; }
        private long   _gender { get; set; }
        private String _detailAddress { get; set; }

        private AdminisProvince _selectedAdminisProvince { get; set; }
        private AdminisDistrict _selectedAdminisDistrict { get; set; }
        private AdminisWard _selectedAdminisWard { get; set; }
        private Department _selectedDepartment { get; set; }
        private Position _selectedPosition { get; set; }

        // @TODO: Validate data stuff
        // https://docs.microsoft.com/en-us/previous-versions/windows/apps/743swcz7(v=vs.105)
        //
        public String MEmployeeCode { get => _employeeCode; set { _employeeCode = value; OnPropertyChanged(); } }
        public String MFullName { get => _fullName; set { _fullName = value; OnPropertyChanged(); } }
        public String MEmail { get => _email; set { _email = value; OnPropertyChanged(); } }
        public String MPhone { get => _phone; set { _phone = value; OnPropertyChanged(); } }
        public long   MGender { get => _gender; set { _gender = value; OnPropertyChanged(); } }
        public String MDetailAddress { get => _detailAddress; set { _detailAddress = value; OnPropertyChanged(); } }

        public AdminisProvince SelectedAdminisProvince { get => this._selectedAdminisProvince; set { this._selectedAdminisProvince = value; OnPropertyChanged(); } }
        public AdminisDistrict SelectedAdminisDistrict { get => this._selectedAdminisDistrict; set { this._selectedAdminisDistrict = value; OnPropertyChanged(); } }
        public AdminisWard SelectedAdminisWard { get => this._selectedAdminisWard; set { this._selectedAdminisWard = value; OnPropertyChanged(); } }
        public Department SelectedDepartment { get => this._selectedDepartment; set { this._selectedDepartment = value; OnPropertyChanged(); } }
        public Position SelectedPosition { get => this._selectedPosition; set { this._selectedPosition = value; OnPropertyChanged(); } }

        public ObservableCollection<AdminisProvince> ProvincesSource { get; set; }
        public ObservableCollection<AdminisDistrict> DistrictsSource { get; set; }
        public ObservableCollection<AdminisWard> WardsSource { get; set; }
        public ObservableCollection<Department> DepartmentsSource { get; set; }
        public ObservableCollection<Position> PositionsSource { get; set; }

        public ICommand AddNewEmployeeCommand { get; set; }
        public ICommand LoadProvinceCommand { get; set; }
        public ICommand LoadDepartmentCommand { get; set; }
        public ICommand HandleProvinceChangedCommand { get; set; }
        public ICommand HandleDistrictChangedCommand { get; set; }
        public ICommand HandleDepartmentChangedCommand { get; set; }

        public EmployeeFormViewModel()
        {
            // intitial data
            this.ProvincesSource = new ObservableCollection<AdminisProvince>();
            this.DistrictsSource = new ObservableCollection<AdminisDistrict>();
            this.WardsSource = new ObservableCollection<AdminisWard>();
            this.DepartmentsSource = new ObservableCollection<Department>();
            this.PositionsSource = new ObservableCollection<Position>();

            // register command
            // command
            AddNewEmployeeCommand = new AsyncCommand<Button, ProgressBar, Snackbar>((v, x, w) =>
            {
                return this._canAdd;
            }, (v, x, w) =>
            {
                this._canAdd = false;
                try
                {
                    x.Dispatcher.Invoke(new Action(() => { x.Visibility = Visibility.Visible; }));

                    FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("DepartmentCode", this._selectedDepartment.DepartmentCode)
                                                          & Builders<BsonDocument>.Filter.Eq("Positions.PositionCode", this._selectedPosition.PositionCode);
                    UpdateDefinition<BsonDocument> update = Builders<BsonDocument>.Update.AddToSet("EmployeeInfos", new EmployeeInfo
                    {
                        PositionCode = this._selectedPosition.PositionCode,
                        EmployeeCode = this._employeeCode,
                        FullName = this._fullName,
                        Email = this._email,
                        Phone = this._phone,
                        Gender = this._gender,
                        ProvinceName = this._selectedAdminisProvince.ProvinceName,
                        DistrictName = this._selectedAdminisDistrict.DistrictName,
                        WardName = this._selectedAdminisWard.WardName,
                        DetailAddress = this._detailAddress
                    }).Inc("Positions.$.Current", 1);

                    MongoCRUD crud = MongodbRequest.Instance().StartDbSession(MongoDefine.DATABASE.HR_DATA_DB);
                    crud.UpdateOne(MongoDefine.COLLECTION.HR_DEPARTMENT_COLLECTION, filter, update);

                    w.Dispatcher.Invoke(new Action(() => { w.Message.Content = "Insert successfully!"; }));
                }
                catch (MongoWriteException ex)
                {
                    w.Dispatcher.Invoke(new Action(() => { w.Message.Content = ex.Message; }));
                }

                this._canAdd = true;
                x.Dispatcher.Invoke(new Action(() => { x.Visibility = Visibility.Hidden; }));
                w.Dispatcher.Invoke(new Action(() => { w.IsActive = true; }));
            });

            // command
            LoadProvinceCommand = new AsyncCommand<ProgressBar>((p) => { return true; }, (p) =>
            {
                p.Dispatcher.Invoke(() => { p.Visibility = Visibility.Visible; });

                FilterDefinition<AdminisProvince> filter = new AdminisProvince
                {
                    AdminisType = 1
                }.ToBsonDocument();

                MongoCRUD crud = MongodbRequest.Instance().StartDbSession(MongoDefine.DATABASE.HR_DATA_DB);
                List<AdminisProvince> provinces = crud.GetMany<AdminisProvince>(MongoDefine.COLLECTION.HR_ADMINISTRATIVE_VN_COLLECTION, filter);

                foreach (AdminisProvince province in provinces)
                {
                    App.Current.Dispatcher.Invoke(() => { this.ProvincesSource.Add(province); });
                }

                p.Dispatcher.Invoke(() => { p.Visibility = Visibility.Hidden; });
            });

            // command
            LoadDepartmentCommand = new AsyncCommand<ProgressBar>((p) => { return true; }, (p) =>
            {
                p.Dispatcher.Invoke(() => { p.Visibility = Visibility.Visible; });
                FilterDefinition<BsonDocument> filter = new Department
                {

                }.ToBsonDocument();
                ProjectionDefinition<BsonDocument> projection = Builders<BsonDocument>.Projection.Include("DepartmentCode").Include("DepartmentName");

                MongoCRUD crud = MongodbRequest.Instance().StartDbSession(MongoDefine.DATABASE.HR_DATA_DB);
                List<Department> departments = crud.GetMany<Department>(MongoDefine.COLLECTION.HR_DEPARTMENT_COLLECTION, filter, projection);

                foreach (Department department in departments)
                {
                    App.Current.Dispatcher.Invoke(() => { this.DepartmentsSource.Add(department); });
                }

                p.Dispatcher.Invoke(() => { p.Visibility = Visibility.Hidden; });
            });

            // command
            HandleProvinceChangedCommand = new AsyncCommand<ProgressBar>((p) => { return true; }, (p) =>
            {
                if (this._selectedAdminisProvince == null)
                {
                    return;
                }

                p.Dispatcher.Invoke(() => { p.Visibility = Visibility.Visible; });
                App.Current.Dispatcher.Invoke(() => { this.DistrictsSource.Clear(); });

                FilterDefinition<AdminisDistrict> filter = new AdminisDistrict
                {
                    AdminisType = 2,
                    ProvinceName = this._selectedAdminisProvince.ProvinceName
                }.ToBsonDocument();

                MongoCRUD crud = MongodbRequest.Instance().StartDbSession(MongoDefine.DATABASE.HR_DATA_DB);
                List<AdminisDistrict> districts = crud.GetMany<AdminisDistrict>(MongoDefine.COLLECTION.HR_ADMINISTRATIVE_VN_COLLECTION, filter);

                foreach(AdminisDistrict district in districts)
                {
                    App.Current.Dispatcher.Invoke(() => { this.DistrictsSource.Add(district); });
                }

                p.Dispatcher.Invoke(() => { p.Visibility = Visibility.Hidden; });
            });

            // command
            HandleDistrictChangedCommand = new AsyncCommand<ProgressBar>((p) => { return true; }, (p) =>
            {
                if (this._selectedAdminisProvince == null || this._selectedAdminisDistrict == null)
                {
                    return;
                }

                p.Dispatcher.Invoke(() => { p.Visibility = Visibility.Visible; });
                App.Current.Dispatcher.Invoke(() => { this.WardsSource.Clear(); });

                FilterDefinition<AdminisWard> filter = new AdminisWard
                {
                    AdminisType = 3,
                    ProvinceName = this._selectedAdminisProvince.ProvinceName,
                    DistrictName = this._selectedAdminisDistrict.DistrictName,
                }.ToBsonDocument();

                MongoCRUD crud = MongodbRequest.Instance().StartDbSession(MongoDefine.DATABASE.HR_DATA_DB);
                List<AdminisWard> wards = crud.GetMany<AdminisWard>(MongoDefine.COLLECTION.HR_ADMINISTRATIVE_VN_COLLECTION, filter);

                foreach(AdminisWard ward in wards)
                {
                    App.Current.Dispatcher.Invoke(() => { this.WardsSource.Add(ward); });
                }

                p.Dispatcher.Invoke(() => { p.Visibility = Visibility.Hidden; });
            });

            // command
            HandleDepartmentChangedCommand = new AsyncCommand<ProgressBar>((p) => { return true; }, (p) =>
            {
                if (this._selectedDepartment == null)
                {
                    return;
                }

                p.Dispatcher.Invoke(() => { p.Visibility = Visibility.Visible; });
                App.Current.Dispatcher.Invoke(() => { this.PositionsSource.Clear(); });

                FilterDefinition<BsonDocument> filter = new Department
                {
                    DepartmentCode = this._selectedDepartment.DepartmentCode,
                }.ToBsonDocument();

                ProjectionDefinition<BsonDocument> projection = Builders<BsonDocument>.Projection.Include("Positions");
                MongoCRUD crud = MongodbRequest.Instance().StartDbSession(MongoDefine.DATABASE.HR_DATA_DB);
                List<Department> departments = crud.GetMany<Department>(MongoDefine.COLLECTION.HR_DEPARTMENT_COLLECTION, filter, projection);

                if (departments.Count > 0 && departments[0].Positions != null)
                {
                    List<Position> positions = departments[0].Positions;
                    foreach (Position position in positions)
                    {
                        App.Current.Dispatcher.Invoke(() => { this.PositionsSource.Add(position); });
                    }
                }

                p.Dispatcher.Invoke(() => { p.Visibility = Visibility.Hidden; });
            });
        }

        // Validate propertyOnChanged

        public String Error { get { return null; } }

        public String this[String name]
        {
            get
            {
                String result = "";
                switch (name)
                {
                    case "MFullName":
                        if (MFullName == "")
                        {
                            result = "Tên bắt buộc nhập!";
                            goto fail;
                        }
                        else
                        {
                            goto success;
                        }
                    case "MEmail":
                        if (MEmail == "")
                        {
                            result = "Email bắt buộc nhập!";
                            goto fail;
                        }
                        else
                        {
                            bool ok = this.IsValidEmail();
                            if (ok == false)
                            {
                                result = "Email is not valid!";
                                goto fail;
                            }
                            else
                            {
                                goto success;
                            }
                        }
                    case "MPhone":
                        if (MPhone == "")
                        {
                            result = "Phone is required!";
                            goto fail;
                        }
                        else
                        {
                            bool ok = this.IsPhoneNumber();
                            if (ok == false)
                            {
                                result = "Phone is not valid!";
                                goto fail;
                            }
                            else
                            {
                                goto success;
                            }
                        }
                    case "MDetailAddress":
                        if (MDetailAddress == "")
                        {
                            result = "Detail addess is required!";
                            goto fail;
                        }
                        else
                        {
                            goto success;
                        }
                    default:
                        goto success;
                }
            fail:
                _canAdd = false;
                goto exit;
            success:
                _canAdd = true;
                goto exit;
            exit:
                return result;
            }
        }

        private bool IsValidEmail()
        {
            if (MEmail != null && MEmail.EndsWith("."))
            {
                return false;
            }
            else if (MEmail == null)
            {
                return true;
            }

            try
            {
                var validateEmail = new System.Net.Mail.MailAddress(MEmail);
                return validateEmail.Address == MEmail;
            }
            catch
            {
                return false;
            }
        }

        private bool IsPhoneNumber()
        {
            return Regex.Match(MPhone, @"^(\+[0-9]{9})$").Success;
        }
    }
}
