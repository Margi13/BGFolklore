using AutoMapper;
using BGFolklore.Data;
using System;

namespace BGFolklore.Services
{
    public abstract class BaseService
    {
        protected BaseService(ApplicationDbContext context,
            IMapper mapper)
        {
            this.Context = context;
            this.Mapper = mapper;
        }

        protected ApplicationDbContext Context { get; private set; }
        protected IMapper Mapper { get; private set; }
    }
}
