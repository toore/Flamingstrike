using RISK.Core;

namespace RISK.Tests.Builders
{
    public class PlayerBuilder
    {
        public IPlayer Build()
        {
            return new Player("default");
        }
    }
}