using AutoMapper;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Models.UserModels
{
    // TODO: Add docs comments

    /// <summary>
    /// A <see cref="User"/> application model.
    /// </summary>
    public class UserModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string AchievedLevel { get; set; } = null!;
        public int MaxWeeklyWorkHours { get; set; }
        public Guid? QuestionnaireToken { get; set; }

        public override string ToString()
        {
            return $"{FirstName} {Surname}";
        }
    }

    /// <summary>
    /// A mapping profile for <see cref="UserModel"/>.
    /// </summary>
    public sealed class UserModelMappingProfile : Profile
    {
        /// <summary>
        /// Creates a new instance of the <see cref="UserModelMappingProfile"/> class.
        /// </summary>
        public UserModelMappingProfile()
        {
            CreateMap<User, UserModel>()
                .ForMember(x => x.AchievedLevel, m => m.MapFrom(s => s.AchievedLevel.ToString()));
        }
    }
}
