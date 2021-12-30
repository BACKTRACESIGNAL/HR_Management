using HR_Management.HR_Libs;
using HR_Management.Model;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HR_Management.ViewModel.HR_Window
{
    public class LoginViewModel : BaseViewModel, IDataErrorInfo
    {
        private bool _canLogin { get; set; } = false;

        private string _accountName { get; set; }
        private string _password { get; set; }

        public bool LoginSuccess { get; set; } = false;

        public String MAccountName { get => this._accountName; set { _ = this.IsAccountNameValid(value); this._accountName = value; OnPropertyChanged(); } }
        public String MPassward { get => this._password; set { _ = IsPasswordValid(value); this._password = value; OnPropertyChanged(); } }

        public ICommand HandlePasswordChangeCommand { get; set; }
        public ICommand HandleLoginCommand { get; set; }

        public LoginViewModel()
        {
            HandlePasswordChangeCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) =>
            {
                this._password = p.Password;
            });

            HandleLoginCommand = new AsyncCommand<Window>((p) => { return this._canLogin; }, (p) =>
            {
                FilterDefinition<Account> filter = new BsonDocument
                {
                    {"AccountName", this._accountName},
                };

                MongoCRUD crud = MongodbRequest.Instance().StartDbSession(MongoDefine.DATABASE.HR_DATA_DB);
                List<Account> accounts = crud.GetMany<Account>(MongoDefine.COLLECTION.HR_ACCOUNT_COLLECTION, filter);
                if (accounts.Count != 1)
                {
                    MessageBox.Show("Wrong account!");
                    return;
                }

                bool ok = PasswordHasher.VerifyPassword(this._password, accounts[0].Password);
                if (ok == false)
                {
                    MessageBox.Show("Wrong password!");
                    return;
                }

                this._canLogin = false;
                this.LoginSuccess = true;

                Utility.GLOBAL_VARIABLE.ACCOUNT_CACHED = accounts[0];
                p.Dispatcher.Invoke(() => { p.Close(); });
            });
        }

        private bool IsAccountNameValid(String value)
        {
            bool isValid = true;

            // Email format
            if (value.EndsWith("."))
            {
                this.AddError("MAccountName", INVALID_EMAIL_FORMAT_ERROR);
                isValid = false;
            }
            else
            {
                try
                {
                    var validateEmail = new System.Net.Mail.MailAddress(value);
                    if (validateEmail.Address != value)
                    {
                        this.AddError("MAccountName", INVALID_EMAIL_FORMAT_ERROR);
                        isValid = false;
                    }
                    else
                    {
                        this.RemoveError("MAccountName", INVALID_EMAIL_FORMAT_ERROR);
                    }
                }
                catch
                {
                    this.AddError("MAccountName", INVALID_EMAIL_FORMAT_ERROR);
                    isValid = false;
                }
            }

            return isValid;
        }

        private bool IsPasswordValid(string value)
        {
            bool isValid = true;

            if (value.Length < 8)
            {
                this.AddError("MPassword", INVALID_LENGTH_ERROR + 8);
                isValid = false;
            }
            else
            {
                this.RemoveError("MPassword", INVALID_LENGTH_ERROR + 8);
            }

            return isValid;
        }

        private Dictionary<string, List<string>> errors = new Dictionary<string, List<string>>();

        private const string INVALID_EMAIL_FORMAT_ERROR = "Email format is invalid";
        private const string INVALID_LENGTH_ERROR = "Length must be greater than ";

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
                    this._canLogin = true;
                    return null;
                }
                else
                {
                    this._canLogin = false;
                    return String.Join(Environment.NewLine, errors[propertyName]);
                }
            }
        }
    }
}
