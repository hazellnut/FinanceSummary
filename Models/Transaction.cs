using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceSummary.Models
{
    public class Transaction
    {
        //this holds the data for an individual transaction



        public TransactionCategory Category { get; set; }
        public string CatName
        {
            get { return Category.name; }
        }
        public string TransactionCompany { get; set; }
        public DateTime Date { get; set; }
        public double Amount { get; set; }

        public Account Account { get; set; }

        public int AccountID
        {
            get { return Account.id; }
        }


        public Transaction()
        {

        }
        public Transaction(Account account,string date, string TransactionString, double debit)
        {
            Date = DateTime.Parse(date);
            Amount = debit;
            Account = account;
            //try
            //{
            //    //TransactionCategory test = DatabaseAccess.find_company_category();
            //}
        }

    }



}
