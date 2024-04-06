using PassIn.Communication.Requests;
using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;
using System.Net.Mail;

namespace PassIn.Application.UseCases.Events.RegisterAttendee
{
    public class RegisterAttedeeOnEventUseCase
    {
        private readonly PassInDbContext _dbContext;

        public RegisterAttedeeOnEventUseCase()
        {
            _dbContext = new PassInDbContext();
        }

        public ResponseRegisterJson Execute(Guid eventId, RequestRegisterEventJson request)
        {
            Validate(eventId, request);
            var entity = new Infrastructure.Entitites.Attendee
            {
                Email = request.Email,
                Name = request.Name,
                Event_Id = eventId,
                Created_At = DateTime.UtcNow
            };

            _dbContext.Attendees.Add(entity);
            _dbContext.SaveChanges();
            return new ResponseRegisterJson()
            {
                Id = entity.Id
            };
        }

        private void Validate(Guid eventId, RequestRegisterEventJson request)
        {
            var eventEntity = _dbContext.Events.Find(eventId);
            if (eventEntity is null) 
            {
                throw new NotFoundException("This event does not exist.");
            }

            if (string.IsNullOrEmpty(request.Name))
            {
                throw new ErrorOnValidationException("The name is invalid.");
            }

            if (!EmailIsValid(request.Email))
            {
                throw new ErrorOnValidationException("The email is invalid.");
            }

            var emailExist = _dbContext.Attendees.Any(at => at.Email.Equals(request.Email) && at.Event_Id == eventId);
            if (emailExist)
            {
                throw new ConflictException("You can not register twice on the same event.");
            }

            var attForEvent = _dbContext.Attendees.Count(att => att.Event_Id == eventId);
            if (attForEvent == eventEntity.Maximum_Attendees)
            {
                throw new ErrorOnValidationException("There is no room for this event.");
            }
        }

        private bool EmailIsValid(string email)
        {
            try
            {
                new MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
