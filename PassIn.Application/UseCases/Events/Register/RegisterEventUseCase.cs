using PassIn.Communication.Requests;
using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;

namespace PassIn.Application.UseCases.Events.Register
{
    public class RegisterEventUseCase
    {
        public ResponseRegisterEventJson Execute(RequestEventJson request)
        {
            Validate(request);

            var dbContext = new PassInDbContext();

            var entity = new Infrastructure.Entitites.Event
            {
                Title = request.Title,
                Details = request.Details,
                Maximum_Attendees = request.MaximumAttendees,
                Slug = request.Title.ToLower().Replace(" ","-"),                
            };

            dbContext.Events.Add(entity);//prepara a query para o insert
            dbContext.SaveChanges();//executa query
            return new ResponseRegisterEventJson
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
                throw new PassInException("The Maximun Attendees is invalid.");
            }

            if (string.IsNullOrWhiteSpace(request.Title)) 
            {
                throw new PassInException("The Title is invalid.");
            }

            if (string.IsNullOrWhiteSpace(request.Details))
            {
                throw new PassInException("The Details is invalid.");
            }
        }
    }
}
