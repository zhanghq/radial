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
     * IUnitOfWorkEssential instance must be Nhs.UnitOfWork or Efs.UnitOfWork
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
        /// Gets the type of the instance.
        /// </summary>
        /// <returns></returns>
        public Type GetInstanceType()
        {

            Type repoType = typeof(TRepository);

            Checker.Requires(repoType.IsInterface, "{0} must be an interface type", repoType.FullName);

            Type repoBaseInterfaceType = repoType.GetInterfaces().Where(o => o.FullName.StartsWith(typeof(IRepository<>).FullName)).FirstOrDefault();

            if (repoBaseInterfaceType != null)
            {
                var tobjType = repoBaseInterfaceType.GenericTypeArguments.FirstOrDefault();

                if (tobjType != null)
                {
                    string nsName = "N" + Guid.NewGuid().ToString("N").ToUpper();
                    string tobjTypeName = tobjType.FullName;
                    string repositoryInterfaceName = repoType.FullName;
                    string repositoryClassName = Toolkits.FirstCharUpperCase(repoType.Name.TrimStart('I'));
                    string repositoryClassFullName = string.Format("{0}.{1}", nsName, repositoryClassName);

                    var uowType = _uow.GetType();

                    Checker.Requires(uowType.FullName == "Radial.Persist.Efs.UnitOfWork" || uowType.FullName == "Radial.Persist.Nhs.UnitOfWork"
                        , "unit of work instance must be Radial.Persist.Efs.UnitOfWork or Radial.Persist.Nhs.UnitOfWork");

                    string source = CodeTemp.Replace("{Namespace}", nsName)
                    .Replace("{RepositoryClassName}", repositoryClassName)
                    .Replace("{PersistenceNamespace}", uowType.Namespace)
                    .Replace("{TObjectType}", tobjTypeName)
                    .Replace("{RepositoryInterfaceName}", repositoryInterfaceName);


                    Microsoft.CSharp.CSharpCodeProvider objCSharpCodePrivoder = new Microsoft.CSharp.CSharpCodeProvider();
                    System.CodeDom.Compiler.CompilerParameters objCompilerParameters = new System.CodeDom.Compiler.CompilerParameters();
                    objCompilerParameters.ReferencedAssemblies.Add("System.Core.dll");

                    if (!objCompilerParameters.ReferencedAssemblies.Contains(repoBaseInterfaceType.Assembly.Location))
                        objCompilerParameters.ReferencedAssemblies.Add(repoBaseInterfaceType.Assembly.Location);
                    if (!objCompilerParameters.ReferencedAssemblies.Contains(tobjType.Assembly.Location))
                        objCompilerParameters.ReferencedAssemblies.Add(tobjType.Assembly.Location);
                    if (!objCompilerParameters.ReferencedAssemblies.Contains(repoType.Assembly.Location))
                        objCompilerParameters.ReferencedAssemblies.Add(repoType.Assembly.Location);

                    objCompilerParameters.GenerateExecutable = false;
                    objCompilerParameters.GenerateInMemory = true;
                    System.CodeDom.Compiler.CompilerResults cr = objCSharpCodePrivoder.CompileAssemblyFromSource(objCompilerParameters, source);
                    if (cr.Errors.Count == 0)
                        return cr.CompiledAssembly.GetType(repositoryClassFullName);
                }
            }


            return null;
        }
    }
}
