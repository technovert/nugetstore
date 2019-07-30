using DependencyInjection.AutofacHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace $rootnamespace$
{
    public class DependencyBuilder
    {
        public static void Init()
        {
            AutofacDIHelper.Init(typeof(DependencyBuilder).Assembly);


            /*dependency registration - START*/
            
            /*dependency registration - END*/

            AutofacDIHelper.Build();
        }
    }
}
