using HR_Management.ViewModel.HR_UserControl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HR_Management.View.HR_UserControl.Models
{
    /// <summary>
    /// Interaction logic for EmployeeForm.xaml
    /// </summary>
    public partial class EmployeeForm : UserControl
    {
        private EmployeeFormViewModel _employeeForm { get; set; }
        public EmployeeForm()
        {
            InitializeComponent();
            this.DataContext = _employeeForm = new EmployeeFormViewModel();
        }
    }
}
