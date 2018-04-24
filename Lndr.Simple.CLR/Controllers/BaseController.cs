using System;
using System.Reflection;

namespace Lndr.Simple.CLR.Controllers
{
    public class BaseController
    {
        public BaseController()
        {
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler((sender, args) => {
                var resourceName = new AssemblyName(args.Name).Name;
                var fullName = string.Format("Lndr.Simple.CLR.libs.{0}.dll", resourceName);
                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(fullName))
                {
                    var buffer = new byte[(int)stream.Length];
                    stream.Read(buffer, 0, (int)stream.Length);
                    return Assembly.Load(buffer);
                }
            });
        }
    }
}
