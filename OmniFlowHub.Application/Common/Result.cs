namespace OmniFlowHub.Application.Common
{
    public class Result
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public string Error { get; }

        protected Result(bool isSuccess, string error)
        {
            //Validación lógica: Si el resultado es exitoso, no debe haber un mensaje de error
            if (isSuccess && !string.IsNullOrEmpty(error))
            {
                throw new InvalidOperationException("Un resultado exitoso no puede contener un mensaje de error.");
            }

            //Validación lógica: Si falla, debe venir un error descriptivo
            if (!isSuccess && string.IsNullOrEmpty(error))
            {
                throw new InvalidOperationException("Un resultado fallido debe contener un mensaje de error.");
            }

            IsSuccess = isSuccess;
            Error = error;
        }

        //Estandarizar la creación de resultados
        public static Result Success()
        {
            return new Result(true, string.Empty);
        }

        public static Result Failure(string error)
        {
            return new Result(false, error);
        }

        public static Result<T> Success<T>(T value)
        {
            return new Result<T>(value, true, string.Empty);
        }

        public static Result<T> Failure<T>(string error)
        {
            return new Result<T>(default!, false, error);
        }
    }

    public class Result<T> : Result
    {
        private readonly T _value;

        protected internal Result(T value, bool isSuccess, string error) : base(isSuccess, error)
        {
            _value = value;
        }

        //Value con lectura limpia y protegida
        public T Value
        {
            get
            {
                if (!IsSuccess)
                {
                    throw new InvalidOperationException("No se puede acceder al valor de un resultado fallido.");
                }

                return _value;
            }
        }
    }
}