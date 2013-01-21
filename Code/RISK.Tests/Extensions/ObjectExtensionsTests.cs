using System.Linq;
using NUnit.Framework;
using RISK.Domain.Extensions;

namespace RISK.Tests.Extensions
{
    [TestFixture]
    public class ObjectExtensionsTests
    {
        [Test]
        public void AsList_creates_enumerable_with_single_element()
        {
            var someObject = new object();
            var actual = someObject.AsList();

            Assert.AreEqual(1, actual.Count(), "item count");
        }

        [Test]
        public void AsList_creates_enumerable_containing_original_object()
        {
            var someObject = new object();
            var actual = someObject.AsList();

            CollectionAssert.Contains(actual, someObject);
        }
    }
}