using payerClient.vievmodel;
using payerClient.viewmodel;
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

namespace payerClient
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private PayViewModel viewModelPay;
        private GetStatusViewModel viewModelGetStatus;
        private RefundViewModel viewModelRefund;
        public MainWindow()
        {
            InitializeComponent();
            viewModelPay = new PayViewModel();
            viewModelGetStatus = new GetStatusViewModel();
            viewModelRefund = new RefundViewModel();
            this.grdPay.DataContext = viewModelPay;
            this.grdGetStatus.DataContext = viewModelPay;
            this.grdRefund.DataContext = viewModelRefund;
        }
    }
}
