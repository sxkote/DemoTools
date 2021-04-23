using SX.Common.Shared.Contracts;
using SX.Common.Shared.Interfaces;
using System;

namespace SX.Common.Api.Services
{
    public class FakeTokenProvider : ITokenProvider
    {
        public IToken GetToken()
        {
            return null;
        }
    }
}
