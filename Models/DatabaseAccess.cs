
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using MySql.Data.MySqlClient;
using System.Windows;
using System.Security.Principal;
using System.Linq.Expressions;

namespace FinanceSummary.Models
{
    public static class DatabaseAccess
    {
        public static string ConnectionString;
        public static IEnumerable<Transaction> get_transactions ()
        {
            using (IDbConnection db = new MySqlConnection(ConnectionString))
            {
                var sql = @"SELECT 
                            * from transactions";
                List<Transaction> transactions = db.Query<Transaction>(sql).Distinct().ToList();
                return transactions;
            }
        }

        public static void add_transaction(Transaction input)
        {
            using (IDbConnection db = new MySqlConnection(ConnectionString))
            {
                var sql = @"INSERT INTO `transactions`
                            (`datetime`,
                            `amount`,
                            `category`,
                            `account_id`)
                            VALUES
                            (@Date,
                            @Amount,
                            @CatName,
                            @AccountID);";

                db.Execute(sql, input);
            }
        }

        public static void bulk_add_transaction(List<Transaction> input)
        {
            using (IDbConnection db = new MySqlConnection(ConnectionString))
            {
                var sql = @"INSERT INTO `transactions`
                            (`datetime`,
                            `amount`,
                            `category`,
                            `account_id`)
                            VALUES
                            (@Date,
                            @Amount,
                            @CatName,
                            @AccountID);";

                db.Execute(sql, input);
            }
        }

        public static void add_account(Account account)
        {
            using (IDbConnection db = new MySqlConnection(ConnectionString))
            {
                var sql = @"INSERT INTO `accounts`
                            (`name`,
                            `description`,
                            `balance`,
                            `accnumber`,
                            `bank`)
                            VALUES
                            (@name,
                            @description,
                            @balance,
                            @accnumber,
                            @bank);";

                db.Execute(sql, account);
            }
        }

        public static void add_category(TransactionCategory category)
        {
            using (IDbConnection db = new MySqlConnection(ConnectionString))
            {
                var sql = @"INSERT INTO `transaction_categories` (`category`)
                        VALUES
                        (@name);";

                db.Execute(sql, category);
            }
        }

        public static void add_category_list(TransactionCategory category)
        {
            using (IDbConnection db = new MySqlConnection(ConnectionString))
            {
                var sql = @"INSERT INTO `transaction_categories` (`category`)
                        VALUES
                        (@name);";

                var cats = new List<Object>();
                foreach (string cat in category.Categories)
                {
                    cats.Add(new { name = cat });
                }
                db.Execute(sql, cats);
            }
        }




        public static void add_company(string company, TransactionCategory category)
        {
            using (IDbConnection db = new MySqlConnection(ConnectionString))
            {
                var sql = @"INSERT INTO `companies` (`companystring`,`category`)
                        VALUES
                        (@company, @category);";


                db.Execute(sql, new { company=company, category = category.name });
            }
        }


        public static TransactionCategory find_company_category(string company, string purchasetype)
        {
            using (IDbConnection db = new MySqlConnection(ConnectionString))
            {
                var sql = @"SELECT ( `category`) FROM  (`companies`) WHERE (`companystring` = @company);";
                try
                {
                    string result = db.Query<string>(sql, new { company = company }).First();
                    TransactionCategory cat = new();
                    cat.name = result;
                    return cat;
                }
                catch
                {
                    //MessageBox.Show("company does not yet exist!"); //need to implement
                    //do keyword search
                    TransactionCategory cat = new TransactionCategory();
                    cat.name = "Uncategorised";
                    switch (purchasetype)
                    {
                        case "Visa Purchase":
                            cat.name = "Uncategorised";
                            break;
                        case "Internal Transfer":
                            cat.name = "Transfer";
                            break;
                        default:
                            if (purchasetype.Contains("Receipt"))
                            {
                                cat.name = "Transfer";
                            }
                            break;
                    }
                    if (company.Length < 45)
                        add_company(company, cat);
                    return cat;
                }

            }
        }

        public static List<Account> get_accounts()
        {
            using (IDbConnection db = new MySqlConnection(ConnectionString))
            {
                var sql = @"SELECT * FROM  `accounts`;";
                try
                {
                    List<Account> accs = db.Query<Account>(sql).ToList();
                    return accs;
                }
                catch
                {
                    MessageBox.Show("Error loading accounts!"); //need to implement
                    //do keyword search
                    return new List<Account>();
                }

            }
        }

        public static List<string> get_categories()
        {
            using (IDbConnection db = new MySqlConnection(ConnectionString))
            {
                var sql = @"SELECT * FROM  `transaction_categories`;";
                try
                {
                    List<string> accs = db.Query<string>(sql).ToList();
                    return accs;
                }
                catch
                {
                    MessageBox.Show("Error loading categories!"); //need to implement
                    //do keyword search
                    return new List<string>();
                }

            }
        }




        public static void delete_account(Account account)
        {

            using (IDbConnection db = new MySqlConnection(ConnectionString))
            {
                var sql = @"DELETE FROM `accounts` WHERE (`id` = @id );";
                db.Execute(sql, account);
            }

        }

    }
}
