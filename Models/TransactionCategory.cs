using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FinanceSummary.Models
{
    public class TransactionCategory
    {
        private string _name;
        public string name
        {
            get { return _name; }
            set
            {
                if (!Categories.Contains(value))
                {
                    MessageBox.Show("Incorrect cateogy!");
                    return;
                }
                _name = value;
            }
        }


        public TransactionCategory()
        {

        }


        public List<string> SubCategories { get; set; }

        public List<string> Categories = new()
        {
            "Groceries",
            "Bills",
            "Subscriptions",
            "Gifts",
            "Eating Out/ Takeaway",
            "House Necessities (e.g. Furniture)",
            "Repairs/Maintenance"
        };

    }
}
