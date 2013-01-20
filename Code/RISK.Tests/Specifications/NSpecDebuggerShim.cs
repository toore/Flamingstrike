using System.Linq;
using System.Reflection;
using NSpec;
using NSpec.Domain;
using NUnit.Framework;

namespace RISK.Tests.Specifications
{
    [Ignore("Debugger shim method Debug makes no sense to be called from base class.")]
    public class NSpecDebuggerShim : nspec
    {
        [Test]
        public void Debug()
        {
            var classNameToDebug = GetType().Name;

            var invocation = new RunnerInvocation(Assembly.GetExecutingAssembly().Location, classNameToDebug);

            var contexts = invocation.Run();

            //assert that there aren't any failures
            contexts.Failures().Count().should_be(0);
        }
    }
}