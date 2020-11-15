using System.Collections.Generic;

namespace MiniFB.Infrastructure.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public byte[] Password { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string PhoneNumber { get; set; }
        public virtual ICollection<Post> UserPosts { get; set; }
        public virtual ICollection<UserFriends> UserFriends { get; set; }
        public virtual ICollection<UserFriends> FriendsUsers { get; set; }
    }
}
