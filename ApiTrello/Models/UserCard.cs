using System;
using System.Collections.Generic;

namespace ApiTrello.Models
{
    public partial class UserCard
    {
        public int UserCardId { get; set; }
        public int UserId { get; set; }
        public int CardId { get; set; }

        public virtual Board? Card { get; set; } = null!;
        public virtual User? User { get; set; } = null!;
    }
}
