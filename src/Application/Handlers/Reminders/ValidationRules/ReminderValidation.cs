using ShedulingReminders.Application.Handlers.Reminders.Commands;

namespace ShedulingReminders.Application.Handlers.Reminders.ValidationRules;

public class CreateReminderValidation : AbstractValidator<CreateReminderCommand>
{
    public CreateReminderValidation()
    {
        RuleFor(x => x.Content).NotEmpty();
        RuleFor(x => x.SendAt).NotEmpty();
        RuleFor(x => x.Method).NotEmpty();
        RuleFor(x => x.To).NotEmpty();
    }
}

public class UpdateReminderValidation : AbstractValidator<UpdateReminderCommand>
{
    public UpdateReminderValidation()
    {
        RuleFor(x => x.Content).NotEmpty();
        RuleFor(x => x.SendAt).NotEmpty();
        RuleFor(x => x.Method).NotEmpty();
        RuleFor(x => x.To).NotEmpty();
    }
}

