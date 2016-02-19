using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial.Persist.Nhs
{
    /// <summary>
    /// ExtensionMethods
    /// </summary>
    public static class ExtensionMethods
    {
        #region NHibernate.Cfg.Configuration

        ///// <summary>
        ///// Adds class semiauto mapper.
        ///// </summary>
        ///// <param name="cfg">The CFG.</param>
        ///// <param name="classType">Type of the class.</param>
        //public static void AddSemiautoMapper(this NHibernate.Cfg.Configuration cfg, Type classType)
        //{
        //    if (cfg == null)
        //        return;

        //    new SemiautoMapper(cfg).Add(classType);
        //}

        ///// <summary>
        ///// Adds class semiauto mapper.
        ///// </summary>
        ///// <typeparam name="TClass">The type of the class.</typeparam>
        ///// <param name="cfg">The CFG.</param>
        //public static void AddSemiautoMapper<TClass>(this NHibernate.Cfg.Configuration cfg) where TClass:class
        //{
        //    if (cfg == null)
        //        return;

        //    new SemiautoMapper(cfg).Add<TClass>();
        //}

        ///// <summary>
        ///// Adds classes semiauto mapper.
        ///// </summary>
        ///// <param name="cfg">The CFG.</param>
        ///// <param name="classTypes">Type of the class.</param>
        //public static void AddSemiautoMapper(this NHibernate.Cfg.Configuration cfg, Type [] classTypes)
        //{
        //    if (cfg == null)
        //        return;

        //    new SemiautoMapper(cfg).Add(classTypes);
        //}

        ///// <summary>
        ///// Adds class types to mapper in the specified assembly.
        ///// </summary>
        ///// <param name="cfg">The CFG.</param>
        ///// <param name="assembly">The assembly.</param>
        ///// <param name="filter">The class type filter.</param>
        //public static void AddSemiautoMapper(this NHibernate.Cfg.Configuration cfg, System.Reflection.Assembly assembly, Func<Type, bool> filter)
        //{
        //    if (cfg == null)
        //        return;

        //    new SemiautoMapper(cfg).Add(assembly, filter);
        //}

        ///// <summary>
        ///// Adds class types to mapper in the specified assemblies.
        ///// </summary>
        ///// <param name="cfg">The CFG.</param>
        ///// <param name="assemblies">The assemblies.</param>
        ///// <param name="filter">The class type filter.</param>
        //public static void AddSemiautoMapper(this NHibernate.Cfg.Configuration cfg, System.Reflection.Assembly[] assemblies, Func<Type, bool> filter)
        //{
        //    if (cfg == null)
        //        return;

        //    new SemiautoMapper(cfg).Add(assemblies, filter);
        //}

        #endregion
    }
}
