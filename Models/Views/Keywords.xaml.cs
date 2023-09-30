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

namespace FinanceSummary.Models.Views
{
    /// <summary>
    /// Interaction logic for Keywords.xaml
    /// </summary>
    public partial class Keywords : UserControl
    {
        public Keywords()
        {
            InitializeComponent();
            UpdateDataGrid();
        }


        private void UpdateDataGrid()
        {
            Keyword_Datagrid.ItemsSource = DatabaseAccess.get_keywords();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Input.keywordinput kw = new();
            if (kw.ShowDialog() == true )
            {
                DatabaseAccess.add_keyword(kw.keywordpair);
            }
            UpdateDataGrid();
        }
    }
}
