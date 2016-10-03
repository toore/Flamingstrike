using RISK.GameEngine;

namespace Tests.RISK.GameEngine.Builders
{
    public class PlayerBuilder
    {
        private string _name = "default";

        public IPlayer Build()
        {
            return new Player(_name);
        }

        public PlayerBuilder Name(string name)
        {
            _name = name;
            return this;
        }
    }
}