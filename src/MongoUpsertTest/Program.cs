using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoUpsertTest;

namespace MongoUpsertTest
{
    //public class Program
    //{
    //    public static void Main(string[] args)
    //    {
    //        var mongoClient = new MongoClient("mongodb://localhost:27017");
    //        var database = mongoClient.GetDatabase("UpsertTest");

    //        var collection = database.GetCollection<Upsert>("Upserts");


    //        var names = new[] { "one", "two", "three", "quatro" };
    //        var i = 0;

    //        while (true)
    //        {
    //            var item = new Item { Time = DateTime.Now, Name = names[i++ % 4] };

    //            var updateDefinition = new UpdateDefinitionBuilder<Upsert>()
    //                .Set(x => x.Items[-1], item)
    //                .Set(device => device.IsSomething, true)
    //                .Set(device => device.LastOne, item.Time);

    //            collection.UpdateOne(x => x.Serial == "el paso del caballo" && x.Items.Any(t => t.Name == item.Name), updateDefinition, new UpdateOptions { IsUpsert = true});

    //            Console.ReadKey();
    //        }
    //    }
    //}
    public class Program
    {
        public static void Main(string[] args)
        {
            var mongoClient = new MongoClient("mongodb://localhost:27017");
            var database = mongoClient.GetDatabase("UpsertTest");

            var collection = database.GetCollection<Upsert>("Upserts");


            var names = new[] { "one", "two", "three", "quatro" };
            var i = 0;

            while (true)
            {
                var item = new Item { Time = DateTime.Now, Name = names[i++ % 4] };

                var updateDefinition1 = new UpdateDefinitionBuilder<Upsert>()
                    .PullFilter(device => device.Items, t => t.Name.Equals(item.Name));
                var updateDefinition2 = new UpdateDefinitionBuilder<Upsert>()
                    .Push(device => device.Items, item)
                    .Set(device => device.IsSomething, true)
                    .Set(device => device.LastOne, item.Time);

                collection.UpdateOne(x => x.Serial == "halapaloosa", updateDefinition1, new UpdateOptions { IsUpsert = true });
                collection.UpdateOne(x => x.Serial == "halapaloosa", updateDefinition2, new UpdateOptions { IsUpsert = true });

                Console.ReadKey();
            }
        }
    }

    class Upsert
    {
        public string Serial { get; set; }
        public List<Item> Items { get; set; }
        public bool IsSomething { get; set; }
        public DateTime? LastOne { get; set; }
    }

    internal class Item
    {
        public string Name { get; set; }
        public DateTime? Time { get; set; }
    }
}
