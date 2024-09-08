using Microsoft.EntityFrameworkCore;
using Models.Passport;
using Models.PassportBoard;
using Models.Users;

namespace Service.ApplicationDbContext;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Admin> Admins { get; init; }
    public DbSet<Passport> Passports { get; init; }
    public DbSet<ConnectId> ConnectIds { get; init; }
    public DbSet<SessionNumber> SessionNumbers { get; init; }
    public DbSet<Comment> Comments { get; init; }
}