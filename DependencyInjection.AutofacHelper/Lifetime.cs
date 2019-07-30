using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInjection.AutofacHelper
{
    public enum Lifetime
    {
        /// <summary>
        /// A singleton service. Atmost one instance will be created for all scopes
        /// </summary>
        Singleton = 0,

        /// <summary>
        /// A shared service inside a certain scope. Instance will be created per scope
        /// </summary>
        Scoped = 1,

        /// <summary>
        /// A transient service. New instance will be created for each request
        /// </summary>
        Transient = 2
    }
}
