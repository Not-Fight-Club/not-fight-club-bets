using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetsApi_Models.ViewModels {
    public class ViewWager {
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
