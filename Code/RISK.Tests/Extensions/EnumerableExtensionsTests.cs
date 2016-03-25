using System.Linq;
using FluentAssertions;
using Xunit;

namespace RISK.Tests.Extensions
{
    public class EnumerableExtensionsTests
    {
        [Fact]
        public void List_with_single_item_is_equivalent()
        {
            var item = new object();
            var first = new[] { item };
            var second = new[] { item };

            first.IsEquivalent(second).Should().BeTrue();
        }

        [Fact]
        public void List_with_single_item_is_not_equivalent()
        {
            var first = new[] { new object() };
            var second = new[] { new object() };

            first.IsEquivalent(second).Should().BeFalse();
        }

        [Fact]
        public void Empty_lists_are_equivalent()
        {
            var first = new object[] { };
            var second = new object[] { };

            first.IsEquivalent(second).Should().BeTrue();
        }

        [Fact]
        public void When_first_list_is_empty_is_not_equivalent()
        {
            var first = new object[] { };
            var second = new[] { new object() };

            first.IsEquivalent(second).Should().BeFalse();
        }

        [Fact]
        public void When_second_list_is_empty_is_not_equivalent()
        {
            var first = new[] { new object() };
            var second = new object[] { };

            first.IsEquivalent(second).Should().BeFalse();
        }
    }
}