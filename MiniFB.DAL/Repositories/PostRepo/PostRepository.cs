using MiniFB.DAL.Repositories.PostRepo;
using MiniFB.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniFB.DAL.Repositories.PostRepository
{
    class PostRepository: Repository<Post>, IPostRepository
    {
        public PostRepository(IMiniFbContext context): base(context)
        {

        }
    }
}
