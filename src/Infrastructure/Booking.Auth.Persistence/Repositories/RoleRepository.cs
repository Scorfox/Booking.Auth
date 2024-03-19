using Booking.Auth.Application.Repositories;
using Booking.Auth.Persistence.Context;

namespace Booking.Auth.Persistence.Repositories;

public class RoleRepository(DataContext dataContext) : IRoleRepository;