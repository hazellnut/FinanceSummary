
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
using System.Net.Http.Headers;


namespace FinanceSummary.Models
{
    public static class DatabaseAccess
    {
        public static string ConnectionString;
        public static IEnumerable<Transaction> get_transactions ()
        {
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
            using (IDbConnection db = new MySqlConnection(ConnectionString))
            {
                var sql = @"SELECT 
                            * from transactions INNER JOIN accounts on (account_id = accounts.id)";


                var transactions = db.Query<Transaction,Account,Transaction>(sql, (trans, acc) =>
                {
                    trans.Account = acc;
                    return trans;

                });
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
                            (@datetime,
                            @Amount,
                            @Category,
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

        public static void add_keyword(KeywordPair kw)
        {
            using (IDbConnection db = new MySqlConnection(ConnectionString))
            {
                var sql = @"INSERT INTO `keywords` (`keyword`,`category`)
                        VALUES
                        (@keyword,@category);";

                db.Execute(sql, kw);
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




        public static void add_company(string company, string category)
        {
            using (IDbConnection db = new MySqlConnection(ConnectionString))
            {
                var sql = @"INSERT INTO `companies` (`companystring`,`category`)
                        VALUES
                        (@company, @category);";


                db.Execute(sql, new { company=company, category = category });
            }
        }


        public static string find_company_category(string company, string purchasetype)
        {
            using (IDbConnection db = new MySqlConnection(ConnectionString))
            {
                var sql = @"SELECT ( `category`) FROM  (`companies`) WHERE (`companystring` = @company);";
                try
                {
                    string result = db.Query<string>(sql, new { company = company }).First();
                    return result;
                }
                catch
                {
                    //MessageBox.Show("company does not yet exist!"); //need to implement
                    //do keyword search
                    string cat = "";
                    cat = "Uncategorised";
                    switch (purchasetype)
                    {
                        case "Visa Purchase":
                            cat = "Uncategorised";
                            break;
                        case "Internal Transfer":
                            cat = "Transfer";
                            break;
                        default:
                            if (purchasetype.Contains("Receipt"))
                            {
                                cat = "Transfer";
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


        public static List<Company> get_companies()
        {
            using (IDbConnection db = new MySqlConnection(ConnectionString))
            {
                var sql = @"SELECT * FROM  `companies`;";
                try
                {
                    List<Company> comp = db.Query<Company>(sql).ToList();
                    return comp;
                }
                catch
                {
                    MessageBox.Show("Error loading companies!");
                    return new List<Company>();
                }

            }
        }



        public static List<KeywordPair> get_keywords()
        {
            using (IDbConnection db = new MySqlConnection(ConnectionString))
            {
                var sql = @"SELECT * FROM  `keywords`;";
                try
                {
                    List<KeywordPair> kws = db.Query<KeywordPair>(sql).ToList();
                    return kws;
                }
                catch
                {
                    MessageBox.Show("Error loading keywords!"); //need to implement
                    //do keyword search
                    return new List<KeywordPair>();
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
