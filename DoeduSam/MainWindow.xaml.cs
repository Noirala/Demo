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

namespace DoeduSam
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Products> _MyProducts;

        public List<Products> MyProducts
        {
            get
            {
                return _MyProducts;
            }
            set
            {
                _MyProducts = value;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            MyProducts = Core.DB.Products.ToList();
        }

        private void CreateProductButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
