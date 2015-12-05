﻿using NUnit.Framework;
using Radial.Persist;
using Radial.Persist.Efs;
using Radial.UnitTest.Persist.Efs.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial.UnitTest.Persist.Efs
{
    [TestFixture]
    public class EfUnitOfWorkTest : EfTestBase
    {

        /// <summary>
        /// Registerses this instance.
        /// </summary>
        [Test]
        public void Registers()
        {
            Assert.DoesNotThrow(() =>
            {
                using (IUnitOfWork uow = new EfUnitOfWork())
                {
                    var c = new Article { Id = Radial.TimingSeq.Next() };
                    uow.RegisterNew<Article>(c);
                    c.Content = "测试1";
                    uow.RegisterUpdate<Article>(c);
                    //uow.RegisterDelete<Article>(c);
                    //uow.RegisterDelete<Article, string>(c.Id);
                    //uow.RegisterClear<Article>();

                    uow.Commit();
                }
            });
        }
    }
}
