
namespace MiniFB.Infrastructure.Entities
{
   public class Post
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string ImageUrl { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
