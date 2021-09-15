using AutoMapper;
using BGFolklore.Data;
using BGFolklore.Data.Models.Calendar;
using BGFolklore.Services.Public.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGFolklore.Services.Public
{
    public class StatusService : BaseService, IStatusService
    {
        public StatusService(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
        {
        }
        public Status GetStatus(int statusId)
        {
            var newStatus = this.Context.Status.Where(s => s.Id == statusId).FirstOrDefault();
            if (newStatus == null)
            {
                throw new Exception();
            }
            Status status = this.Mapper.Map<Status>(newStatus);
            return status;
        }
    }
}
