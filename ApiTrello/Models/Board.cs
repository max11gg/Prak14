using System;
using System.Collections.Generic;

namespace ApiTrello.Models
{
    public partial class Board
    {
        public int BoardId { get; set; }
        public string BoardName { get; set; } = null!;
        public int UserId { get; set; }

        public virtual User? User { get; set; } = null!;
    }
}
