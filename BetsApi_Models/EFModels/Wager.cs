using System;
using System.Collections.Generic;

#nullable disable

namespace BetsApi_Data.Models
{
    public partial class Wager
    {
        public int WagerId { get; set; }
        public Guid UserId { get; set; }
        public int FightId { get; set; }
        public int Amount { get; set; }
        public int FighterId { get; set; }
    }
}
