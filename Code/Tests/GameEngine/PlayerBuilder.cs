using FlamingStrike.GameEngine;
using FlamingStrike.GameEngine.Play;

namespace Tests.GameEngine
{
    public class PlayerBuilder
    {
        private string _name = "default";

        public PlayerName Build()
        {
            return new PlayerName(_name);
        }

        public PlayerBuilder Name(string name)
        {
            _name = name;
            return this;
        }
    }
}