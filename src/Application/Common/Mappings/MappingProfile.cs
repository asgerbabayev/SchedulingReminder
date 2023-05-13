using ShedulingReminders.Application.Handlers.Reminders.Commands;
using ShedulingReminders.Application.Handlers.Reminders.Queries;

namespace ShedulingReminders.Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Reminder, CreateReminderCommand>().ReverseMap();
        CreateMap<Reminder, UpdateReminderCommand>().ReverseMap();
        CreateMap<Reminder, GetRemindersQuery>().ReverseMap();
    }
}
