using SX.Common.Shared.Models;
using System;

namespace SX.Common.Shared.Interfaces
{
    public interface IToken
    {
        Guid TokenID { get; }
        Guid UserID { get; }
        string Login { get; }
        string IP { get; }
        string Name { get; }
        string Avatar { get; }

        DateTimeOffset? Date { get; }
        DateTimeOffset? Expire { get; }

        string[] Roles { get; }
        string[] Permissions { get; }

        ParamValueCollection Params { get; }

        bool IsValid();
        bool IsInRole(string role);
        bool HasPermission(string permisssion);
        bool IsAdministrator();
    }
}
