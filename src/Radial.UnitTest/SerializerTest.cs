using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Radial.Serialization;
using Radial.Serialization.Converters;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Radial.UnitTest
{
    public class Movie
    {
        public string Title
        { get; set; }

        public int Rating
        { get; set; }

        public MovieLevel? Level { get; set; }

        [JsonConverter(typeof(BooleanJsonConverter))]
        public bool? IsNew { get; set; }

        [JsonConverter(typeof(DateTimeJsonConverter))]
        public DateTime? ReleaseDate
        { get; set; }
    }

    public class Movie2
    {
        public string Title
        { get; set; }

        public int Rating
        { get; set; }

        public MovieLevel? Level { get; set; }

        public bool IsNew { get; set; }

        [JsonConverter(typeof(UnixTimeJsonConverter))]
        public DateTime? ReleaseDate
        { get; set; }
    }

    public enum MovieLevel
    {
        A,
        B
    }

    [Serializable]
    public class Movie3
    {
        public string Title
        { get; set; }

        public int Rating
        { get; set; }
    }

    [TestFixture]
    public class SerializerTest
    {
        [Test]
        public void Xml()
        {
            var o = new Movie { Title = "a3d", Level = MovieLevel.A, IsNew = false, ReleaseDate = DateTime.Now };
            string s = Radial.Serialization.XmlSerializer.Serialize<Movie>(o);
            Console.WriteLine(s);
            Assert.IsNotNull(Radial.Serialization.XmlSerializer.Deserialize<Movie>(s));
        }

        [Test]
        public void Json()
        {
            var o = new Movie { Title = "a3d", Level = MovieLevel.A, IsNew = null, ReleaseDate = DateTime.Now };
            
            string s = Radial.Serialization.JsonSerializer.Serialize(o);
            Console.WriteLine(s);
            var o2 = Radial.Serialization.JsonSerializer.Deserialize<Movie>(s);
            Assert.AreEqual(o.ReleaseDate.Value.ToString("yyyy/MM/dd HH:mm:ss"), o2.ReleaseDate.Value.ToString("yyyy/MM/dd HH:mm:ss"));
            Assert.AreEqual(o.IsNew, o2.IsNew);

            var o3 = new Movie2 { Title = "a3d", Level = MovieLevel.A, IsNew = true };

            string s2 = Radial.Serialization.JsonSerializer.Serialize(o3);
            Console.WriteLine(s2);
            var o4 = Radial.Serialization.JsonSerializer.Deserialize<Movie2>(s2);
            Assert.AreEqual(o3.ReleaseDate, o4.ReleaseDate);
            Assert.AreEqual(o3.IsNew, o4.IsNew);
        }

        [Test]
        public void Binary()
        {
            Movie3 o = new Movie3 { Title = "测试", Rating = 100 };

            byte[] bytes = BinarySerializer.Serialize(o);

            Movie3 o2 = (Movie3)BinarySerializer.Deserialize(bytes);

            Assert.AreEqual(o.Title, o2.Title);
        }
    }
}
