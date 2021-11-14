using System;

namespace Pokermon.Core.Model.Entities
{
    public class Player
    {
        public Guid Id { get; set; }

        public Player(Guid id)
        {
            Id = id;
        }
    }
}
