using System;
using System.Collections.Generic;

namespace ApiTrello.Models
{
    public partial class Card
    {
        public int CardId { get; set; }
        public string CardTitle { get; set; } = null!;
        public string? CardDescription { get; set; }
        public int ListId { get; set; }
        public DateTime? Deadline { get; set; }

        public virtual List? List { get; set; } = null!;
    }
}
