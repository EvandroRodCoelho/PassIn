using Microsoft.EntityFrameworkCore;
using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassIn.Application.UseCases.Events.GetById
{
    public class GetEventByIdUseCase
    {
        public  ResponseEventJson Execute(Guid id)
        {
            var dbContext = new PassInDbContext();

            var entity = dbContext.Events.FirstOrDefault(e => e.Id == id);

            if (entity is null)
                throw new NotFoundException("An event with this id does not exist.");

            return new ResponseEventJson
            {
                Id = entity.Id,
                Title = entity.Title,
                Details = entity.Details,
                MaximumAttendees = entity.Maximum_Attendees,
                AttendeesAmount = -1
            };

        }
    }
}
