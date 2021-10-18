using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetsApi_Models.ViewModels {
    public class ViewUser {
        /// <summary>
        /// The UserId is stored as a Guid and uniquely identifies a User from other microservices
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// The TotalCurrency is the currency that should be paid back or should be placed on a bet
        /// </summary>
        public int TotalCurrency { get; set; }
    }
}
