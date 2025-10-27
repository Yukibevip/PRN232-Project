using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace DataAccessObjects;

public partial class CallioTestContext : DbContext
{
    public CallioTestContext()
    {
    }

    public CallioTestContext(DbContextOptions<CallioTestContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Accusation> Accusations { get; set; }

    public virtual DbSet<BlockList> BlockLists { get; set; }

    public virtual DbSet<FriendInvitation> FriendInvitations { get; set; }

    public virtual DbSet<FriendList> FriendLists { get; set; }

    public virtual DbSet<Log> Logs { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer(GetConnectionString());

    string GetConnectionString()
    {
        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json").Build();
        return config["ConnectionStrings:MyCallioDB"];
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Accusation>(entity =>
        {
            entity.Property(e => e.Category)
                .HasMaxLength(64)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ReviewAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(64)
                .IsUnicode(false);

            entity.HasOne(d => d.Accused).WithMany(p => p.AccusationAccuseds)
                .HasForeignKey(d => d.AccusedId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Accusations_Users1");

            entity.HasOne(d => d.Reported).WithMany(p => p.AccusationReporteds)
                .HasForeignKey(d => d.ReportedId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Accusations_Users");

            entity.HasOne(d => d.ReviewedByNavigation).WithMany(p => p.AccusationReviewedByNavigations)
                .HasForeignKey(d => d.ReviewedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Accusations_Users2");
        });

        modelBuilder.Entity<BlockList>(entity =>
        {
            entity.HasKey(e => e.BlockId);

            entity.ToTable("BlockList");

            entity.Property(e => e.ExpiresAt).HasColumnType("datetime");

            entity.HasOne(d => d.Blocked).WithMany(p => p.BlockListBlockeds)
                .HasForeignKey(d => d.BlockedId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BlockList_Users1");

            entity.HasOne(d => d.Blocker).WithMany(p => p.BlockListBlockers)
                .HasForeignKey(d => d.BlockerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BlockList_Users");
        });

        modelBuilder.Entity<FriendInvitation>(entity =>
        {
            entity.HasKey(e => e.InvitationId);

            entity.ToTable("FriendInvitation");

            entity.Property(e => e.SentAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Receiver).WithMany(p => p.FriendInvitationReceivers)
                .HasForeignKey(d => d.ReceiverId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FriendInvitation_Users1");

            entity.HasOne(d => d.Sender).WithMany(p => p.FriendInvitationSenders)
                .HasForeignKey(d => d.SenderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FriendInvitation_Users");
        });

        modelBuilder.Entity<FriendList>(entity =>
        {
            entity.HasKey(e => new { e.UserId1, e.UserId2 });

            entity.ToTable("FriendList");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.UserId1Navigation).WithMany(p => p.FriendListUserId1Navigations)
                .HasForeignKey(d => d.UserId1)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FriendList_Users");

            entity.HasOne(d => d.UserId2Navigation).WithMany(p => p.FriendListUserId2Navigations)
                .HasForeignKey(d => d.UserId2)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FriendList_Users1");
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.Property(e => e.Action)
                .HasMaxLength(128)
                .IsUnicode(false);
            entity.Property(e => e.ErrorCode)
                .HasMaxLength(128)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(128)
                .IsUnicode(false);
            entity.Property(e => e.TimeStamp)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.Logs)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Logs_Users");
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.Property(e => e.Content).IsUnicode(false);
            entity.Property(e => e.ReadAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.SentAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Receiver).WithMany(p => p.MessageReceivers)
                .HasForeignKey(d => d.ReceiverId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Messages_Users1");

            entity.HasOne(d => d.ReplyTo).WithMany(p => p.InverseReplyTo)
                .HasForeignKey(d => d.ReplyToId)
                .HasConstraintName("FK_Messages_Messages");

            entity.HasOne(d => d.Sender).WithMany(p => p.MessageSenders)
                .HasForeignKey(d => d.SenderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Messages_Users");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.UserId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.AvatarUrl).IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(256)
                .IsUnicode(false);
            entity.Property(e => e.FullName).HasMaxLength(128);
            entity.Property(e => e.Gender).HasMaxLength(64);
            entity.Property(e => e.Password)
                .HasMaxLength(128)
                .IsUnicode(false);
            entity.Property(e => e.UserRole)
                .HasMaxLength(64)
                .IsUnicode(false);
            entity.Property(e => e.Username).HasMaxLength(128);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
