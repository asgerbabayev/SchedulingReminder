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
        public GetRemindersQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IDataResult<IEnumerable<GetRemindersQuery>>> Handle(GetRemindersQuery request, CancellationToken cancellationToken)
        {
            return new SuccessDataResult<IEnumerable<GetRemindersQuery>>(
                _mapper.Map<IEnumerable<GetRemindersQuery>>(await _context.Reminders.AsNoTracking().ToListAsync()));
        }
    }
}
