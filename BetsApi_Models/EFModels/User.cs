﻿using System;
using System.Collections.Generic;

#nullable disable

namespace BetsApi_Models.EFModels {
    public partial class User
    {
        public Guid UserId { get; set; }
        public int TotalCurrency { get; set; }
    }
}
