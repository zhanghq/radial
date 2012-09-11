using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Radial.Data.Mongod;
using Radial.Data;
using MongoDB.Driver.Builders;

namespace Radial.UnitTest
{
    public class MongoBook
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public DateTime Date { get; set; }
    }

    public class MongoBookRepository : MongoRepository<MongoBook, string>
    {

        public override bool Exists(string key)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(key), "object kay can not be empty or null");

            return GetTotal(o => o.Id == key.Trim()) > 0;
        }

        public override MongoBook Find(string key)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(key), "object kay can not be empty or null");

            return Find(o => o.Id == key.Trim());
        }

        public override void Remove(MongoBook obj)
        {
            if (obj != null)
            {
                var query = Query.EQ("_id", obj.Id);
                WriteDatabase.GetCollection<MongoBook>(CollectionName).Remove(query);
            }
        }
    }

    [TestFixture]
    public class MongodTest
    {

        [Test]
        public void Exist()
        {
            MongoBookRepository repository = new MongoBookRepository();

            repository.Clear();

            string id = Guid.NewGuid().ToString("N");

            repository.Save(new MongoBook { Id = id, Title = "Test Book 1", Date = DateTime.Now.Date });

            Assert.IsTrue(repository.Exists(o => o.Title == "Test Book 1"));

            repository.Remove(id);

            repository.Clear();
        }

        [Test]
        public void Paging()
        {
            MongoBookRepository repository = new MongoBookRepository();

            repository.Clear();

            IList<MongoBook> bks = new List<MongoBook>();

            for (int i = 0; i < 30; i++)
            {
                bks.Add(new MongoBook { Id = Guid.NewGuid().ToString("N"), Title = "Test Book " + i, Date = DateTime.Now.Date });
            }

            repository.Add(bks);

            int objectTotal;
            bks = repository.FindAll(null, new OrderBySnippet<MongoBook>[] { new OrderBySnippet<MongoBook>(o => o.Title) }, 10, 1, out objectTotal);

            Assert.AreEqual(30, objectTotal);

            repository.Clear();
        }

        [Test]
        public void Paging2()
        {
            MongoBookRepository repository = new MongoBookRepository();

            repository.Clear();

            IList<MongoBook> bks = new List<MongoBook>();

            for (int i = 0; i < 30; i++)
            {
                bks.Add(new MongoBook { Id = Guid.NewGuid().ToString("N"), Title = "Test Book " + i, Date = DateTime.Now.Date.AddDays(i) });
            }

            repository.Add(bks);

            int objectTotal;
            bks = repository.FindAll(o => o.Date > DateTime.Now.Date.AddDays(2), 10, 1, out objectTotal);

            Assert.AreEqual(27, objectTotal);

            repository.Clear();
        }

    }
}
