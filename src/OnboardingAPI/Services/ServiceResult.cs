namespace OnboardingAPI.Services
{
    public class ServiceResult<T>
    {
        public bool Success { get; private set; } 
        public T Data { get; private set; }
        public string Message { get; private set; }

        private ServiceResult(bool success, T data, string message)
        {
            Success = success;
            Data = data;
            Message = message;
        }

        public static ServiceResult<T> CreateSuccess(T data)
        {
            return new ServiceResult<T>(true, data, string.Empty);
        }

        public static ServiceResult<T> CreateFailure(string message)
        {
            return new ServiceResult<T>(false, default(T), message);
        }
    }
}
