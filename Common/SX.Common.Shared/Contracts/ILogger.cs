using System;

namespace SX.Common.Shared.Contracts
{
    public interface ILogger
    {
        // запись информации в режиме трассировки
        void Trace(object data);

        //// запись информации в режиме отладки
        //void Debug(string message);

        // запись сообщения в лог
        void Log(string message);

        // запись ошибки в лог
        void Error(string message);
        void Error(Exception exception);
    }
}
