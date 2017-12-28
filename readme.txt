dotnet new --install "Microsoft.AspNetCore.SpaTemplates"
dotnet new aurelia
npm intstall


# System.Threading.AccessControl won't load from asp.net Core 2.0 (netcoreapp2.0) website

Referencing .net 4.5.1 framework class library from asp.net Core 2.0 (netcoreapp2.0), where class library references System.Threading.AccessControl leads to receives FileNotFound exception.

**System.IO.FileNotFoundException** occurred
  HResult=0x80070002
  Message=Could not load file or assembly '**System.Threading.AccessControl**, Version=4.0.2.0,
```
Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'. The system cannot find the file specified.
  Source=Util
  StackTrace:
   at Util.MutexTest..ctor(String name) in C:\temp\projects\DotNetCore20MutexSetAccessControl\Util\MutexTest.cs:line 18
   at DotNetCore20MutexSetAccessControl.Controllers.HomeController.Index() in C:\temp\projects\DotNetCore20MutexSetAccessControl\spa\Controllers\HomeController.cs:line 11
   at Microsoft.Extensions.Internal.ObjectMethodExecutor.Execute(Object target, Object[] parameters)
   at Microsoft.AspNetCore.Mvc.Internal.ControllerActionInvoker.<InvokeActionMethodAsync>d__12.MoveNext()
```

# Repro
Problem solution uploaded to [DotNetCore20MutexSetAccessControl](https://github.com/CarlosOnline/DotNetCore20MutexSetAccessControl.git)

# Manual Repro
dotnet new --install "Microsoft.AspNetCore.SpaTemplates"
dotnet new aurelia
npm intstall
add new Utils .Net Framework 4.51 project (version doesn't matter)
add class below to Utils .Net Framework 4.51

```
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;

namespace Util
{
    public class MutexTest
    {
        public MutexTest(string name)
        {
            var mutexId = $"Global\\{name}";
            var mutex = new Mutex(false, mutexId);

            var allowEveryoneRule = new MutexAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), MutexRights.FullControl, AccessControlType.Allow);
            var securitySettings = new MutexSecurity();
            securitySettings.AddAccessRule(allowEveryoneRule);
            mutex.SetAccessControl(securitySettings);
        }
    }
}
```

add Utils project reference to asp.net core app project
update HomeController.cs
```
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Util;

namespace DotNetCore20MutexSetAccessControl.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var mutexTest = new MutexTest("MyTest");
            return View();
        }

        public IActionResult Error()
        {
            ViewData["RequestId"] = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            return View();
        }
    }
}
```
start debugging
