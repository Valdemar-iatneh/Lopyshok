using Lopyshok.DB;
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

namespace Lopyshok.Pages
{
    /// <summary>
    /// Interaction logic for ProductListPage.xaml
    /// </summary>
    public partial class ProductListPage : Page
    {
        private List<Product> productList = new List<Product>();
        private List<ProductType> productTypeList = new List<ProductType>();
        public ProductListPage()
        {
            InitializeComponent();
            productList = DBConnection.connection.Product.ToList();
            productTypeList = DBConnection.connection.ProductType.ToList();
            productTypeList.Insert(0, new ProductType { Id = -1, Name = "Все типы" });
            ProductLV.ItemsSource = productList;
            TypeCB.ItemsSource = productTypeList;
            SortingCB.SelectedIndex = 0;
            TypeCB.SelectedIndex = 0;
        }
       
        private void ToFilter()
        {
            var productList = (IEnumerable<Product>)DBConnection.connection.Product.ToList();

            //Поиск по наименованию
            if (!string.IsNullOrWhiteSpace(SearchTB.Text)) {
                productList = productList.Where(x => x.Name.StartsWith(SearchTB.Text));
            }

            //Фильтрация по типам
            if (TypeCB.SelectedIndex > 0) 
            {
                productList = productList.Where(x => x.ProductTypeId == (TypeCB.SelectedItem as ProductType).Id);
            }

            //Сортировки
            switch (SortingCB.SelectedIndex) 
            {
                //Значение по умолчанию
                case 0:
                    productList = productList.OrderBy(x => x.Id);
                    break;
                //Минимальная стоимость по убыванию
                case 1: 
                    productList = productList.OrderByDescending(x => x.MinPrice);
                    break;
                //Минимальная стоимость по возрастанию
                case 2:
                    productList = productList.OrderBy(x => x.MinPrice);
                    break;
                //Номер цеха по убыванию
                case 3:
                    productList = productList.OrderBy(x => x.ManufactoryNumber);
                    break;
                //Номер цеха по возрастанию
                case 4:
                    productList = productList.OrderByDescending(x => x.ManufactoryNumber);
                    break;
                //От А до Я
                case 5:
                    productList = productList.OrderBy(x => x.Name);
                    break;
                //От Я до А
                case 6:
                    productList = productList.OrderByDescending(x => x.Name);
                    break;
            }
            ProductLV.ItemsSource = productList;
        }

        private void ProductLV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            NavigationService.Navigate(new ProductPage(ProductLV.SelectedItem as Product));
        }

        private void SearchTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            ToFilter();
        }

        private void SortingCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ToFilter();
        }

        private void TypeCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ToFilter();
        }

        private void NewProductBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ProductPage(new Product()));
        }
    }
}
