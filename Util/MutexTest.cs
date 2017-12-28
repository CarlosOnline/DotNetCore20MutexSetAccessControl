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
