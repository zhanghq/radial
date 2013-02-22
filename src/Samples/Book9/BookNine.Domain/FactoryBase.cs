using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookNine.Infrastructure;
using Radial.Persist;

namespace BookNine.Domain
{
    public abstract class FactoryBase
    {
        public FactoryBase(IUnitOfWorkEssential uow)
        {
            UnitOfWorkEssential = uow;
        }

        protected IUnitOfWorkEssential UnitOfWorkEssential { get; set; }


        protected TRepository CreateRepository<TRepository>() where TRepository : class
        {
            return DependencyResolver.CreateRepository<TRepository>(UnitOfWorkEssential);
        }

    }
}
