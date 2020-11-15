using AutoMapper;
using MiniFB.BAL.PostManager;
using MiniFB.BAL.UserManager;

namespace MiniFB.BAL.Mappings
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            UserService.Mapping(this);
            PostService.Mapping(this);
        }
    }
}
