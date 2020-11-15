using System.ComponentModel.DataAnnotations.Schema;

namespace MiniFB.Infrastructure.Entities
{
    [Table("UserFriends")]
    public class UserFriends
    {
        [ForeignKey("User")]
        public int UserId { get; set; }
        [ForeignKey("Friend")]
        public int FriendId { get; set; }
        public User User { get; set; }
        public User Friend { get; set; }
    }
}
