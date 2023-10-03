using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using FinanceSummary.Tests;
using AbnLookup.SearchClientCSharpe;
using System.Windows;
using FinanceSummary.Import;
using System.ComponentModel;

namespace FinanceSummary.Models
{
    public class TransactionView_ViewModel: PrChanged
    {
        public ICommand TestWindow { get; set; }
        public ICommand TestBusinessSearch { get; set; }
        public ICommand ImportCSV { get; set; }
        public ICommand Categorise { get; set; }

        private List<Transaction> _Transactions { get; set; }
        public List<Transaction> Transactions
        {
            get
            {
                return _Transactions;
            }
            set
            { _Transactions = value;
                RaisePropertyChanged();
            }
        }

        private List<string> _Categories { get; set; }
        public List<string> Categories
        {
            get
            {
                return _Categories;
            }
            set
            {
                _Categories = value;
                RaisePropertyChanged();
            }
        }

        private string _SelectedCategory { get; set; }
        public string SelectedCategory
        {
            get
            {
                return _SelectedCategory;
            }
            set { _SelectedCategory = value; RaisePropertyChanged(); }
        }

        private List<Transaction> _SelectedTransactions { get; set; }
        public List<Transaction> SelectedTransactions
        {
            get
            {
                return _SelectedTransactions;
            }
            set { _SelectedTransactions = value; RaisePropertyChanged(); }
        }



        public TransactionView_ViewModel()
        {
            TestWindow = new RelayCommand(LoadWindow);
            TestBusinessSearch = new RelayCommand(BusinessSearch);
            ImportCSV = new RelayCommand(import_csv);
            Categorise = new RelayCommand(categorise);
            Transactions = DatabaseAccess.get_transactions().ToList();
            Categories = DatabaseAccess.get_categories().ToList();
        }


        public async void LoadTransactions()
        {
            Transactions = DatabaseAccess.get_transactions().ToList();
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

        public async void import_csv(object obj)
        {
            ImportWindow window = new();
            window.ShowDialog();
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

        public async void categorise(object obj)
        {

        }
    }
}
