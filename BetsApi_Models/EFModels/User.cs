using System;
using System.Collections.Generic;

#nullable disable

namespace BetsApi_Models.EFModels {
    public partial class User
    {
        /// <summary>
        /// The UserId is stored as a Guid and uniquely identifies a User from other microservices
        /// </summary>
        public Guid UserId { get; set; }
        public int TotalCurrency { get; set; }
    }
}
