using MiniFB.DAL.Repositories.AuthenticationRepo;
using MiniFB.DAL.Repositories.PostRepo;
using MiniFB.DAL.Repositories.PostRepository;
using MiniFB.DAL.Repositories.UserFriendsRepo;
using MiniFB.DAL.Repositories.UserRepo;
using System;


namespace MiniFB.DAL.UnitofWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly IMiniFbContext context;
        private IUserRepository userRepository;
        private IUserFriendsRepository userFriendsRepository;
        private IPostRepository postRepository;
        private IAuthentication authentication;
        private bool disposed = false; 
        public UnitOfWork(IMiniFbContext miniFbContext)
        {
            context = miniFbContext; 
        }
        public IUserRepository UserRepository => this.userRepository = userRepository ?? new UserRepository(context);

        public IPostRepository PostRepository =>this.postRepository = postRepository ?? new PostRepository(context);

        public IUserFriendsRepository UserFriendsRepository => this.userFriendsRepository = userFriendsRepository ?? new UserFriendsRepository(context);

        public IAuthentication Authentication =>this.authentication = authentication ?? new Authentication(context, new UnitOfWork(context));

        public bool SaveChanges()
        {
            var count = context.SaveChanges();
            return count > 0;
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
