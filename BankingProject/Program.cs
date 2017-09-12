using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using BankingLibrary;

namespace BankingProject
{
    class Program
    {
        static void Main(string[] args)
        {
            //new Program().Run();
        }

        // Changes the active database
        void SetDatabase(ref SqlConnection conn, string dbName)
        {
            conn.ChangeDatabase(dbName);
        }

        // Gets connection settings and returns SQL connection string
        string GetConnectionSettings()
        {
            string connectionString, 
                username = "", 
                password = "", 
                server = "";

            using (XmlReader reader = XmlReader.Create("settings.xml"))
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        switch (reader.Name)
                        {
                            case "settings":
                                Console.WriteLine("Found settings in settings.xml");
                                break;

                            case "username":
                                Console.WriteLine("Found username.");
                                if (reader.Read())
                                    username = reader.Value.Trim();
                                break;

                            case "password":
                                Console.WriteLine("Found password.");
                                if (reader.Read())
                                    password = reader.Value.Trim();
                                break;

                            case "server":
                                Console.WriteLine("Found server name.");
                                if (reader.Read())
                                    server = reader.Value.Trim();
                                break;

                            default:
                                break;
                        }
                    }
                }
            }

                connectionString = $"user id={username};" +
                    $"password={password};" +
                    $"server={server};" +
                    $"Trusted_Connection=yes;connection timeout=30";
            return connectionString;
        }
        // Returns a list of strings of database names
        List<string> GetDatabases(ref SqlConnection conn)
        {
            DataTable schema = conn.GetSchema("Databases");
            List<string> result = new List<string>();
            foreach (DataRow row in schema.Rows)
            {
                result.Add(row[0].ToString());
            }
            return result;
        }

        // Returns a list of strings of table names in a current database
        List<string> GetTables(ref SqlConnection conn)
        {
            List<string> result = new List<string>();
            DataTable schema = conn.GetSchema("Tables");
            foreach (DataRow row in schema.Rows)
            {
                result.Add(row[3].ToString());
            }
            return result;
        }

        // Returns a list of column names
        List<string> GetColumns(ref SqlConnection conn, string tableName)
        {
            List<string> result = new List<string>();
            string[] restrictions = new string[3];
            restrictions[2] = tableName;
            DataTable schema = conn.GetSchema("Columns", restrictions);
            foreach (DataRow row in schema.Rows)
            {
                result.Add(row[3].ToString());
            }
            return result;
        }

        void Run2()
        {
            Account ac = new Account(473);
            Savings sv = new Savings(1092);
        }

        // Whole bunch of stuff in here... don't ask.
        void Run()
        {
            string connectionString = GetConnectionSettings();

            SqlConnection sqlConnection = new SqlConnection(connectionString);

            // Connect to the database
            try
            {
                sqlConnection.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            SetDatabase(ref sqlConnection, "DotNetDatabase");

            // Greet the user
            Console.WriteLine("\t\tWelcome to the Bank of Money!\n\n");
            Console.Write("Please enter your account number: ");
            int accountNumber = 0;
            try
            {
                accountNumber = int.Parse(Console.ReadLine());
            }
            catch (Exception e)
            {
                Console.WriteLine("Invalid input.");
            }

            try
            {
                SqlDataReader dataReader = null;
                SqlCommand command = new SqlCommand($"SELECT * FROM " +
                    $"Account a " +
                    $"JOIN Customer c ON a.Owner = c.Id " +
                    $"WHERE a.Id = {accountNumber.ToString()}", sqlConnection);

                dataReader = command.ExecuteReader();

                // Assume there is only one result and harvest our data
                dataReader.Read();
                if (dataReader.HasRows)
                {
                    Console.Clear();
                    Console.WriteLine($"Welcome {dataReader["FirstName"]}, your current balance is: {dataReader["Balance"]}.");
                }
                else
                {
                    Console.WriteLine("Invalid account number.");
                }
                dataReader.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine("Let's make a transaction!");
            Console.Write("What is the originating account number? ");
            int origin = 0;
            try
            {
                origin = int.Parse(Console.ReadLine());
            }
            catch (Exception e)
            {
                Console.WriteLine("Invalid input.");
            }

            Console.Write("What is the destination account number? ");
            int destination = 0;
            try
            {
                destination = int.Parse(Console.ReadLine());
            }
            catch (Exception e)
            {
                Console.WriteLine("Invalid input.");
            }

            Console.Write("How much would you like to move? ");
            double money = 0;
            try
            {
                money = double.Parse(Console.ReadLine());
            }
            catch (Exception e)
            {
                Console.WriteLine("Invalid input.");
            }

            Console.Write("Please enter a short memo: ");
            string memo = Console.ReadLine();

            // Perform an INSERT
            try
            {
                SqlCommand command = new SqlCommand($"INSERT into [Transaction]" +
                    $"(Origin, Destination, Amount, Memo) " +
                    $"VALUES ({origin.ToString()}, {destination.ToString()}, {money.ToString()}, '{memo}')", sqlConnection);
                command.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            // Close our connection before we exit
            try
            {
                sqlConnection.Close();
                Console.WriteLine("\n\nConnection closed.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
