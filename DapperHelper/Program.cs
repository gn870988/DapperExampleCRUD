using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Transactions;

namespace DapperHelper
{
    class Program
    {
        // 選擇資料庫類型及連線字串(目前支援MsSql(預設)、SqlLite，可自行擴充)
        static DapperHelper d = new DapperHelper(Dialect.SqlLite, ConnectionSelecter.SqlLiteLocalDb);

        static void Main(string[] args)
        {
            // InitDb
            CreatSqlLiteDb();

            Console.WriteLine("");
            Example1();

            Console.WriteLine("");
            Example2();

            Console.WriteLine("");
            Example3();

            // [Update]
            //Console.WriteLine("");
            //Example4();

            // [Delete]
            //Console.WriteLine("");
            //Example5();

            // [Insert(TransactionScope)]
            //Console.WriteLine("");
            //Example6();

            Console.WriteLine("");
            Example7();

            Console.ReadKey();
        }

        static void Example1()
        {
            Console.WriteLine("一般Select");
            Console.WriteLine("---------------------------------------------------");
            string sql = "Select * From Member";
            var Value = d.Query<Member>(sql);
            foreach (var item in Value)
            {
                Console.WriteLine($"Id：{item.Id}，Name：{item.Name}，DisplayName：{item.DisplayName}");
            }
            Console.WriteLine("---------------------------------------------------");
        }

        static void Example2()
        {
            Console.WriteLine("Select一筆資料");
            Console.WriteLine("---------------------------------------------------");
            string sql = "Select * From Member Where Id=@Id";
            var m = new Member { Id = 2 };
            var Value = d.QueryFirstOrDefault(sql, m);
            Console.WriteLine($"Id：{Value.Id}，Name：{Value.Name}，DisplayName：{Value.DisplayName}");
            Console.WriteLine("---------------------------------------------------");
        }

        static void Example3()
        {
            Console.WriteLine("Where多筆資料");
            Console.WriteLine("---------------------------------------------------");
            string sql = "Select * From Member Where Id in @Id";
            var dps = new DynamicParameters();
            var dict = new Dictionary<string, object>();
            var list = new List<string>();
            dict.Add("Id", list);
            list.Add("1");
            list.Add("2");
            dps.AddDynamicParams(dict);
            var Value = d.Query(sql, dps);
            foreach (var item in Value)
            {
                Console.WriteLine($"Id：{item.Id}，Name：{item.Name}，DisplayName：{item.DisplayName}");
            }
            Console.WriteLine("---------------------------------------------------");
        }

        static void Example4()
        {
            Console.WriteLine("已更新資料");
            string sql = "Update Member Set Name=@Name, DisplayName=@DisplayName where Id=@Id";
            List<Member> list = new List<Member>()
            {
                new Member() { Id = 4 , Name ="更新1", DisplayName="已更新1"},
                new Member() { Id = 5 , Name ="更新2", DisplayName="已更新2"},
            };
            d.Execute(sql, list);
        }

        static void Example5()
        {
            Console.WriteLine("已刪除資料");
            string sql = "DELETE FROM Member WHERE id=@id";
            var u = new Member() { Id = 3 };
            d.Execute(sql, u);
        }

        static void Example6()
        {
            Console.WriteLine("已新增資料(TransactionScope)");
            string sql = "Insert into Member (Name, DisplayName) Values (@Name, @DisplayName);";
            var list = new List<Member>();
            list.Add(new Member { Name = "TransactionScope1", DisplayName = "T1" });
            list.Add(new Member { Name = "TransactionScope2", DisplayName = "T2" });
            using (var ts = new TransactionScope())
            {
                d.Execute(sql, list);
                ts.Complete();
            }
        }

        static void Example7()
        {
            Console.WriteLine("分頁範例");
            Console.WriteLine("---------------------------------------------------");
            Console.WriteLine($"");
            string sql = "Select * From Member";
            var Value = d.Query<Member>(sql);
            int pagesize = 2;
            int pageindex = Value.Count / pagesize;
            int totalindex = Value.Count % pagesize == 0 ? pageindex : pageindex + 1;

            for (int i = 1; i <= totalindex; i++)
            {
                Console.WriteLine($"【第{i}頁】");
                var PartValue = d.SearchByPagerNum(Value, pagesize, i);
                foreach (var item in PartValue)
                {
                    Console.WriteLine($"Id：{item.Id}，Name：{item.Name}，DisplayName：{item.DisplayName}");
                }
                Console.WriteLine($"");
            }
            Console.WriteLine("---------------------------------------------------");
        }

        /// <summary>
        /// 建立(Create)資料表及新增(Insert)資料
        /// </summary>
        static void CreatSqlLiteDb()
        {
            string dbPath = Properties.Settings.Default.SqlLiteDbPath;
            if (File.Exists(dbPath)) return;
            SQLiteConnection.CreateFile(dbPath);

            CreateData();
            InsertData();
        }

        /// <summary>
        /// Create 範例
        /// </summary>
        static void CreateData()
        {
            d.Execute(@"CREATE TABLE IF NOT EXISTS Member 
                        (Id INTEGER PRIMARY KEY AUTOINCREMENT, 
                         Name VARCHAR(30), 
                         DisplayName VARCHAR(30));");
        }

        /// <summary>
        /// // Insert 範例
        /// </summary>
        static void InsertData()
        {
            string sql = "Insert into Member (Name, DisplayName) Values (@Name, @DisplayName);";
            var list = new List<Member>();
            list.Add(new Member { Name = "Vincent", DisplayName = "A" });
            list.Add(new Member { Name = "Jasmine", DisplayName = "B" });
            list.Add(new Member { Name = "Jay", DisplayName = "C" });
            list.Add(new Member { Name = "Jack", DisplayName = "D" });
            list.Add(new Member { Name = "Mary", DisplayName = "E" });
            d.Execute(sql, list);
        }
    }
}
