using Microsoft.EntityFrameworkCore;
using SPATIUM_backend.Models;
using System.Threading;
using System;

namespace SPATIUM_backend.Extensions;

public static class SkillExtensions
{
	public static async Task MapToSkillUsersAsync(this ICollection<SkillUser> skillUsers, List<string> skillNames,
		User user, AppDbContext appDbContext, CancellationToken cancellationToken)
	{
		await MapToSkillEntity(skillUsers, skillNames, appDbContext, (skillUser, skill) =>
		{
			skillUser.Skill = skill;
			skillUser.SkillId = skill.Id;
			skillUser.User = user;
			skillUser.UserId = user.Id;
		}, cancellationToken);
	}

	public static async Task MapToSkillProjectAsync(this ICollection<SkillProject> skillProjects, List<string> skillNames,
		Project project, AppDbContext appDbContext, CancellationToken cancellationToken)
	{
		await MapToSkillEntity(skillProjects, skillNames, appDbContext, (skillProject, skill) =>
		{
			skillProject.Skill = skill;
			skillProject.SkillId = skill.Id;
			skillProject.Project = project;
			skillProject.ProjectId = project.Id;
		}, cancellationToken);
	}

	private static async Task MapToSkillEntity<T>(ICollection<T> skills, List<string> skillNames,
		AppDbContext appDbContext, Action<T, Skill> skillSetter,  CancellationToken cancellationToken) where T : class
	{
		skillNames = skillNames.Distinct().ToList();
		skills.Clear();
		var allSkills = await appDbContext.Skills.ToListAsync(cancellationToken);

		foreach (var skillName in skillNames)
		{
			var skill = allSkills.FirstOrDefault(x => x.Name == skillName);

			if (skill == null)
			{
				skill = new Skill { Name = skillName };
				await appDbContext.Skills.AddAsync(skill, cancellationToken);
			}

			var newSkill = Activator.CreateInstance<T>();
			skillSetter(newSkill, skill);
			skills.Add(newSkill);
		}
	}
}