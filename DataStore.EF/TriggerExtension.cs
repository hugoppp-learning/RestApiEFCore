using DataStore.EF.Models;
using Microsoft.EntityFrameworkCore;

namespace DataStore.EF;

public static class TriggerExtension
{
    public static DbContextOptionsBuilder AddAllTriggers(this DbContextOptionsBuilder services)
    {
        return services.UseTriggers(options => options.AddTrigger<TicketInsertIdTrigger>());
    }

}
