using Microsoft.EntityFrameworkCore;
using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;

namespace PassIn.Application.UseCases.Events.Attendees.GetAllByEventId
{
    public class GetAllAttendeesByEventIdUseCase
    {
        private readonly PassInDbContext _dbContext;

        public GetAllAttendeesByEventIdUseCase()
        {
            _dbContext = new PassInDbContext();
        }

        public ResponseAllAttendeesJson Execute(Guid eventId)
        {
            var entity = _dbContext.Events.Include(ev => ev.Attendees).ThenInclude(att => att.CheckIn).FirstOrDefault(ev => ev.Id == eventId);
            if (entity is null)
            {
                throw new NotFoundException("An event with this id does not exist.");
            }
            return new ResponseAllAttendeesJson()
            {
                Attendees = entity.Attendees.Select(att => new ResponseAttendeeJson
                {
                    Id = att.Id,
                    Name = att.Name,
                    Email = att.Email,
                    CreatedAt = att.Created_At,
                    CheckedInAt = att.CheckIn?.Created_at
                }).ToList()
            };
        }
    }
}
