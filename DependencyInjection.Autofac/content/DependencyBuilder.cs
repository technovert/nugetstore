using DependencyInjection.AutofacHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autofac
{
    public class DependencyBuilder
{
    public static void Init()
    {
        AutofacDIHelper.Init(typeof(DependencyBuilder).Assembly);


        /*dependency registration - START*/

        //AutofacDIHelper.Register<MyService>();
        //AutofacDIHelper.RegisterAs<MyService, IMyService>();
        //AutofacDIHelper.RegisterAs<MyService, IMyService>(Lifetime.Scoped);
        //AutofacDIHelper.RegisterAs<MyService, IMyService>(Lifetime.Singleton);
        //AutofacDIHelper.RegisterAs<MyService, IMyService>(Lifetime.Transient);
        
        /*dependency registration - END*/

        AutofacDIHelper.Build();
    }
}
}
