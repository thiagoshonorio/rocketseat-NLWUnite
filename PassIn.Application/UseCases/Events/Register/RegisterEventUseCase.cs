using PassIn.Communication.Requests;
using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;

namespace PassIn.Application.UseCases.Events.Register
{
    public class RegisterEventUseCase
    {
        private readonly PassInDbContext _dbContext;

        public RegisterEventUseCase()
        {
            _dbContext = new PassInDbContext();
        }

        public ResponseRegisterJson Execute(RequestEventJson request)
        {
            Validate(request);

            var entity = new Infrastructure.Entitites.Event
            {
                Title = request.Title,
                Details = request.Details,
                Maximum_Attendees = request.MaximumAttendees,
                Slug = request.Title.ToLower().Replace(" ","-"),                
            };

            _dbContext.Events.Add(entity);//prepara a query para o insert
            _dbContext.SaveChanges();//executa query
            return new ResponseRegisterJson
            {
                Id = entity.Id
            };
        }

        //Erro 500 Exception não esperado
        //Erro 400 Exception esperado
        private void Validate(RequestEventJson request)
        {
            if (request.MaximumAttendees <= 0)
            {
                throw new ErrorOnValidationException("The Maximun Attendees is invalid.");
            }

            if (string.IsNullOrWhiteSpace(request.Title)) 
            {
                throw new ErrorOnValidationException("The Title is invalid.");
            }

            if (string.IsNullOrWhiteSpace(request.Details))
            {
                throw new ErrorOnValidationException("The Details is invalid.");
            }
        }
    }
}
