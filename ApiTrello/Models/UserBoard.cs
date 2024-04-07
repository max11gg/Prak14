using System;
using System.Collections.Generic;

namespace ApiTrello.Models
{
    public partial class UserBoard
    {
        public int UserBoardId { get; set; }
        public int UserId { get; set; }
        public int BoardId { get; set; }

        public virtual Board Board { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
