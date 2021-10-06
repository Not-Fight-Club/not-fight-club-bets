using System;
using System.Collections.Generic;

#nullable disable

namespace BetsApi_Data.Models
{
    public partial class User
    {
        public Guid UserId { get; set; }
        public int TotalCurrency { get; set; }
    }
}
