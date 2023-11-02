using UsersManager.Application.Interfaces;

namespace UsersManager.Application.Common;

public class LocalUtcDateTimeService : IDateTimeService
{
    public DateTime Now => DateTime.UtcNow;
}
