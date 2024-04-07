using System;
using System.Collections.Generic;

namespace ApiTrello.Models
{
    public partial class List
    {
        public int ListId { get; set; }
        public string ListName { get; set; } = null!;
        public int BoardId { get; set; }

        public virtual Board? Board { get; set; } = null!;
    }
}
