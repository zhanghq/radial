﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Threading.Tasks;
using Radial.Data.Nhs;
using NHibernate;
using Radial.Data.Nhs.Key;
using Radial.Data;

namespace Radial.UnitTest
{
    [TestFixture]
    public class SequentialKeyTest
    {

        [Test]
        public void NextKey()
        {
            HashSet<ulong> set = new HashSet<ulong>();
            Parallel.For(0, 100, i =>
            {
                using (IUnitOfWork uow = new NhUnitOfWork())
                {
                    ISequentialKeyBuilder builder = new DefaultSequentialKeyBuilder(new SqlServerSequentialKeyRepository(uow));

                    ulong v=builder.Next<string>();

                    Console.WriteLine("P{0}={1}", i, v);
                    set.Add(v);
                }
            });

            Assert.AreEqual(set.Count, 100);
        }
    }
}
