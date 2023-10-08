using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SPATIUM_backend.Models;

namespace SPATIUM_backend;

public class AppDbContext : IdentityDbContext<User>
{
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
	{
	}

	public DbSet<Project> Projects { get; set; }
	public DbSet<UserProject> UserProjects { get; set; }
	public DbSet<Skill> Skills { get; set; }
	public DbSet<SkillUser> SkillUsers { get; set; }
	public DbSet<SkillProject> SkillProjects { get; set; }

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);

		builder.Entity<UserProject>()
			.HasKey(up => new { up.UserId, up.ProjectId });

		builder.Entity<UserProject>()
			.HasOne(up => up.User)
			.WithMany(u => u.UserProjects)
			.HasForeignKey(up => up.UserId);

		builder.Entity<UserProject>()
			.HasOne(up => up.Project)
			.WithMany(p => p.UserProjects)
			.HasForeignKey(up => up.ProjectId);

		builder.Entity<Skill>()
			.HasIndex(s => s.Name)
			.IsUnique();

		builder.Entity<SkillUser>()
			.HasKey(su => new { su.UserId, su.SkillId });

		builder.Entity<SkillUser>()
			.HasOne(su => su.User)
			.WithMany(u => u.SkillUsers)
			.HasForeignKey(su => su.UserId);

		builder.Entity<SkillProject>()
			.HasKey(su => new { su.ProjectId, su.SkillId });

		builder.Entity<SkillProject>()
			.HasOne(sp => sp.Project)
			.WithMany(p => p.RequiredSkills)
			.HasForeignKey(sp => sp.ProjectId);
	}
}