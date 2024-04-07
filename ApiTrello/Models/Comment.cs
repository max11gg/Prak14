using System;
using System.Collections.Generic;

namespace ApiTrello.Models
{
    public partial class Comment
    {
        public int CommentId { get; set; }
        public int CardId { get; set; }
        public int UserId { get; set; }
        public string CommentText { get; set; } = null!;
        public DateTime CreatedDate { get; set; }

        public virtual Card? Card { get; set; } = null!;
        public virtual User? User { get; set; } = null!;
    }
}
