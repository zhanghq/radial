using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Radial.Data.Mongod;
using Radial.Data;
using Norm;

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
        public override void Remove(string key)
        {
            Remove(Get(key));

        }

        public override bool Exist(string key)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(key), "object kay can not be empty or null");

            return GetTotal(o => o.Id == key.Trim()) > 0;
        }

        public override MongoBook Get(string key)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(key), "object kay can not be empty or null");

            return Get(o => o.Id == key.Trim());
        }

        public IList<MongoBook> OrderTest()
        {
            using (var db = CreateReadDb())
            {
                var query = db.GetCollection<MongoBook>(CollectionName).AsQueryable();

                return query.Where(o => o.Date > DateTime.Now).OrderByDescending(o => o.Date).Take(2).Skip(2).ToList();
            }
        }

        public override IList<MongoBook> Gets(System.Linq.Expressions.Expression<Func<MongoBook, bool>> where, int pageSize, int pageIndex, out int objectTotal)
        {
            using (var db = CreateReadDb())
            {
                var query = db.GetCollection<MongoBook>(CollectionName).AsQueryable();

                if (where != null)
                    query = query.Where(where);
                objectTotal = query.Count();

                return query.OrderByDescending(o => o.Date).Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
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

            Assert.IsTrue(repository.Exist(o => o.Title == "Test Book 1"));

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
            bks = repository.Gets(10, 1, out objectTotal);

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
            bks = repository.Gets(o => o.Date > DateTime.Now.Date.AddDays(2), 10, 1, out objectTotal);

            Assert.AreEqual(27, objectTotal);

            repository.Clear();
        }

    }
}
