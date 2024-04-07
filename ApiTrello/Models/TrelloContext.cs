using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ApiTrello.Models
{
    public partial class TrelloContext : DbContext
    {
        public TrelloContext()
        {
        }

        public TrelloContext(DbContextOptions<TrelloContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Board> Boards { get; set; } = null!;
        public virtual DbSet<Card> Cards { get; set; } = null!;
        public virtual DbSet<CardLabel> CardLabels { get; set; } = null!;
        public virtual DbSet<Comment> Comments { get; set; } = null!;
        public virtual DbSet<Label> Labels { get; set; } = null!;
        public virtual DbSet<List> Lists { get; set; } = null!;
        public virtual DbSet<Task> Tasks { get; set; } = null!;
        public virtual DbSet<Token> Tokens { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserBoard> UserBoards { get; set; } = null!;
        public virtual DbSet<UserCard> UserCards { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=ALEXANDER-LAPTO\\SQLEXPRESS;Initial Catalog=Trello;Persist Security Info=True;User ID=sa;Password=123");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Board>(entity =>
            {
                entity.HasIndex(e => new { e.UserId, e.BoardName }, "UQ_BoardName_User")
                    .IsUnique();
                entity.Property(e => e.BoardId).HasColumnName("BoardID").ValueGeneratedOnAdd();

                entity.Property(e => e.BoardName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserId).HasColumnName("UserID");

           });

            modelBuilder.Entity<Card>(entity =>
            {
                entity.Property(e => e.CardId).HasColumnName("CardID").ValueGeneratedOnAdd(); ;

                entity.Property(e => e.CardDescription).HasColumnType("text");

                entity.Property(e => e.CardTitle)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Deadline).HasColumnType("datetime");

                entity.Property(e => e.ListId).HasColumnName("ListID");

          });

            modelBuilder.Entity<CardLabel>(entity =>
            {
                entity.HasIndex(e => new { e.CardId, e.LabelId }, "UQ__CardLabe__76692F352BE9AEF3")
                    .IsUnique();

                entity.Property(e => e.CardLabelId).HasColumnName("CardLabelID").ValueGeneratedOnAdd(); ;

                entity.Property(e => e.CardId).HasColumnName("CardID");

                entity.Property(e => e.LabelId).HasColumnName("LabelID");

          });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.Property(e => e.CommentId).HasColumnName("CommentID").ValueGeneratedOnAdd(); ;

                entity.Property(e => e.CardId).HasColumnName("CardID");

                entity.Property(e => e.CommentText).HasColumnType("text");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasColumnName("UserID");

            });

            modelBuilder.Entity<Label>(entity =>
            {
                entity.Property(e => e.LabelId).HasColumnName("LabelID").ValueGeneratedOnAdd(); ;

                entity.Property(e => e.Color)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.LabelName)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<List>(entity =>
            {
                entity.Property(e => e.ListId).HasColumnName("ListID").ValueGeneratedOnAdd(); ;

                entity.Property(e => e.BoardId).HasColumnName("BoardID");

                entity.Property(e => e.ListName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

           });

            modelBuilder.Entity<Task>(entity =>
            {
                entity.Property(e => e.TaskId).HasColumnName("TaskID").ValueGeneratedOnAdd(); ;

                entity.Property(e => e.CardId).HasColumnName("CardID");

                entity.Property(e => e.TaskDescription).HasColumnType("text");

          });

            modelBuilder.Entity<Token>(entity =>
            {
                entity.Property(e => e.TokenId).HasColumnName("token_id");

                entity.Property(e => e.Token1)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("token");

                entity.Property(e => e.TokenDatetime)
                    .HasColumnName("token_datetime")
                    .HasDefaultValueSql("(sysdatetime())");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.UserId, "UQ_UserID")
                    .IsUnique();

                entity.HasIndex(e => e.Email, "UQ__Users__A9D10534C4475256")
                    .IsUnique();
                entity.Property(e => e.FcmToken).HasMaxLength(255);

                entity.Property(e => e.UserId).HasColumnName("UserID").ValueGeneratedOnAdd(); ;

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.PasswordHash)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Salt)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UserBoard>(entity =>
            {
                entity.Property(e => e.UserBoardId).HasColumnName("UserBoardID").ValueGeneratedOnAdd(); ;

                entity.Property(e => e.BoardId).HasColumnName("BoardID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

           });

            modelBuilder.Entity<UserCard>(entity =>
            {
                entity.Property(e => e.UserCardId).HasColumnName("UserCardID").ValueGeneratedOnAdd(); ;

                entity.Property(e => e.CardId).HasColumnName("CardID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

           });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
