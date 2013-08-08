using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XEO.Core
{
    public class Factory<Type> 
    {
        private static CompositionContainer Container;

        static Factory()
        {
            AggregateCatalog catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(Type).Assembly));
            Container = new CompositionContainer(catalog);
        }

        public static Type Instance
        {
            get
            {
                return Container.GetExport<Type>().Value;
            }
        }

        //public static Type Instance(string contractName)
        //{
        //    return Container.GetExport<Type>(contractName).Value;
        //}
    }
}
