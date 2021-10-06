using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetsApi_Models.ViewModels {
    public class ViewWager {
        public Guid UserId { get; set; }
        public int FightId { get; set; }
        public int Amount { get; set; }
        public int FighterId { get; set; }
    }
}
