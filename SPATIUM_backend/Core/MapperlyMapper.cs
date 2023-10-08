using Riok.Mapperly.Abstractions;
using SPATIUM_backend.Dtos;
using SPATIUM_backend.Models;
using System.Threading;
using Microsoft.EntityFrameworkCore;

namespace SPATIUM_backend.Core;

[Mapper(PropertyNameMappingStrategy = PropertyNameMappingStrategy.CaseInsensitive, UseReferenceHandling = true)]
public partial class MapperlyMapper
{
	[MapProperty(nameof(User.SkillUsers), nameof(ProfileDto.Skills))]
	public partial void Map(User user, ProfileDto profileDto);
	[MapProperty(nameof(User.SkillUsers), nameof(ProfileDto.Skills))]
	public partial ProfileDto Map(User user);
	[MapperIgnoreTarget(nameof(User.Id))]
	public partial void Map(ProfileDto profileDto, User user);
	[MapperIgnoreTarget(nameof(User.Id))]
	public partial User Map(ProfileDto profileDto);

	public void MapCustom(User user, ProfileDto userDto)
	{
		Map(user, userDto);
		userDto.Skills = user.SkillUsers.Select(x => x.Skill.Name).ToList();
	}
	public ProfileDto MapCustom(User user)
	{
		var projectDto = Map(user);
		projectDto.Skills = user.SkillUsers.Select(x => x.Skill.Name).ToList();
		return projectDto;
	}


	[MapProperty(nameof(Project.RequiredSkills), nameof(ProjectDto.RequiredSkills))]
	public partial void Map(Project project, ProjectDto projectDto);
	[MapProperty(nameof(Project.RequiredSkills), nameof(ProjectDto.RequiredSkills))]
	public partial ProjectDto Map(Project project);
	[MapperIgnoreTarget(nameof(Project.Id))]
	[MapperIgnoreTarget(nameof(Project.RequiredSkills))]
	[MapProperty(nameof(ProjectDto.Creator), nameof(@Project.Creator.UserName))]
	public partial void Map(ProjectDto projectDto, Project project);
	[MapperIgnoreTarget(nameof(Project.Id))]
	[MapperIgnoreTarget(nameof(Project.RequiredSkills))]
	[MapProperty(nameof(ProjectDto.Creator), nameof(@Project.Creator.UserName))]
	public partial Project Map(ProjectDto projectDto);

	public void MapCustom(Project project, ProjectDto projectDto)
	{
		Map(project, projectDto);
		projectDto.RequiredSkills = project.RequiredSkills.Select(x => x.Skill.Name).ToList();
	}
	public ProjectDto MapCustom(Project project)
	{
		var projectDto = Map(project);
		projectDto.RequiredSkills = project.RequiredSkills.Select(x => x.Skill.Name).ToList();
		return projectDto;
	}

	public partial void Map(Skill skill, SkillDto skillDto);
	public partial SkillDto Map(Skill skill);
	public partial void Map(SkillDto skillDto, Skill skill);
	public partial Skill Map(SkillDto skillDto);

	[MapProperty(nameof(@SkillUser.Skill.Name), nameof(SkillDto.Name))]
	public partial void Map(SkillUser skillUser, SkillDto skillDto);
	[MapProperty(nameof(@SkillUser.Skill.Name), nameof(SkillDto.Name))]
	public partial SkillDto Map(SkillUser skillUser);
	public partial void Map(SkillDto skillDto, SkillUser skillUser);

	[MapProperty(nameof(@SkillProject.Skill.Name), nameof(SkillDto.Name))]
	public partial void Map(SkillProject skillProject, SkillDto skillDto);
	[MapProperty(nameof(@SkillProject.Skill.Name), nameof(SkillDto.Name))]
	public partial SkillDto Map(SkillProject skillProject);
	public partial void Map(SkillDto skillDto, SkillProject skillProject);
}