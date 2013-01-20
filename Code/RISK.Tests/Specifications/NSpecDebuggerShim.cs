using System.Linq;
using System.Reflection;
using NSpec;
using NSpec.Domain;
using NUnit.Framework;

namespace RISK.Tests.Specifications
{
    public class NSpecDebuggerShim : nspec
    {
        [Test]
        public void Debug()
        {
            var tagOrClassName = this.GetType().Name;

            if (tagOrClassName == "NSpecDebuggerShim")
            {
                Assert.Ignore("Debugger shim method Debug makes no sense to be called from base class.");
            }

            var invocation = new RunnerInvocation(Assembly.GetExecutingAssembly().Location, tagOrClassName);

            var contexts = invocation.Run();

            //assert that there aren't any failures
            contexts.Failures().Count().should_be(0);
        }
    }
}