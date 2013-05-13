using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Radial;
using Radial.Boot;
using Radial.Persist;
using Radial.Persist.Nhs;
using Microsoft.Practices.Unity;
using BookNine.Domain.Repository;
using BookNine.Infrastructure.Persistence.Repository;

namespace BookNine.Application.Startup
{
    public class DefaultBootTask : IBootTask
    {
        public void Initialize()
        {
            //model conversion
            ModelConversion.RegisterMappers();

            //unit of work
            Components.Container.RegisterType<IUnitOfWork, ContextualUnitOfWork>(new InjectionConstructor());

            //repository
            Components.Container.RegisterType<IUserRepository, UserRepository>();
        }

        public void Start()
        {
            HibernateEngine.OpenAndBindSession();
        }

        public void Stop()
        {
            HibernateEngine.CloseAndUnbindSession();
        }
    }
}
