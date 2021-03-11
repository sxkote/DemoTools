using System;
using System.Net;

namespace SX.Common.Shared.Exceptions
{
    public enum ExceptionCategory : int
    {
        /// <summary>
        /// Нет ошибки
        /// </summary>
        None = 0,

        /// <summary>
        /// Логические ошибки ввода данных и выполнения действий
        /// </summary>
        Logic = 1,

        /// <summary>
        /// Ошибки доступа и авторизации
        /// </summary>
        Access = 7,

        /// <summary>
        /// Серьезные ошибки интеграции, функционала 
        /// </summary>
        Error = 9,

        /// <summary>
        /// Критические ошибки конфигурации, реализации
        /// </summary>
        Critical = 10
    }

    public class CustomException : ApplicationException
    {
        public virtual ExceptionCategory Category { get; protected set; }

        public virtual string Comment { get; protected set; }

        public virtual int StatusCode { get; set; } = (int)HttpStatusCode.InternalServerError;

        public override string Message => this.Comment ?? "";


        public CustomException(string comment, ExceptionCategory category)
        {
            this.Comment = comment;
            this.Category = category;
        }
    }

    public class CustomConfigurationException : CustomException
    {
        public override int StatusCode { get { return (int)HttpStatusCode.ServiceUnavailable; } }

        public CustomConfigurationException(string comment)
            : base(comment, ExceptionCategory.Critical) { }
    }

    public class CustomNotFoundException : CustomException
    {
        public override int StatusCode { get { return (int)HttpStatusCode.NotFound; } }

        public CustomNotFoundException(string comment)
            : base(comment, ExceptionCategory.Error) { }
    }

    public class CustomAuthenticationException : CustomException
    {
        public override int StatusCode { get { return (int)HttpStatusCode.Unauthorized; } }

        public CustomAuthenticationException(string comment)
            : base(comment, ExceptionCategory.Access) { }
    }

    public class CustomAccessException : CustomException
    {
        public override int StatusCode { get { return (int)HttpStatusCode.Forbidden; } }

        public CustomAccessException(string comment)
            : base(comment, ExceptionCategory.Access) { }
    }

    public class CustomIntegrationException : CustomException
    {
        public override int StatusCode { get { return (int)HttpStatusCode.ServiceUnavailable; } }

        public CustomIntegrationException(string comment)
            : base(comment, ExceptionCategory.Error) { }
    }

    public class CustomExecutionException : CustomException
    {
        public override int StatusCode { get { return (int)HttpStatusCode.MethodNotAllowed; } }

        public CustomExecutionException(string comment)
            : base(comment, ExceptionCategory.Error) { }
    }

    public class CustomOperationException : CustomExecutionException
    {
        public CustomOperationException(string comment)
            : base(comment)
        {
            this.Category = ExceptionCategory.Logic;
        }
    }

    //public class CustomLockedException : CustomException
    //{
    //    public override int StatusCode { get { return (int)HttpStatusCode.NotAcceptable; } }

    //    public CustomLockedException(string comment, ExceptionCategory category = ExceptionCategory.Error)
    //        : base(comment, category) { }
    //}

    public class CustomArgumentException : CustomException
    {
        public override int StatusCode { get { return (int)HttpStatusCode.BadRequest; } }

        public CustomArgumentException(string comment)
            : base(comment, ExceptionCategory.Error) { }
    }

    public class CustomInputException : CustomArgumentException
    {
        public CustomInputException(string comment)
            : base(comment)
        {
            this.Category = ExceptionCategory.Logic;
        }
    }

    public class CustomNotImplementedException : CustomException
    {
        public override int StatusCode { get { return (int)HttpStatusCode.NotImplemented; } }

        public CustomNotImplementedException(string comment)
            : base(comment, ExceptionCategory.Critical) { }
    }

    public class CustomFormatException : CustomException
    {
        public override int StatusCode { get { return (int)HttpStatusCode.MethodNotAllowed; } }

        public CustomFormatException(string comment)
            : base(comment, ExceptionCategory.Error) { }
    }
}
