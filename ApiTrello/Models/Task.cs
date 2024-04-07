using System;
using System.Collections.Generic;

namespace ApiTrello.Models
{
    public partial class Task
    {
        public int TaskId { get; set; }
        public int CardId { get; set; }
        public string TaskDescription { get; set; } = null!;
        public bool IsCompleted { get; set; }

        public virtual Card Card { get; set; } = null!;
    }
}
