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
using WPF.Model;

namespace WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MyContext _context = new MyContext();
        Supplier supplier = new Supplier();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (NametextBox.Text == "")
            {
                MessageBox.Show("Insert Name Supplier");
            }
            else
            {
                supplier.Name = NametextBox.Text;
                supplier.JoinDate = DateTimeOffset.Now.LocalDateTime;
                supplier.CreateDate = DateTimeOffset.Now.LocalDateTime;
                _context.Suppliers.Add(supplier);
                var result = _context.SaveChanges();
                if (result > 0)
                {
                    MessageBox.Show("Insert Success");
                    NametextBox.Text = "";
                }
                else
                {
                    MessageBox.Show("Insert Failed");
                }
            }
        }
        private void NametextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("^[a-zA-Z .]*$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }

        private void NametextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CounttextBox.Text = NametextBox.Text.Length.ToString();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SupplierdataGrid.ItemsSource = _context.Suppliers.Where(x => x.isDelete == false).ToList();
            SuppliercomboBox.ItemsSource = _context.Suppliers.Where(x => x.isDelete == false).ToList();
        }
        
        private void SupplierdataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            object item = SupplierdataGrid.SelectedItem;
            NametextBox.Text = (SupplierdataGrid.SelectedCells[1].Column.GetCellContent(item) as TextBlock).Text;
            // SuppliercomboBox.Text = (SupplierdataGrid.SelectedCells[1].Column.GetCellContent(item) as TextBlock).Text;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (SearchtextBox.Text == "")
            {
                MessageBox.Show("Insert Search Keywoard");
            }
            else
            {
                string search = SearchtextBox.Text;
                string category = SearchcomboBox.Text;
                if (category == "Id")
                {
                    SupplierdataGrid.ItemsSource = _context.Suppliers.Where(x => (x.isDelete == false) && (x.Id.ToString().Contains(search))).ToList();
                }
                else if (category == "Name")
                {
                    SupplierdataGrid.ItemsSource = _context.Suppliers.Where(x => (x.isDelete == false) && (x.Name.Contains(search))).ToList();
                }
                else if (category == "Join Date")
                {
                    DateTimeOffset dateSeacrh = DateTimeOffset.Parse(search);
                    SupplierdataGrid.ItemsSource = _context.Suppliers.Where(x => (x.isDelete == false) && (x.JoinDate >= dateSeacrh)).ToList();
                }
                else
                {
                    MessageBox.Show("Please Choice Category Search First!");
                }
            }
        }
    }
}
