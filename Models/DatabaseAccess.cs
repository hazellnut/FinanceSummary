
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
using System.Reflection;
using System.Windows.Forms.PropertyGridInternal;
using System.Xml.Linq;

namespace FinanceSummary.Models
{
    public static class DatabaseAccess
    {
        public static string ConnectionString;
        public static IEnumerable<Transaction> get_transactions()
        {
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
            using (IDbConnection db = new MySqlConnection(ConnectionString))
            {
                var sql = @"SELECT 
                            * from transactions INNER JOIN accounts on (account_id = accounts.id) INNER JOIN companies on (company =idcompany);";


                var transactions = db.Query<Transaction, Account, Company, Transaction>(sql, (trans, acc, comp) =>
                {
                    trans.Account = acc;
                    trans.company = comp;
                    return trans;

                }, splitOn: "company,idcompany");
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
                var sql = @"INSERT IGNORE INTO `transactions`
                            (`datetime`,
                            `amount`,
                            `category`,
                            `account_id`,
                            `company`)
                            VALUES
                            (@datetime,
                            @Amount,
                            @Category,
                            @AccountID,
                            @CompanyID);";

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
            add_single<Company>(new Company() { companystring = company, category = category }, "companies");


            //using (IDbConnection db = new MySqlConnection(ConnectionString))
            //{
            //    var sql = @"INSERT INTO `companies` (`companystring`,`category`)
            //            VALUES
            //            (@company, @category);";


            //    db.Execute(sql, new { company = company, category = category });
            //}
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
                    if (company.Length < 255)
                        add_company(company.Trim('\"'), cat);
                    return cat;
                }

            }
        }

        public static Company find_company(string company)
        {
           
            using (IDbConnection db = new MySqlConnection(ConnectionString))
            {
                try
                {
                    var sql = @"SELECT * FROM  (`companies`) WHERE (`companystring` = @company);";
                    var result = db.Query<Company>(sql, new { company = company.Trim('\"') }).First();
                    return result;
                }
                catch
                {
                    Console.WriteLine("too long!");
                    return new Company();
                }
            }
        }

        public static void update_category(Transaction tr)
        {
            using (IDbConnection db = new MySqlConnection(ConnectionString))
            {
                //try
                //{
                    var sql = @"UPDATE `transactions` SET `category` = @Category WHERE (`company` = @CompanyID);";
                    var companysql = @"UPDATE `companies` SET `category` = @category WHERE (`idcompany` = @idcompany);";
                    db.Execute(companysql, tr.company);
                    db.Execute(sql, tr);
                    
                //}
                //catch
                //{
                //    Console.WriteLine("Some issue");
                //}
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
            return get_all<string>("transaction_categories");

            //using (IDbConnection db = new MySqlConnection(ConnectionString))
            //{
            //    var sql = @"SELECT * FROM  `transaction_categories`;";
            //    try
            //    {
            //        List<string> accs = db.Query<string>(sql).ToList();
            //        return accs;
            //    }
            //    catch
            //    {
            //        MessageBox.Show("Error loading categories!"); //need to implement
            //        //do keyword search
            //        return new List<string>();
            //    }

            //}
        }


        public static List<T> get_all<T>(string table)
        {
            using (IDbConnection db = new MySqlConnection(ConnectionString))
            {
                var sql = @"SELECT * FROM  " + table + ";";
                try
                {
                    List<T> items = db.Query<T>(sql).ToList();
                    return items;
                }
                catch
                {
                    MessageBox.Show("Error loading categories!"); //need to implement
                                                                  //do keyword search
                    return new List<T>();
                }
            }
        }


        public static void add_single<T>(T dbobject, string table)
        {
            try
            {
                using (IDbConnection db = new MySqlConnection(ConnectionString))
                {

                    PropertyInfo[] pis = dbobject.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                    List<string> props = new();
                    var endstr = " VALUES (";
                    var sql = @"INSERT INTO `" + table + "` (";
                    foreach (PropertyInfo pi in pis)
                    {
                        sql += "`" + pi.Name + "`,";
                        endstr += "@" + pi.Name + ",";
                    }
                    sql = sql.Remove(sql.Length - 1, 1);
                    endstr = endstr.Remove(endstr.Length - 1, 1);
                    sql += ") " + endstr + ");";

                    db.Execute(sql, dbobject);
                }
            }
            catch
            {
                Console.WriteLine("Duplicate entry");
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
