using HR_Management.Model;
using HR_Management.ViewModel.HR_UserControl.Models;
using System.Windows.Controls;

namespace HR_Management.View.HR_UserControl.Models
{
    /// <summary>
    /// Interaction logic for EmployeeEditForm.xaml
    /// </summary>
    public partial class EmployeeEditForm : UserControl
    {
        private EmployeeEditFormViewModel _viewModel { get; set; }
        public EmployeeEditForm(EmployeeInfo employeeInfo)
        {
            InitializeComponent();
            this.DataContext = _viewModel = new EmployeeEditFormViewModel(employeeInfo);
        }
    }
}
