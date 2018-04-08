using System;

namespace FlamingStrike.GameEngine
{
    public class PlayerName : IEquatable<PlayerName>
    {
        private readonly string _name;

        public PlayerName(string name)
        {
            _name = name;
        }

        public static explicit operator string(PlayerName playerName)
        {
            return playerName._name;
        }

        public static bool operator ==(PlayerName a, PlayerName b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(PlayerName a, PlayerName b)
        {
            return !(a == b);
        }

        public bool Equals(PlayerName other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(_name, other._name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PlayerName)obj);
        }

        public override int GetHashCode()
        {
            return (_name != null ? _name.GetHashCode() : 0);
        }
    }
}