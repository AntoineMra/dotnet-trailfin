using Trailfin.Web.Helpers;
using System.Net;

namespace Trailfin.Web.Helpers
{
    public class Envelope<T>
    {
        public Envelope()
        { }

        public T Result { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime TimeGenerated { get; set; }

        protected internal Envelope(string errorMessage)
        {
            ErrorMessage = errorMessage;
            TimeGenerated = DateTime.UtcNow;
        }

        protected internal Envelope(T result)
        {
            Result = result;
            TimeGenerated = DateTime.UtcNow;
        }
    }
}

public class Envelope : Envelope<string>
{
    public Envelope(string result) : base(result)
    {
    }

    public Envelope(string errorMessage, HttpStatusCode code) : base(errorMessage)
    {
    }

    public static Envelope<T> Error<T>(string errorMessage)
    {
        return new Envelope<T>(errorMessage);
    }

    public static Envelope Error(string errorMessage)
    {
        return new Envelope(errorMessage);
    }

    public static Envelope<T> Ok<T>(T result)
    {
        return new Envelope<T>(result);
    }
}