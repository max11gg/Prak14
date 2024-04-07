using System;
using System.Collections.Generic;

namespace ApiTrello.Models
{
    public partial class Label
    {
        public int LabelId { get; set; }
        public string LabelName { get; set; } = null!;
        public string Color { get; set; } = null!;

    }
}
