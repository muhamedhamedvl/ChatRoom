
using Microsoft.EntityFrameworkCore;
namespace ChatRoom.Models.Data
{
    public class ChatRoomContext : DbContext
    {
        public ChatRoomContext(DbContextOptions<ChatRoomContext> options) : base(options)
        {
        }
        public DbSet<ChatMessage> ChatMessages { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
