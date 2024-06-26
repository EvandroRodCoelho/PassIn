﻿using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;
using PassIn.Infrastructure.Entities;

namespace PassIn.Application.UseCases.Checkins.DoCheckin
{
    public class DoAttendeeCheckinUseCase
    {
        private readonly PassInDbContext _dbContext;
        public DoAttendeeCheckinUseCase()
        {
            _dbContext = new PassInDbContext();
        }
        public ResponseRegisteredJson Execute(Guid attendeeId)
        {
            Validate(attendeeId);
            var entity = new CheckIn
            {
                Attendee_Id = attendeeId,
                Created_At = DateTime.UtcNow,
            };
            _dbContext.CheckIns.Add(entity);
            _dbContext.SaveChanges();
            return new ResponseRegisteredJson { 
                Id = entity.Id
            };
        }

        private void Validate(Guid attendeeId)
        {
           var existAttendee = _dbContext.Attendees.Any(ateendee => ateendee.Id == attendeeId);

            if (!existAttendee) {
                throw new NotFoundException("The attendee with this Id was not found.");
            }

            var existCheckIn = _dbContext.CheckIns.Any(ch => ch.Attendee_Id == attendeeId);
            if (existCheckIn)
            {
                throw new ConfictException("Attendee can not do checking twice in the same event");
            }
        }
    }
}
 