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
using Microsoft.Win32;
using MySqlX.XDevAPI.Relational;
using System.IO;

namespace FinanceSummary.Import
{
    /// <summary>
    /// Interaction logic for ImportWindow.xaml
    /// </summary>
    public partial class ImportWindow : Window
    {
        public List<Account> AllAccounts { get; set; }
        public ImportWindow()
        {
            AllAccounts = DatabaseAccess.get_accounts();
            InitializeComponent();
            accountbox.ItemsSource = AllAccounts;
        }

        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Title = "Import Fifo";
            openFileDialog.DefaultExt = ".smb|.csv";
            openFileDialog.Filter = "Csv (.csv)|*.csv|All files (*.*)|*.*";
            openFileDialog.Multiselect = true;
            if (openFileDialog.ShowDialog() == true)
            {
                filename.Text = openFileDialog.FileName;
            }
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            List<Transaction> imported_transactions = new();
            if (File.Exists(filename.Text))
            {
                using (var reader = new StreamReader(File.OpenRead(filename.Text)))
                {
                    bool init = true;
                    while (!reader.EndOfStream)
                    {
                        if (init)
                        {
                            init = false;
                            var first = reader.ReadLine();
                            continue;
                        }
                        var line = reader.ReadLine();

                        
                        Transaction temp = new();
                        
                        temp.Account = accountbox.SelectedItem as Account;
                        string description;
                        double credit = 0;
                        double debit = 0;
                        double balance;
                        string purchasetype = "";
                        DateTime date;
                        switch (temp.Account.bank)
                        {
                            case "ING":
                                string[] values = Split_CSV_ING(line);
                                date = DateTime.Parse(values[0]);
                                description = values[1];
                                
                                credit = ParseDouble(values[2]);
                                debit = ParseDouble(values[3]);
                                balance = double.Parse(values[4]);
                                temp.TransactionCompany = ParseCompany(description);
                                purchasetype = ParsePurchaseType(description);
                                break;

                            default:
                            case "BankWest":
                                values = line.Split(",", StringSplitOptions.TrimEntries);
                                date = DateTime.Parse(values[2]);
                                description = values[3];
                                debit = ParseDouble(values[5]);
                                credit = ParseDouble(values[6]);
                                balance = double.Parse(values[7]);
                                temp.TransactionCompany = description;

                                break;
                        }
                        temp.datetime = date;
                        temp.Amount = credit + debit;
                        
                        

                        temp.Category = DatabaseAccess.find_company_category(temp.TransactionCompany, purchasetype);
                        temp.company = DatabaseAccess.find_company(temp.TransactionCompany);
                        imported_transactions.Add(temp);
                        //values will be an array of strings I think?
                    }
                    //need to marshall into the correct data format
                }
                DatabaseAccess.bulk_add_transaction(imported_transactions);
                this.DialogResult = true;
                this.Close();
            }
            
            else
            {
                throw new Exception("CSV File doesn't exist!");
            }
        }

        private string[] Split_CSV_ING(string line)
        {
            List<string> split_str = new();
            bool quoting = false;
            int i = 0;
            string current = "";
            foreach (char c in line)
            {
                
                if (c == '\"')
                {
                    quoting = !quoting;
                    current += c;
                }
                if (!quoting && c == ',')
                {
                    split_str.Add(current);
                    current = "";
                }
                else
                {
                    current += c;
                }
            }
            split_str.Add(current);
            return split_str.ToArray();
        }

        private double ParseDouble(string input)
        {
            if (input == "")
                return 0;
            return Double.Parse(input);
        }
        private string ParseCompany(string input_string)
        {
            string[] values = input_string.Split("-");
            return values[0];
        }

        private string ParsePurchaseType(string input_string)
        {
            string[] values = input_string.Split("-");
            return values[1];
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
