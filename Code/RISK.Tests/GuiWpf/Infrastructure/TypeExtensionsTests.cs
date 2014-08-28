using System;
using FluentAssertions;
using Xunit;
using TypeExtensions = GuiWpf.Infrastructure.TypeExtensions;

namespace RISK.Tests.GuiWpf.Infrastructure
{
    
    public class TypeExtensionsTests
    {
        [Fact]
        public void Implements_interface()
        {
            Implements<IA>(typeof(A)).Should().BeTrue();
        }

        [Fact]
        public void Does_not_implement()
        {
            Implements<IA>(typeof(C)).Should().BeFalse();
        }

        [Fact]
        public void Derived_class_implements()
        {
            Implements<IA>(typeof(B)).Should().BeTrue();
        }

        private static bool Implements<T>(Type type)
        {
            return TypeExtensions.Implements<T>(type);
        }

        private interface IA {}

        private class A : IA {}

        private class B : A {}

        private class C {}
    }
}