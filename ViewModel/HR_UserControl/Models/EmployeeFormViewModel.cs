using HR_Management.HR_Libs;
using HR_Management.Model;
using MaterialDesignThemes.Wpf;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HR_Management.ViewModel.HR_UserControl.Models
{
    public class EmployeeFormViewModel : BaseViewModel, IDataErrorInfo
    {
        private bool _canAdd = true;

        private ProgressBar p_progressBar;

        private String p_fullName { get; set; }
        private String p_email { get; set; }
        private String p_phone { get; set; }
        private Boolean p_gender { get; set; }
        private String p_province { get; set; }
        private String p_district { get; set; }
        private String p_detailAddress { get; set; }
        private String p_department { get; set; }
        private String p_position { get; set; }
        private String p_status { get; set; }

        public ObservableCollection<Administrative> ProvincesSource;
        public ObservableCollection<Administrative> DistrictsSource;
        public ObservableCollection<Administrative> WardsSource;

        // @TODO: Validate data stuff
        // https://docs.microsoft.com/en-us/previous-versions/windows/apps/743swcz7(v=vs.105)
        // 
        public String MFullName { get => p_fullName; set { p_fullName = value; OnPropertyChanged(); } }
        public String MEmail { get => p_email; set { p_email = value; OnPropertyChanged(); } }
        public String MPhone { get => p_phone; set { p_phone = value; OnPropertyChanged(); } }
        public Boolean MGender { get => p_gender; set { p_gender = value; OnPropertyChanged(); } }
        public String MProvince { get => p_province; set { p_province = value; OnPropertyChanged(); } }
        public String MDistrict { get => p_district; set { p_district = value; OnPropertyChanged(); } }
        public String MDetailAddress { get => p_detailAddress; set { p_detailAddress = value; OnPropertyChanged(); } }
        public String MDepartment { get => p_department; set { p_department = value; OnPropertyChanged(); } }
        public String MPosition { get => p_position; set { p_position = value; OnPropertyChanged(); } }
        public String MStatus { get => p_status; set { p_status = value; OnPropertyChanged(); } }

        public ICommand AddNewEmployeeCommand { get; set; }

        public EmployeeFormViewModel()
        {
            // register command
            AddNewEmployeeCommand = new AsyncQuadraParamCommand<Button, Button, ProgressBar, Snackbar>((p, v, x, w) =>
            {
                return this._canAdd;
            },
            (p, v, x, w) =>
            {
                this._canAdd = false;
                try
                {
                    x.Dispatcher.Invoke(new Action(() => { x.Visibility = Visibility.Visible; }));
                    String data_json = JsonConvert.SerializeObject(new EmployeeInfoModel
                    {
                        FullName = this.p_fullName,
                        Email = this.p_email,
                        Phone = this.p_phone,
                        Gender = this.p_gender,
                        Province = this.p_province,
                        District = this.p_district,
                        DetailAddress = this.p_detailAddress,
                        Department = this.p_department,
                        Position = this.p_position,
                        Status = this.p_status
                    });

                    MongoCRUD crud = MongodbRequest.Instance().StartDbSession(MongoDefine.DATABASE.HR_DATA_DB);
                    crud.InsertOne(MongoDefine.COLLECTION.HR_EMPOYEE_INFO_COLLECTION, data_json);
                    w.Dispatcher.Invoke(new Action(() => { w.Message.Content = "Insert successfully!"; }));
                }
                catch (MongoWriteException ex)
                {
                    w.Dispatcher.Invoke(new Action(() => { w.Message.Content = ex.Message; }));
                }

                this._canAdd = true;
                x.Dispatcher.Invoke(new Action(() => { x.Visibility = Visibility.Hidden; }));
                w.Dispatcher.Invoke(new Action(() => { w.IsActive = true; }));
                return true;
            });

            // load data async
            this.ProvincesSource = new ObservableCollection<Administrative>();
            this.DistrictsSource = new ObservableCollection<Administrative>();
            this.WardsSource     = new ObservableCollection<Administrative>();
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

        // Load administrative async
        private async Task LoadAdministrativeAsync()
        {
            await Task.Run(() => { this.LoadAdministrative(); });
        }

        private void LoadAdministrative()
        {
            this.p_progressBar.Dispatcher.Invoke(new Action(() => { this.p_progressBar.Visibility = Visibility.Visible; }));
            Administrative filter = new Administrative() { administrative_type = 1 };
            String data_json = JsonConvert.SerializeObject(filter);


            MongoCRUD crud = MongodbRequest.Instance().StartDbSession(MongoDefine.DATABASE.HR_DATA_DB);
            List<Administrative> provinces = crud.GetMany<Administrative>(MongoDefine.COLLECTION.HR_ADMINISTRATIVE_VN_COLLECTION, data_json);

            foreach(Administrative province in provinces)
            {
                App.Current.Dispatcher.Invoke(new Action(() => { this.ProvincesSource.Add(province); }));
            }

            this.p_progressBar.Dispatcher.Invoke(new Action(() => { this.p_progressBar.Visibility = Visibility.Hidden; }));
        }
    }
}
