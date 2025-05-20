using System;

using jsolo.simpleinventory.sys.common.interfaces;


namespace jsolo.simpleinventory.impl.services
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime Now => DateTime.Now;
    }
}
