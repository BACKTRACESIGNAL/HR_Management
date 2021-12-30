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
    public class EmployeeEditFormViewModel : BaseViewModel, IDataErrorInfo
    {
        private bool _canAdd = false;

        private String _employeeCode { get; set; }
        private String _fullName { get; set; }
        private String _email { get; set; }
        private String _phone { get; set; }
        private long _gender { get; set; }
        private String _detailAddress { get; set; }

        private AdminisProvince _selectedAdminisProvince { get; set; }
        private AdminisDistrict _selectedAdminisDistrict { get; set; }
        private AdminisWard _selectedAdminisWard { get; set; }
        private Department _selectedDepartment { get; set; }
        private Position _selectedPosition { get; set; }

        public String MEmployeeCode { get => this._employeeCode; set { _ = this.IsEmployeeCodeValid(value); this._employeeCode = value; OnPropertyChanged(); } }
        public String MFullName { get => this._fullName; set { _ = this.IsFullNameValid(value); this._fullName = value; OnPropertyChanged(); } }
        public String MEmail { get => this._email; set { _ = this.IsEmailValid(value); this._email = value; OnPropertyChanged(); } }
        public String MPhone { get => this._phone; set { _ = this.IsPhoneValid(value); this._phone = value; OnPropertyChanged(); } }
        public long MGender { get => this._gender; set { _ = this.IsGenderValid(value); this._gender = value; OnPropertyChanged(); } }
        public String MDetailAddress { get => this._detailAddress; set { _ = this.IsDetailAddressValid(value); this._detailAddress = value; OnPropertyChanged(); } }

        public AdminisProvince SelectedAdminisProvince { get => this._selectedAdminisProvince; set { _ = this.IsSelectedAdminisProvinceValid(value); this._selectedAdminisProvince = value; OnPropertyChanged(); } }
        public AdminisDistrict SelectedAdminisDistrict { get => this._selectedAdminisDistrict; set { _ = this.IsSelectedAdminisDistrictValid(value); this._selectedAdminisDistrict = value; OnPropertyChanged(); } }
        public AdminisWard SelectedAdminisWard { get => this._selectedAdminisWard; set { _ = this.IsSelectedAdminisWardValid(value); this._selectedAdminisWard = value; OnPropertyChanged(); } }
        public Department SelectedDepartment { get => this._selectedDepartment; set { _ = this.IsSelectedDepartmentValid(value); this._selectedDepartment = value; OnPropertyChanged(); } }
        public Position SelectedPosition { get => this._selectedPosition; set { _ = this.IsSelectedPositionValid(value); this._selectedPosition = value; OnPropertyChanged(); } }

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

        public EmployeeEditFormViewModel(EmployeeInfo employeeInfo)
        {
            // intitial data
            this.ProvincesSource = new ObservableCollection<AdminisProvince>();
            this.DistrictsSource = new ObservableCollection<AdminisDistrict>();
            this.WardsSource = new ObservableCollection<AdminisWard>();
            this.DepartmentsSource = new ObservableCollection<Department>();
            this.PositionsSource = new ObservableCollection<Position>();

            this._employeeCode = employeeInfo.EmployeeCode;
            this._fullName = employeeInfo.FullName;
            this._email = employeeInfo.Email;
            this._phone = employeeInfo.Phone;
            this._gender = (long)employeeInfo.Gender;
            this._detailAddress = employeeInfo.DetailAddress;

            // register command
            // command
            AddNewEmployeeCommand = new AsyncCommand<Button, ProgressBar, Snackbar>((v, x, w) =>
            {
                return this._canAdd;
            }, (v, x, w) =>
            {
                if (v == null || x == null || w == null)
                {
                    return;
                }

                this._canAdd = false;

                if (this.IsFillAllRequired() == false)
                {
                    w.Dispatcher.Invoke(new Action(() => { w.Message.Content = "Required data is emptied!"; }));
                }
                else
                {
                    try
                    {
                        x.Dispatcher.Invoke(new Action(() => { x.Visibility = Visibility.Visible; }));

                        EmployeeInfo newEmployeeInfo = new EmployeeInfo
                        {
                            PositionCode = this._selectedPosition.PositionCode,
                            Position = new Position
                            {
                                PositionCode = this._selectedPosition.PositionCode,
                                PositionName = this._selectedPosition.PositionName,
                            },
                            EmployeeCode = this._employeeCode,
                            FullName = this._fullName,
                            Email = this._email,
                            Phone = this._phone,
                            Gender = this._gender,
                            ProvinceName = this._selectedAdminisProvince.ProvinceName,
                            Province = new AdminisProvince
                            {
                                ProvinceName = this._selectedAdminisProvince.ProvinceName,
                                AdminisType = this._selectedAdminisProvince.AdminisType
                            },
                            DistrictName = this._selectedAdminisDistrict.DistrictName,
                            District = new AdminisDistrict
                            {
                                AdminisType = this._selectedAdminisDistrict.AdminisType,
                                ProvinceName = this._selectedAdminisDistrict.ProvinceName,
                                DistrictName = this._selectedAdminisDistrict.DistrictName
                            },
                            WardName = this._selectedAdminisWard.WardName,
                            Ward = new AdminisWard
                            {
                                AdminisType = this._selectedAdminisWard.AdminisType,
                                ProvinceName = this._selectedAdminisWard.ProvinceName,
                                DistrictName = this._selectedAdminisWard.DistrictName,
                                WardName = this._selectedAdminisWard.WardName
                            },
                            DetailAddress = this._detailAddress,
                            Department = new Department
                            {
                                DepartmentCode = this._selectedDepartment.DepartmentCode,
                                DepartmentName = this._selectedDepartment.DepartmentName,
                            }
                        };

                        // -------------------------------------------------------- //
                        // @TODO: Change permission when transfer employee
                        // -------------------------------------------------------- //

                        bool changePosition = this._selectedPosition.PositionCode == employeeInfo.PositionCode ? false : true;
                        bool changeDepartment = this._selectedDepartment.DepartmentCode == employeeInfo.Department.DepartmentCode ? false : true;

                        FilterDefinition<BsonDocument> essentialFilter;
                        UpdateDefinition<BsonDocument> essentialUpdate;
                        List<ArrayFilterDefinition> essentialArrayFilter;
                        MongoCRUD crud = MongodbRequest.Instance().StartDbSession(MongoDefine.DATABASE.HR_DATA_DB);

                        if (changeDepartment == false && changePosition == true)
                        {
                            // ---------------------------------------------------- //

                            essentialFilter = new BsonDocument
                            {
                                {"DepartmentCode", this._selectedDepartment.DepartmentCode }
                            };

                            essentialUpdate = Builders<BsonDocument>.Update.Inc("Positions.$[u].Current", 1).Inc("Positions.$[t].Current", -1).Set("EmployeeInfos.$[v]", newEmployeeInfo.ToBsonDocument());

                            essentialArrayFilter = new List<ArrayFilterDefinition>
                            {
                                new BsonDocumentArrayFilterDefinition<BsonDocument>(new BsonDocument
                                {
                                    {"u.PositionCode", this._selectedPosition.PositionCode }
                                }),
                                new BsonDocumentArrayFilterDefinition<BsonDocument>(new BsonDocument
                                {
                                    {"t.PositionCode", employeeInfo.Position.PositionCode }
                                }),
                                new BsonDocumentArrayFilterDefinition<BsonDocument>(new BsonDocument
                                {
                                    {"v.Email", this._email }
                                })
                            };

                            crud.UpdateOne(MongoDefine.COLLECTION.HR_DEPARTMENT_COLLECTION, essentialFilter, essentialUpdate, essentialArrayFilter);
                        }
                        else if (changeDepartment == true)
                        {
                            // ---------------------------------------------------- //

                            List<WriteModel<BsonDocument>> listWrite = new List<WriteModel<BsonDocument>>();
                            essentialFilter = new BsonDocument
                            {
                                {"DepartmentCode", employeeInfo.Department.DepartmentCode }
                            };

                            essentialUpdate = Builders<BsonDocument>.Update.Inc("Positions.$[u].Current", -1).Pull("EmployeeInfos", new BsonDocument
                            {
                                {"Email", employeeInfo.Email}
                            });

                            essentialArrayFilter = new List<ArrayFilterDefinition>
                            {
                                new BsonDocumentArrayFilterDefinition<BsonDocument>(new BsonDocument
                                {
                                    {"u.PositionCode", employeeInfo.Position.PositionCode}
                                })
                            };

                            UpdateOneModel<BsonDocument> updateOne = new UpdateOneModel<BsonDocument>(essentialFilter, essentialUpdate) { ArrayFilters = essentialArrayFilter };

                            listWrite.Add(updateOne);

                            // ---------------------------------------------------- //

                            essentialFilter = new BsonDocument
                            {
                                {"DepartmentCode", this._selectedDepartment.DepartmentCode }
                            };

                            essentialUpdate = Builders<BsonDocument>.Update.Inc("Positions.$[u].Current", 1).AddToSet("EmployeeInfos", newEmployeeInfo.ToBsonDocument());
                            essentialArrayFilter = new List<ArrayFilterDefinition>
                            {
                                new BsonDocumentArrayFilterDefinition<BsonDocument>(new BsonDocument
                                {
                                    {"u.PositionCode", this._selectedPosition.PositionCode }
                                })
                            };

                            updateOne = new UpdateOneModel<BsonDocument>(essentialFilter, essentialUpdate) { ArrayFilters = essentialArrayFilter };
                            listWrite.Add(updateOne);

                            crud.BulkWrite(MongoDefine.COLLECTION.HR_DEPARTMENT_COLLECTION, listWrite);
                        }
                        else
                        {
                            // ---------------------------------------------------- //
                            MessageBox.Show(this._email);
                            essentialFilter = Builders<BsonDocument>.Filter.Eq("DepartmentCode", this._selectedDepartment.DepartmentCode)
                                                & Builders<BsonDocument>.Filter.Eq("EmployeeInfos.Email", this._email);

                            essentialUpdate = Builders<BsonDocument>.Update.Set("EmployeeInfos.$", newEmployeeInfo.ToBsonDocument());
                            crud.UpdateOne(MongoDefine.COLLECTION.HR_DEPARTMENT_COLLECTION, essentialFilter, essentialUpdate);
                        }

                        //App.Current.Dispatcher.Invoke(() => { PlayYard.Instance().GetEmployeeViewModel().EmployeeSourceData.Insert(0, employeeInfo); });
                        w.Dispatcher.Invoke(new Action(() => { w.Message.Content = "Updated successfully!"; }));
                    }
                    catch (MongoWriteException ex)
                    {
                        w.Dispatcher.Invoke(new Action(() => { w.Message.Content = ex.Message; }));
                    }
                }

                this._canAdd = true;
                x.Dispatcher.Invoke(new Action(() => { x.Visibility = Visibility.Hidden; }));
                w.Dispatcher.Invoke(new Action(() => { w.IsActive = true; }));
            });

            // command
            LoadProvinceCommand = new AsyncCommand<ProgressBar, ComboBox, ComboBox, ComboBox>((p, v, x, t) => { return true; }, (p, v, x, t) =>
            {
                p.Dispatcher.Invoke(() => { p.Visibility = Visibility.Visible; });

                FilterDefinition<AdminisProvince> filter = new AdminisProvince
                {
                    AdminisType = 1
                }.ToBsonDocument();

                MongoCRUD crud = MongodbRequest.Instance().StartDbSession(MongoDefine.DATABASE.HR_DATA_DB);
                List<AdminisProvince> provinces = crud.GetMany<AdminisProvince>(MongoDefine.COLLECTION.HR_ADMINISTRATIVE_VN_COLLECTION, filter);

                bool loadDistrict = false;
                bool loadWard = false;
                foreach (AdminisProvince province in provinces)
                {
                    if (province.ProvinceName == employeeInfo.ProvinceName)
                    {
                        loadDistrict = true;
                        v.Dispatcher.Invoke(() => { v.SelectedItem = province; });
                    }
                    App.Current.Dispatcher.Invoke(() => { this.ProvincesSource.Add(province); });
                }

                if (loadDistrict == true)
                {
                    loadWard = HandleLoadDistrict(employeeInfo.District.DistrictName, x);
                }

                if (loadWard == true)
                {
                    _ = HandleLoadWard(employeeInfo.Ward.WardName, t);
                }

                p.Dispatcher.Invoke(() => { p.Visibility = Visibility.Hidden; });
            });

            // command
            LoadDepartmentCommand = new AsyncCommand<ProgressBar, ComboBox, ComboBox>((p, v, t) => { return true; }, (p, v, t) =>
            {
                p.Dispatcher.Invoke(() => { p.Visibility = Visibility.Visible; });
                FilterDefinition<BsonDocument> filter = new Department
                {

                }.ToBsonDocument();
                ProjectionDefinition<BsonDocument> projection = Builders<BsonDocument>.Projection.Include("DepartmentCode").Include("DepartmentName");

                MongoCRUD crud = MongodbRequest.Instance().StartDbSession(MongoDefine.DATABASE.HR_DATA_DB);
                List<Department> departments = crud.GetMany<Department>(MongoDefine.COLLECTION.HR_DEPARTMENT_COLLECTION, filter, projection);

                bool loadPosition = false;
                foreach (Department department in departments)
                {
                    if (employeeInfo.Department.DepartmentCode == department.DepartmentCode)
                    {
                        v.Dispatcher.Invoke(() => { v.SelectedItem = department; });
                        loadPosition = true;
                    }
                    App.Current.Dispatcher.Invoke(() => { this.DepartmentsSource.Add(department); });
                }

                if (loadPosition == true)
                {
                    _= HandleLoadPosition(employeeInfo.Position.PositionCode, t);
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

                _ = this.HandleLoadDistrict();

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

                _ = HandleLoadWard();

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
                _ = HandleLoadPosition();

                p.Dispatcher.Invoke(() => { p.Visibility = Visibility.Hidden; });
            });
        }

        private bool HandleLoadDistrict(String goodDistrict = null, ComboBox districtCombobox = null)
        {
            bool loadWard = false;
            App.Current.Dispatcher.Invoke(() =>
            {
                this.DistrictsSource.Clear();
                this.WardsSource.Clear();
            });

            FilterDefinition<AdminisDistrict> filter = new AdminisDistrict
            {
                AdminisType = 2,
                ProvinceName = this._selectedAdminisProvince.ProvinceName
            }.ToBsonDocument();

            MongoCRUD crud = MongodbRequest.Instance().StartDbSession(MongoDefine.DATABASE.HR_DATA_DB);
            List<AdminisDistrict> districts = crud.GetMany<AdminisDistrict>(MongoDefine.COLLECTION.HR_ADMINISTRATIVE_VN_COLLECTION, filter);

            foreach (AdminisDistrict district in districts)
            {
                if (goodDistrict != null && districtCombobox != null)
                {
                    if (goodDistrict == district.DistrictName)
                    {
                        loadWard = true;
                        districtCombobox.Dispatcher.Invoke(() => { districtCombobox.SelectedItem = district; });
                    }
                }
                App.Current.Dispatcher.Invoke(() => { this.DistrictsSource.Add(district); });
            }

            return loadWard;
        }

        private bool HandleLoadWard(String goodWard = null, ComboBox wardCombobox = null)
        {
            bool ok = false;

            App.Current.Dispatcher.Invoke(() => { this.WardsSource.Clear(); });

            FilterDefinition<AdminisWard> filter = new AdminisWard
            {
                AdminisType = 3,
                ProvinceName = this._selectedAdminisProvince.ProvinceName,
                DistrictName = this._selectedAdminisDistrict.DistrictName,
            }.ToBsonDocument();

            MongoCRUD crud = MongodbRequest.Instance().StartDbSession(MongoDefine.DATABASE.HR_DATA_DB);
            List<AdminisWard> wards = crud.GetMany<AdminisWard>(MongoDefine.COLLECTION.HR_ADMINISTRATIVE_VN_COLLECTION, filter);

            foreach (AdminisWard ward in wards)
            {
                if (goodWard != null && wardCombobox != null)
                {
                    if (goodWard == ward.WardName)
                    {
                        ok = true;
                        wardCombobox.Dispatcher.Invoke(() => { wardCombobox.SelectedItem = ward; });
                    }
                }

                App.Current.Dispatcher.Invoke(() => { this.WardsSource.Add(ward); });
            }

            return ok;
        }

        private bool HandleLoadPosition(String goodPosition = null, ComboBox comboboxPosition = null)
        {
            bool ok = false;

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
                    if (goodPosition != null && comboboxPosition != null)
                    {
                        if (goodPosition == position.PositionCode)
                        {
                            comboboxPosition.Dispatcher.Invoke(() => { comboboxPosition.SelectedItem = position; });
                            ok = true;
                        }
                    }
                    App.Current.Dispatcher.Invoke(() => { this.PositionsSource.Add(position); });
                }
            }
            else
            {

            }

            return ok;
        }

        // Validate data
        private bool IsFillAllRequired()
        {
            bool isValid = true;

            if (String.IsNullOrEmpty(this._employeeCode) || String.IsNullOrEmpty(this._fullName) || String.IsNullOrEmpty(this._email)
                || String.IsNullOrEmpty(this._phone) || this._selectedAdminisProvince == null || this._selectedAdminisDistrict == null
                || this._selectedAdminisWard == null || this._selectedDepartment == null || this._selectedPosition == null)
            {
                isValid = false;
            }

            return isValid;
        }

        private bool IsEmployeeCodeValid(string value)
        {
            bool isValid = true;
            int minLength = 5;

            // EmployeeCode length
            if (value.Length < minLength)
            {
                this.AddError("MEmployeeCode", LOW_LENGTH_ERROR + minLength);
                isValid = false;
            }
            else
            {
                this.RemoveError("MEmployeeCode", LOW_LENGTH_ERROR + minLength);
            }

            return isValid;
        }

        private bool IsFullNameValid(string value)
        {
            bool isValid = true;
            int minLength = 5;

            // FullName length
            if (value.Length < minLength)
            {
                this.AddError("MFullName", LOW_LENGTH_ERROR + minLength);
                isValid = false;
            }
            else
            {
                this.RemoveError("MFullName", LOW_LENGTH_ERROR + minLength);
            }

            return isValid;
        }

        private bool IsEmailValid(string value)
        {
            bool isValid = true;

            // Email format
            if (value.EndsWith("."))
            {
                this.AddError("MEmail", INVALID_EMAIL_FORMAT_ERROR);
                isValid = false;
            }
            else
            {
                try
                {
                    var validateEmail = new System.Net.Mail.MailAddress(value);
                    if (validateEmail.Address != value)
                    {
                        this.AddError("MEmail", INVALID_EMAIL_FORMAT_ERROR);
                        isValid = false;
                    }
                    else
                    {
                        this.RemoveError("MEmail", INVALID_EMAIL_FORMAT_ERROR);
                    }
                }
                catch
                {
                    this.AddError("MEmail", INVALID_EMAIL_FORMAT_ERROR);
                }
            }

            return isValid;
        }

        private bool IsPhoneValid(string value)
        {
            bool isValid = true;

            // Phone format
            if (!Regex.Match(value, @"^(\b0[0-9]{9}([0-9]{2})?)$").Success)
            {
                this.AddError("MPhone", PHONE_NUMBER_FORMAT_ERROR);
                isValid = false;
            }
            else
            {
                this.RemoveError("MPhone", PHONE_NUMBER_FORMAT_ERROR);
            }

            return isValid;
        }

        private bool IsGenderValid(long value)
        {
            bool isValid = true;

            // No handle

            return isValid;
        }

        private bool IsDetailAddressValid(string value)
        {
            bool isValid = true;

            if (value.Length == 0)
            {
                this.AddError("MDetailAddress", EMPTY_ERROR);
                isValid = false;
            }
            else
            {
                this.RemoveError("MDetailAddress", EMPTY_ERROR);
            }

            return isValid;
        }

        private bool IsSelectedAdminisProvinceValid(AdminisProvince value)
        {
            bool isValid = true;

            // Empty options
            if (value == null)
            {
                this.AddError("SelectedAdminisProvince", EMPTY_ERROR);
                isValid = false;
            }
            else
            {
                this.RemoveError("SelectedAdminisProvince", EMPTY_ERROR);
            }

            return isValid;
        }

        private bool IsSelectedAdminisDistrictValid(AdminisDistrict value)
        {
            bool isValid = true;

            // Empty options
            if (value == null)
            {
                this.AddError("SelectedAdminisDistrict", EMPTY_ERROR);
                isValid = false;
            }
            else
            {
                this.RemoveError("SelectedAdminisDistrict", EMPTY_ERROR);
            }

            return isValid;
        }

        private bool IsSelectedAdminisWardValid(AdminisWard value)
        {
            bool isValid = true;

            // Empty options
            if (value == null)
            {
                this.AddError("SelectedAdminisWard", EMPTY_ERROR);
                isValid = false;
            }
            else
            {
                this.RemoveError("SelectedAdminisWard", EMPTY_ERROR);
            }

            return isValid;
        }

        private bool IsSelectedDepartmentValid(Department value)
        {
            bool isValid = true;

            // Empty options
            if (value == null)
            {
                this.AddError("SelectedDepartment", EMPTY_ERROR);
                isValid = false;
            }
            else
            {
                this.RemoveError("SelectedDepartment", EMPTY_ERROR);
            }

            return isValid;
        }

        private bool IsSelectedPositionValid(Position value)
        {
            bool isValid = true;

            // Empty options
            if (value == null)
            {
                this.AddError("SelectedPosition", NO_ROOM_ERROR);
                isValid = false;
            }
            else
            {
                this.RemoveError("SelectedPosition", NO_ROOM_ERROR);
            }

            return isValid;
        }

        private Dictionary<string, List<string>> errors = new Dictionary<string, List<string>>();

        private const string LOW_LENGTH_ERROR = "Value length should not be less than ";
        private const string EMPTY_ERROR = "Value length should not be empty";
        private const string INVALID_EMAIL_FORMAT_ERROR = "Email format is invalid";
        private const string PHONE_NUMBER_FORMAT_ERROR = "Phone number is invalid";
        private const string NO_ROOM_ERROR = "There's no room for this department option, please select another one!";

        public void AddError(string propertyName, string error)
        {
            if (!errors.ContainsKey(propertyName))
            {
                errors[propertyName] = new List<string>();
            }

            if (!errors[propertyName].Contains(error))
            {
                errors[propertyName].Insert(0, error);
            }
        }

        public void RemoveError(string propertyName, string error)
        {
            if (errors.ContainsKey(propertyName) && errors[propertyName].Contains(error))
            {
                errors[propertyName].Remove(error);
                if (errors[propertyName].Count == 0)
                {
                    errors.Remove(propertyName);
                }
            }
        }

        public String Error { get { throw new NotImplementedException(); } }

        public String this[String propertyName]
        {
            get
            {
                if (!errors.ContainsKey(propertyName))
                {
                    this._canAdd = true;
                    return null;
                }
                else
                {
                    this._canAdd = false;
                    return String.Join(Environment.NewLine, errors[propertyName]);
                }
            }
        }
    }
}
