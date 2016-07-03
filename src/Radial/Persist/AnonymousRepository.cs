using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial.Persist
{

    /*
     * create In-Memory anonymous repository implementing the TRepository Interface.
     * TRepository must implement  Raidal.Persist.IRepository<> interface and no custom methods
     */

    /// <summary>
    /// AnonymousRepository
    /// </summary>
    /// <typeparam name="TRepository">The type of the repository.</typeparam>
    sealed class AnonymousRepository<TRepository> where TRepository:class
    {
        IUnitOfWorkEssential _uow;
        const string CodeTemp = "namespace {Namespace} {\r\n"
            + "   public class {RepositoryClassName} : {PersistenceNamespace}.BasicRepository<{TObjectType}>, {RepositoryInterfaceName} {\r\n"
            + "       public {RepositoryClassName}(Radial.Persist.IUnitOfWorkEssential uow) : base(uow) {}\r\n"
            + "   }\r\n"
            + "}";

        /// <summary>
        /// Initializes a new instance of the <see cref="AnonymousRepository{TRepository}"/> class.
        /// </summary>
        /// <param name="uow">The uow.</param>
        public AnonymousRepository(IUnitOfWorkEssential uow)
        {
            _uow = uow;
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <returns></returns>
        public TRepository GetInstance()
        {

            TRepository instance = default(TRepository);

            Type repoType = typeof(TRepository);

            Checker.Requires(repoType.IsInterface, "{0} must be an interface type", repoType.FullName);

            Type repoBaseInterfaceType = repoType.GetInterfaces().Where(o => o.FullName.StartsWith(typeof(IRepository<>).FullName)).FirstOrDefault();

            if (repoBaseInterfaceType != null)
            {
                var tobjType = repoBaseInterfaceType.GenericTypeArguments.FirstOrDefault();

                if (tobjType != null)
                {
                    string nsName ="N"+Guid.NewGuid().ToString("N").ToUpper();
                    string tobjTypeName = tobjType.FullName;
                    string repositoryInterfaceName = repoType.FullName;
                    string repositoryClassName = Toolkits.FirstCharUpperCase(repoType.Name.TrimStart('I'));
                    string repositoryClassFullName = string.Format("{0}.{1}", nsName, repositoryClassName);

                    string persistenceNamespace = _uow.GetType().Namespace;

                    string source = CodeTemp.Replace("{Namespace}", nsName)
                    .Replace("{RepositoryClassName}", repositoryClassName)
                    .Replace("{PersistenceNamespace}", persistenceNamespace)
                    .Replace("{TObjectType}", tobjTypeName)
                    .Replace("{RepositoryInterfaceName}", repositoryInterfaceName);


                    Microsoft.CSharp.CSharpCodeProvider objCSharpCodePrivoder = new Microsoft.CSharp.CSharpCodeProvider();
                    System.CodeDom.Compiler.CompilerParameters objCompilerParameters = new System.CodeDom.Compiler.CompilerParameters();
                    objCompilerParameters.ReferencedAssemblies.Add("System.Core.dll");

                    foreach (var ass in AppDomain.CurrentDomain.GetAssemblies())
                    {
                        if (ass.FullName == repoBaseInterfaceType.Assembly.FullName || ass.FullName == tobjType.Assembly.FullName
                            || ass.FullName == repoType.Assembly.FullName)
                        {
                            if (!objCompilerParameters.ReferencedAssemblies.Contains(ass.Location))
                                objCompilerParameters.ReferencedAssemblies.Add(ass.Location);
                        }
                    }
                    objCompilerParameters.GenerateExecutable = false;
                    objCompilerParameters.GenerateInMemory = true;
                    System.CodeDom.Compiler.CompilerResults cr = objCSharpCodePrivoder.CompileAssemblyFromSource(objCompilerParameters, source);
                    if (cr.Errors.Count == 0)
                        instance = Activator.CreateInstance(cr.CompiledAssembly.GetType(repositoryClassFullName), _uow) as TRepository;
                }
            }


            return instance;
        }
    }
}
