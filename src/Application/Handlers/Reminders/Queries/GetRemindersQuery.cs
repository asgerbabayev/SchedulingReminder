using Microsoft.AspNetCore.Http;

namespace ShedulingReminders.Application.Handlers.Reminders.Queries;

public class GetRemindersQuery : IRequest<IDataResult<IEnumerable<GetRemindersQuery>>>
{
    public Guid Id { get; set; }
    public string To { get; set; } = null!;       // Recipient of the reminder
    public string Content { get; set; } = null!;  // Content of the reminder
    public DateTime SendAt { get; set; }          // Date and time to send the reminder
    public string Method { get; set; } = null!;   // Method of sending the reminder
    public class GetRemindersQueryHandler : IRequestHandler<GetRemindersQuery, IDataResult<IEnumerable<GetRemindersQuery>>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _accessor;
        #region Constructor
        public GetRemindersQueryHandler(IApplicationDbContext context,
            IMapper mapper, IHttpContextAccessor accessor)
        {
            _context = context;
            _mapper = mapper;
            _accessor = accessor;
        }
        #endregion
        public async Task<IDataResult<IEnumerable<GetRemindersQuery>>> Handle(GetRemindersQuery request, CancellationToken cancellationToken)
        {
            return new SuccessDataResult<IEnumerable<GetRemindersQuery>>(
                _mapper.Map<IEnumerable<GetRemindersQuery>>(await _context.Reminders.
                Include(x => x.AppUser).AsNoTracking().
                Where(x =>
                x.AppUser.UserName == _accessor.HttpContext.User.Identity.Name).ToListAsync()));
        }
    }
}
