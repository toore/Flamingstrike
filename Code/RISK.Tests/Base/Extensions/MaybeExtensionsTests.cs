using NUnit.Framework;
using RISK.Base;
using RISK.Base.Extensions;

namespace RISK.Tests.Base.Extensions
{
    [TestFixture]
    public class MaybeExtensionsTests
    {
        [Test]
        public void ToMaybeWhenNotNull_returns_Nothing()
        {
            const object nullObject = null;
            var maybe = nullObject.ToMaybeWhenNotNull();

            Assert.AreEqual(Maybe<object>.Nothing, maybe);
        }

        [Test]
        public void ToMaybeWhenNotNull_returns_reference()
        {
            var nullObject = new object();
            var maybe = nullObject.ToMaybeWhenNotNull();

            Assert.IsTrue(maybe.HasValue);
            Assert.AreEqual(nullObject, maybe.Value);
        }
    }
}