using SX.Common.Shared.Models;

namespace SX.Common.Shared.Interfaces
{
    public interface IParamValueContainer
    {
        ParamValueCollection GetParamValues(string prefix = "");
    }
}
