namespace TMS.APP.Models
{
    public class ApplicationResult<T>
    {
        public bool Success { get; set; }
        public IEnumerable<Error> Errors { get; set; }
        public T Data { get; set; }


        public static ApplicationResult<T> DefaultWithSuccess(T data)
        {
            return new ApplicationResult<T> { Success = true, Data = data };
        }

        public static ApplicationResult<T> DefaultWithError(string errorMessage)
        {
            return new ApplicationResult<T> { Success = false, Errors = new List<Error> { new Error(errorMessage) } };
        }
    }

    public class ApplicationResult : ApplicationResult<dynamic>
    {
        public static ApplicationResult DefaultWithSuccess(dynamic data)
        {
            return new ApplicationResult { Success = true, Data = data };
        }

        public static ApplicationResult DefaultWithError(string errorMessage)
        {
            return new ApplicationResult { Success = false, Errors = new List<Error> { new Error(errorMessage) } };
        }
    }
}
