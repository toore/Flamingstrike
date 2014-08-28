using Caliburn.Micro;
using FluentAssertions;
using GuiWpf.Infrastructure;
using NSubstitute;
using StructureMap;
using Xunit;

namespace RISK.Tests.GuiWpf.Infrastructure
{
    public class HandleInterceptorTests
    {
        private HandleInterceptor<IFakeEventAggregator> _handleInterceptor;

        public HandleInterceptorTests()
        {
            _handleInterceptor = new HandleInterceptor<IFakeEventAggregator>();
        }

        [Fact]
        public void Should_match_type()
        {
            _handleInterceptor.MatchesType(typeof(FakeMessageHandler)).Should().BeTrue();
        }

        [Fact]
        public void Should_not_match_type()
        {
            _handleInterceptor.MatchesType(typeof(object)).Should().BeFalse();
        }

        [Fact]
        public void Subscribes_to_event_aggregator()
        {
            var eventAggregator = Substitute.For<IFakeEventAggregator>();
            var messageHandler = new FakeMessageHandler();
            ObjectFactory.Configure(x =>
            {
                x.For<IFakeEventAggregator>().Use(eventAggregator);
                x.For<FakeMessageHandler>().Use(messageHandler);
                x.RegisterInterceptor(_handleInterceptor);
            });

            ObjectFactory.GetInstance<FakeMessageHandler>();

            eventAggregator.Received(1).Subscribe(messageHandler);
        }
    }

    public interface IFakeEventAggregator : IEventAggregator {}

    public class FakeEventAggregator : EventAggregator, IFakeEventAggregator {}

    public class FakeMessage {}

    public class FakeMessageHandler : IHandle<FakeMessage>
    {
        public void Handle(FakeMessage message)
        {
            Message = message;
        }

        public FakeMessage Message { get; set; }
    }
}