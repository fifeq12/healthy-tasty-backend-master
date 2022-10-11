using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthyTasty.Infrastructure.Exceptions
{
    public enum ErrorCodes
    {
        InvalidCredentials,
        UnhandledException,
        RefreshTokenNotFound,
        RefreshTokenAlreadyRevoked,
        UserAlreadyExist
    }
}
