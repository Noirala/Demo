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
using System.ComponentModel;

namespace DoeduSam
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private List<Manufacturers> _ManufacturersList;
        public List<Manufacturers> ManufacturersList { get; set; }

        private int _ManufacturerFilterId = 0;
        public int ManufacturerFilterId
        {
            get { return _ManufacturerFilterId; }
            set
            {
                _ManufacturerFilterId = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("MyProducts"));
                    PropertyChanged(this, new PropertyChangedEventArgs("FilteredProductsCount"));
                }
            }
        }

        private string _ProductNameFilter = "";
        public string ProductNameFilter
        {
            get
            {
                return _ProductNameFilter;
            }
            set
            {
                _ProductNameFilter = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("MyProducts"));
                    PropertyChanged(this, new PropertyChangedEventArgs("FilteredProductsCount"));
                }
            }
        }

        private List<Products> _MyProducts;

        public List<Products> MyProducts
        {
            get
            {
                return _MyProducts.FindAll(item =>
                (ManufacturerFilterId == 0 || item.ManufacturerId==ManufacturerFilterId) && 
                (ProductNameFilter=="" || item.Name.IndexOf(ProductNameFilter, StringComparison.OrdinalIgnoreCase)!=-1));
            }
            set
            {
                _MyProducts = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("MyProducts"));
                    PropertyChanged(this, new PropertyChangedEventArgs("ProductsCount"));
                    PropertyChanged(this, new PropertyChangedEventArgs("FilteredProductsCount"));
                }
            }
        }

        public int FilteredProductsCount
        {
            get
            {
                return MyProducts.Count;
            }
        }

        public int ProductsCount
        {
            get
            {
                return _MyProducts.Count;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            ManufacturersList = Core.DB.Manufacturers.ToList();
            var newManufacturer = new Manufacturers();
            newManufacturer.Name = "Все производители";
            ManufacturersList.Insert(0, newManufacturer);
            MyProducts = Core.DB.Products.ToList();
        }

        private void CreateProductButton_Click(object sender, RoutedEventArgs e)
        {
            var NewProduct = new ProductWindow(new Products());

            if ((bool)NewProduct.ShowDialog())
            {
                MyProducts = Core.DB.Products.ToList();
                UpdateValues();
            }
        }

        private void UpdateValues()
        {
            PropertyChanged(this, new PropertyChangedEventArgs("MyProducts"));
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var item = MainDataGrid.SelectedItem as Products;

            var DeletedProduct = Core.DB.Products.Find(item.Id);

            if (DeletedProduct != null)
                try
                {
                    if (DeletedProduct.ProductsSales.Count > 0)
                    {
                        MessageBox.Show("Нельзя удалять товар, есть продажи");
                        return;
                    }

                    Core.DB.Products.Remove(DeletedProduct);
                    Core.DB.SaveChanges();

                    MyProducts = Core.DB.Products.ToList();
                    UpdateValues();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Не смог удалить товар: " + ex.Message);
                }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var item = MainDataGrid.SelectedItem as Products;
            var EditProduct = Core.DB.Products.Find(item.Id);

            if (EditProduct != null)
            {
                var NewProduct = new ProductWindow(EditProduct);
                if ((bool)NewProduct.ShowDialog())
                {
                    MyProducts = Core.DB.Products.ToList();
                    UpdateValues();
                }
            }
        }

        private void ComboBox_Selected(object sender, RoutedEventArgs e)
        {
            ManufacturerFilterId = (ManufacturersFilter.SelectedItem as Manufacturers).Id;
        }

        private void SearchTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            ProductNameFilter = SearchTextBox.Text;
        }
    }
}
