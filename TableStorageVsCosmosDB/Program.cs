namespace TableSBS
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Table;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Concurrent;

    /// <summary>
    /// This sample program shows how to use the Azure storage SDK to work with premium tables 
    /// (created using the Azure Cosmos DB service)
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Run common Table CRUD and query operations using the Azure Cosmos DB endpoints 
        /// ("premium tables")
        /// </summary>
        /// <param name="args">Command line arguments</param>
        public static void Main(string[] args)
        {
            string connectionString = ConfigurationManager.AppSettings["CosmosStorageConnectionString"];

            int numIterations = 100;

            //run against Cosmo DB Table API
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);

            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            Program p = new Program();

            p.Run(tableClient, numIterations);


            //run against Azure Table Storage
            connectionString = ConfigurationManager.AppSettings["TableStorageConnectionString"];

            storageAccount = CloudStorageAccount.Parse(connectionString);

            tableClient = storageAccount.CreateCloudTableClient();

            p.Run(tableClient, numIterations);


            Console.WriteLine("\n");
            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();

        }

        /// <summary>
        /// Run a bunch of core Table operations. Each operation is run ~100 times to measure latency. 
        /// You can swap the endpoint and compare with regular Azure Table storage.
        /// </summary>
        /// <param name="tableClient">The Azure Table storage client</param>
        /// <param name="numIterations">Number of iterations</param>
        public void Run(CloudTableClient tableClient, int numIterations)
        {
            Console.WriteLine("\n");
            Console.WriteLine("Creating Table if it doesn't exist...");

            CloudTable table = tableClient.GetTableReference("People");
            table.DeleteIfExists();
            table.CreateIfNotExists();

            List<CustomerEntity> items = new List<CustomerEntity>();
            Stopwatch watch = new Stopwatch();

            Console.WriteLine("\n");
            Console.WriteLine("Running inserts: ");
            for (int i = 0; i < numIterations; i++)
            {
                watch.Start();

                CustomerEntity item = new CustomerEntity()
                {
                    PartitionKey = Guid.NewGuid().ToString(),
                    RowKey = Guid.NewGuid().ToString(),
                    Email = $"{GetRandomString(6)}@contoso.com",
                    PhoneNumber = "425-555-0102",
                    Bio = GetRandomString(1000)                   
                };

                TableOperation insertOperation = TableOperation.Insert(item);
                table.Execute(insertOperation);
                double latencyInMs = watch.Elapsed.TotalMilliseconds;

                Console.Write($"\r\tInsert #{i + 1} completed in {latencyInMs} ms.");
                items.Add(item);

                watch.Reset();
            }

            Console.WriteLine("\n");
            Console.WriteLine("Running retrieves: ");

            for (int i = 0; i < numIterations; i++)
            {
                watch.Start();

                TableOperation retrieveOperation = TableOperation.Retrieve<CustomerEntity>(items[i].PartitionKey, items[i].RowKey);
                table.Execute(retrieveOperation);
                double latencyInMs = watch.Elapsed.TotalMilliseconds;

                Console.Write($"\r\tRetrieve #{i + 1} completed in {latencyInMs} ms");

                watch.Reset();
            } 

            Console.WriteLine("\n");
            Console.WriteLine("Running replace: ");


            for (int i = 0; i < numIterations; i++)
            {
                watch.Start();

                // Same latency as inserts, p99 < 15ms, and p50 < 6ms
                items[i].PhoneNumber = "425-555-5555";
                TableOperation replaceOperation = TableOperation.Replace(items[i]);
                table.Execute(replaceOperation);

                double latencyInMs = watch.Elapsed.TotalMilliseconds;
                Console.Write($"\r\tReplace #{i + 1} completed in {latencyInMs} ms");

                watch.Reset();
            }
        }

        public string GetRandomString(int length)
        {
            Random random = new Random(System.Environment.TickCount);
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public class CustomerEntity : TableEntity
        {
            public CustomerEntity(string lastName, string firstName)
            {
                this.PartitionKey = lastName;
                this.RowKey = firstName;
            }

            public CustomerEntity() { }

            public string Email { get; set; }

            public string PhoneNumber { get; set; }

            public string Bio { get; set; }
        }
    }    
}
