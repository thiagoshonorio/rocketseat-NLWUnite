namespace PassIn.Exceptions
{
    public class PassInException : SystemException
    {
        public PassInException(string message) : base(message) //Pega o que esta depois do dois pontos e repassa para o construtor do SystemException
        {
            
        }
    }
}
