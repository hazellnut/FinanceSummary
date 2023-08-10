using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using FinanceSummary.Tests;
using AbnLookup.SearchClientCSharpe;
using System.Windows;

namespace FinanceSummary.ViewModels
{
    public class TransactionView_ViewModel
    {
        public ICommand TestWindow { get; set; }
        public ICommand TestBusinessSearch { get; set; }
        public TransactionView_ViewModel()
        {
            TestWindow = new RelayCommand(LoadWindow);
            TestBusinessSearch = new RelayCommand(BusinessSearch);
        }

        private async void LoadWindow(object obj)
        {
            TestWindow test = new();
            if (test.ShowDialog() == true)
            {
                Console.WriteLine("Yay");
            }
        }

        private async void BusinessSearch(object obj)
        {
            HttpGetSearch test = new HttpGetDocumentSearch();
            string falseflag = SetFlag(false);
            string trueflag = SetFlag(true);
            string SearchPayLoad = test.NameSearch("MADAME FROU",
                trueflag,
                trueflag,
                falseflag,
                falseflag,
                falseflag,
                falseflag,
                falseflag,
                falseflag,
                "",
                "",
                "",
                "dc6e3c62-0aaa-4a1f-8204-9874428d9118");
            MessageBox.Show(SearchPayLoad);
        }

        private static string SetFlag(bool checkedIdent)
        {
            const string YES = "Y";
            const string NO = "N";
            if (checkedIdent)
            {
                return YES;
            }
            else
            {
                return NO;
            }
        }
    }
}
