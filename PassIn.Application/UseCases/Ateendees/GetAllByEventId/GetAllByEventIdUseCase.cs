using Microsoft.EntityFrameworkCore;
using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassIn.Application.UseCases.Ateendees.GetAllByEventId
{
    public class GetAllByEventIdUseCase
    {
        private readonly PassInDbContext _dbContext;
        public GetAllByEventIdUseCase()
        {
            _dbContext = new PassInDbContext();
        }
        public ResponseAllAttendeesJson Execute(Guid eventId)
        {
            var entity = _dbContext.Events.Include(ev => ev.Attendees).FirstOrDefault(ev => ev.Id == eventId);

            if (entity is null)
                throw new NotFoundException("An event with this id does not exist.");


            return new ResponseAllAttendeesJson
            {
                Attendees = entity.Attendees.Select(attendee => new ResponseAttendeeJson
                {
                    Id = attendee.Id,
                    Name = attendee.Name,
                    Email = attendee.Email,
                    CreatedAt = attendee.Created_At
                }).ToList()
            };
        }
    }
}
