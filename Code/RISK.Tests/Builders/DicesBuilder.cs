using System.Collections.Generic;
using RISK.Application.Play.Attacking;

namespace RISK.Tests.Builders
{
    public class DicesBuilder
    {
        public Dices Build()
        {
            return new Dices(new List<int>(), new List<int>());
        }
    }
}