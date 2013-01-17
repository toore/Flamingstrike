using System.Collections.Generic;
using RISK.Domain.Entities;

namespace RISK.Domain
{
    public class Game
    {
        private readonly List<IUser> users = new List<IUser>();

        public void AddUser(IUser user)
        {
            users.Add(user);
        }
    }
}