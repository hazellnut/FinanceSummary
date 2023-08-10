using FinanceSummary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FinanceSummary.Tests
{
    public static class Testing
    {
        public static void add_categories()
        {
            TransactionCategory category = new TransactionCategory();
            DatabaseAccess.add_category_list(category);
        }
        public static void test_add_account()
        {
            test_add_account(false);
        }
        public static void test_add_account(bool? autodelete)
        {
            Account account = new Account();
            account.balance = 1000;
            account.name = "Savings";
            account.description = "Everyday ING";
            account.accnumber = "310218813";
            account.bank = "ING";
            DatabaseAccess.add_account(account);

            List<Account> accs = DatabaseAccess.get_accounts();
            var currentacc = accs.Where(x => (x.name == "Savings" && x.accnumber == "310218813"));
            if (currentacc.Any())
            {
                MessageBox.Show("Success");
                if (autodelete==true)
                {
                    DatabaseAccess.delete_account(currentacc.First());
                }
            }
            else
                MessageBox.Show("Failure");
        }


        public static void test_add_transaction()
        {
            Transaction transaction = new Transaction();
            transaction.Amount = 100.0;
            transaction.Category = new TransactionCategory() { name = "Bills" };
            transaction.Date = DateTime.Now;
            transaction.Account = new Account();
            List<Account> accs = DatabaseAccess.get_accounts();
            if (!accs.Any())
            {
                test_add_account();
            }
            transaction.Account = accs.First();
            
            DatabaseAccess.add_transaction(transaction); 
            //transaction.Account =
            //DatabaseAccess

        }
    }
}
