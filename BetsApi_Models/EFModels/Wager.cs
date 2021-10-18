using System;
using System.Collections.Generic;

#nullable disable

namespace BetsApi_Models.EFModels
{
    public partial class Wager
    {
        /// <summary>
        /// Primary Key for a Wager Record
        /// </summary>
        public int WagerId { get; set; }
        /// <summary>
        /// The UserId is stored as a Guid and uniquely identifies a User with other microservices
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// The FightId uniquely identifies the Fight that the wager is placed on
        /// </summary>
        public int FightId { get; set; }
        /// <summary>
        /// Amount is the size of the wager
        /// </summary>
        public int Amount { get; set; }
        /// <summary>
        /// The FighterId identifies which Fighter the wager is placed on
        /// </summary>
        public int FighterId { get; set; }
    }
}
