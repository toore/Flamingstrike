using NUnit.Framework;
using RISK.Base;

namespace RISK.Tests.Base.Extensions
{
    [TestFixture]
    public class MaybeTests
    {
        [Test]
        public void Nothing_has_no_value()
        {
            Assert.IsFalse(Maybe<int>.Nothing.HasValue);
        }

        [Test]
        public void Instantiated_maybe_has_value()
        {
            var maybe = new Maybe<int>(1);
            Assert.IsTrue(maybe.HasValue);
        }

        [Test]
        [ExpectedException(typeof(ForbiddenMaybeValueAccessException))]
        public void Throws_when_accessing_value_of_nothing()
        {
            var v = Maybe<int>.Nothing.Value;
        }

        [Test]
        public void Value_returns_given_value_for_instantiated_maybes()
        {
            var maybe = new Maybe<int>(1);
            Assert.AreEqual(1, maybe.Value);
        }
    }
}