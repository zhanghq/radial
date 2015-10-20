﻿using NUnit.Framework;

namespace Radial.UnitTest
{
    [TestFixture]
    public class PlainTextConfigTest
    {
        [Test]
        public void Test1()
        {
            string text = "abc=sdfer";

            var cfg = PlainTextConfig.LoadFromText(text);

            Assert.AreEqual("sdfer", cfg["abc"]);


            text = "abc=\"sdfer\"";

            cfg = PlainTextConfig.LoadFromText(text);

            Assert.AreEqual("sdfer", cfg["abc"]);

            text = "abc='sdfer'";

            cfg = PlainTextConfig.LoadFromText(text);

            Assert.AreEqual("sdfer", cfg["abc"]);

            text = "abc='12\"sd\"'";

            cfg = PlainTextConfig.LoadFromText(text);

            Assert.AreEqual("12\"sd\"", cfg["abc"]);

            text = "abc=\"12'sd'\"";

            cfg = PlainTextConfig.LoadFromText(text);

            Assert.AreEqual("12'sd'", cfg["abc"]);

        }
    }
}
