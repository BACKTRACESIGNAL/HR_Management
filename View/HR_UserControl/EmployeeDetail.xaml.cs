using HR_Management.Model;
using HR_Management.ViewModel.HR_UserControl;
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

namespace HR_Management.View.HR_UserControl
{
    /// <summary>
    /// Interaction logic for EmployeeDetail.xaml
    /// </summary>
    public partial class EmployeeDetail : UserControl
    {
        public EmployeeDetailViewModel viewModel { get; set; }
        public EmployeeDetail(EmployeeInfo employeeInfo)
        {
            InitializeComponent();
            this.DataContext = viewModel = new EmployeeDetailViewModel(employeeInfo);
        }
    }
}
