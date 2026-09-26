using System;
using Microsoft.EntityFrameworkCore;
using MessagingApp.Entities.Concrete;

namespace MessagingApp.DataAccess.Concrete.EntityFramework
{

    public class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Conversation> Conversations { get; set; } = null!;

        public DbSet<Message> Messages { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

                optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=MessagingAppDb;Username=postgres;Password=postgres");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Conversation>(entity =>
            {
                entity.ToTable("Conversations");

                entity.HasKey(c => c.Id);

                entity.Property(c => c.Title)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(c => c.CreatedAt)
                    .IsRequired();

                entity.HasMany(c => c.Messages)
                    .WithOne(m => m.Conversation)
                    .HasForeignKey(m => m.ConversationId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.ToTable("Messages");

                entity.HasKey(m => m.Id);

                entity.Property(m => m.Sender)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(m => m.Content)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(m => m.SentAt)
                    .IsRequired();

                entity.HasIndex(m => m.ConversationId);

                entity.HasIndex(m => m.SentAt);
            });
        }
    }
}
