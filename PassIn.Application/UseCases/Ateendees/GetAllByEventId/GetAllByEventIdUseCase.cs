using Microsoft.EntityFrameworkCore;
using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;
using System.Globalization;

public class GetAllByEventIdUseCase
{
    private readonly PassInDbContext _dbContext;
    public GetAllByEventIdUseCase()
    {
        _dbContext = new PassInDbContext();
    }
    public ResponseAllAttendeesJson Execute(Guid eventId, string nameFilter = null, int pageSize = 10, int pageIndex = 0)
    {
        var entity = _dbContext.Events
            .Include(ev => ev.Attendees)
                .ThenInclude(attendee => attendee.CheckIn)
            .FirstOrDefault(ev => ev.Id == eventId);

        if (entity is null)
            throw new NotFoundException("An event with this id does not exist.");

        var filteredAttendees = entity.Attendees
            .Where(attendee => string.IsNullOrEmpty(nameFilter) || attendee.Name.Contains(nameFilter));

        var paginatedAttendees = filteredAttendees
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .Select(attendee => new ResponseAttendeeJson
            {
                Id = attendee.Id,
                Name = attendee.Name,
                Email = attendee.Email,
                CreatedAt = attendee.Created_At,
                CheckedInAt = attendee.CheckIn?.Created_At
            }).ToList();

        int totalItems = filteredAttendees.Count();
        double totalItemsDouble = totalItems;
        int totalPages = (int)Math.Ceiling(totalItemsDouble / pageSize);

        return new PaginatedResponseAllAttendeesJson
        {
            Attendees = paginatedAttendees,
            PageIndex = pageIndex,
            TotalPages = totalPages,
            ItensCount = totalItems
        };
    }
}
