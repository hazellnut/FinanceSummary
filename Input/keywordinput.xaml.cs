using FinanceSummary.Models;
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
using System.Windows.Shapes;

namespace FinanceSummary.Input
{
    /// <summary>
    /// Interaction logic for keywordinput.xaml
    /// </summary>
    public partial class keywordinput : Window
    {
        public KeywordPair keywordpair { get; set; } = new();
        public keywordinput()
        {
            InitializeComponent();
            categories.ItemsSource = DatabaseAccess.get_categories();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            keywordpair.keyword = keyword.Text;
            keywordpair.category = categories.Text;
            this.DialogResult = true;
        }
    }
}
