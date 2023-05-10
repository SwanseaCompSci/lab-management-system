using SwanseaCompSci.LabManagementSystem.Core.Application.Allocation.Enums;
using SwanseaCompSci.LabManagementSystem.Core.Application.Allocation.Models;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Allocation.Common
{
    // TODO: Add docs comments
    public static class Helpers
    {
        public static Degree GetDegree(UserModel user)
        {
            return user.AchievedLevel switch
            {
                Level.Year1 => Degree.Undergraduate,
                Level.Year2 => Degree.Undergraduate,
                Level.Year3 => Degree.Undergraduate,
                Level.Masters => Degree.Postgraduate,
                Level.PhD => Degree.PhD,
                _ => throw new ArgumentOutOfRangeException(nameof(user)),
            };
        }

        public static bool IsAvailable(UserModel user, WorkDayOfWeek day, TimeOnly startTime, TimeOnly endTime)
        {
            var timeAvailabilities = user.TimeAvailabilities.Where(x => x.Day == day && x.IsAllocated == false);

            var startHour = startTime.Hour;
            var endHour = endTime.Minute == 0 ? endTime.Hour : endTime.Hour + 1;

            for (int i = startHour; i < endHour; i++)
            {
                if (timeAvailabilities.Any(x => x.StartTime.Hour == i) == false)
                {
                    return false;
                }
            }

            return true;
        }

        public static void AllocateHours(UserModel user, WorkDayOfWeek day, TimeOnly startTime, TimeOnly endTime)
        {
            var timeAvailabilities = user.TimeAvailabilities.Where(x => x.Day == day && x.IsAllocated == false);

            var startHour = startTime.Hour;
            var endHour = endTime.Minute == 0 ? endTime.Hour : endTime.Hour + 1;

            for (int i = startHour; i < endHour; i++)
            {
                var timeAvailability = timeAvailabilities.FirstOrDefault(x => x.StartTime.Hour == i);

                if (timeAvailability is not null)
                {
                    timeAvailability.IsAllocated = true;
                }
                else
                {
                    timeAvailability = new TimeAvailabilityModel()
                    {
                        Id = Guid.NewGuid(),
                        Day = day,
                        StartTime = new TimeOnly(i, 00),
                        EndTime = new TimeOnly(i + 1, 00),
                        IsAllocated = true,
                    };
                    user.TimeAvailabilities.Add(timeAvailability);
                }
            }
        }
    }
}
