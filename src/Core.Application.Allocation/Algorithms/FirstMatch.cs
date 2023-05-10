using SwanseaCompSci.LabManagementSystem.Core.Application.Allocation.Common;
using SwanseaCompSci.LabManagementSystem.Core.Application.Allocation.Common.Interfaces;
using SwanseaCompSci.LabManagementSystem.Core.Application.Allocation.Models;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Allocation.Algorithms
{
    // TODO: Add docs comments
    public sealed class FirstMatch : IAllocator
    {
        public IEnumerable<AllocationModel> Allocate(IReadOnlyCollection<UserModel> users,
                                                     IReadOnlyCollection<LabModel> labs,
                                                     IReadOnlyCollection<AllocationModel> allocations)
        {
            // Prepare output
            var output = new LinkedList<AllocationModel>();

            // Allocate the minimum number of staff
            foreach (var lab in labs)
            {
                // Add pre-allocated users to the lab model
                foreach (var allocation in allocations.Where(x => x.LabId == lab.Id))
                {
                    var user = users.FirstOrDefault(x => x.Id == allocation.UserId)
                        ?? throw new NullReferenceException($"User ({allocation.UserId}) not found.");

                    // Mark Time Availabilities as allocated
                    Helpers.AllocateHours(user: user, day: lab.Day, startTime: lab.StartTime, endTime: lab.EndTime);

                    lab.AllocatedUsers.Add(user);
                }

                // Run only if there is not enough members of staff
                if (lab.MinNumberOfStaff > lab.AllocatedUsers.Count)
                {
                    // Find all eligible users
                    var eligibleUsers = GetEligibleUsers(users: users, lab: lab);

                    // Take preferred members of staff first
                    var preferredUsers = eligibleUsers
                        .Where(eu => eu.ModulePreferences.Any(mp => mp.ModuleId == lab.ModuleId && mp.Status == Status.Accepted)).ToList();

                    foreach (var item in preferredUsers.Take(Math.Min(preferredUsers.Count(), lab.MinNumberOfStaff - lab.AllocatedUsers.Count)))
                    {
                        Helpers.AllocateHours(user: item, day: lab.Day, startTime: lab.StartTime, endTime: lab.EndTime);

                        lab.AllocatedUsers.Add(item);

                        eligibleUsers.Remove(item);
                    }

                    // Fill the rest with members of staff without preference
                    foreach (var item in eligibleUsers.Take(Math.Min(eligibleUsers.Count(), lab.MinNumberOfStaff - lab.AllocatedUsers.Count)).ToList())
                    {
                        Helpers.AllocateHours(user: item, day: lab.Day, startTime: lab.StartTime, endTime: lab.EndTime);

                        lab.AllocatedUsers.Add(item);

                        eligibleUsers.Remove(item);
                    }
                }

                // Produce output
                foreach (var allocatedUser in lab.AllocatedUsers)
                {
                    output.AddLast(new AllocationModel(userId: allocatedUser.Id, moduleId: lab.ModuleId, labId: lab.Id));
                }
            }

            // Allocate users to any labs that have not reached the maximum number of staff members
            var hasNewAllocation = true;
            while (hasNewAllocation && labs.Any(x => x.AllocatedUsers.Count < x.MaxNumberOfStaff))
            {
                hasNewAllocation = false;

                // Allocate 1 user to each lab until all labs have the maximum number of staff or no more users can be allocated
                foreach (var lab in labs.Where(x => x.AllocatedUsers.Count < x.MaxNumberOfStaff))
                {
                    // Find eligible users
                    var eligibleUsers = GetEligibleUsers(users: users, lab: lab);

                    // Take preferred members of staff first
                    var preferredUsers = eligibleUsers
                        .Where(eu => eu.ModulePreferences.Any(mp => mp.ModuleId == lab.ModuleId && mp.Status == Status.Accepted)).ToList();

                    // Take a preferred user or an eligible user
                    var user = preferredUsers.Any()
                        ? preferredUsers.FirstOrDefault()
                        : eligibleUsers.FirstOrDefault();

                    if (user is not null)
                    {
                        // Mark Time Availabilities as allocated
                        Helpers.AllocateHours(user: user, day: lab.Day, startTime: lab.StartTime, endTime: lab.EndTime);

                        lab.AllocatedUsers.Add(user);

                        output.AddLast(new AllocationModel(userId: user.Id, moduleId: lab.ModuleId, labId: lab.Id));

                        hasNewAllocation = true;
                    }
                }
            }

            return output;
        }

        private static IList<UserModel> GetEligibleUsers(IReadOnlyCollection<UserModel> users, LabModel lab)
        {
            var labEndHour = lab.EndTime.Minute == 0 ? lab.EndTime.Hour : lab.EndTime.Hour + 1;
            var labDuration = labEndHour - lab.StartTime.Hour;

            return users
                // Users who are NOT in the lab already
                .Where(user => lab.AllocatedUsers.Any(au => au.Id == user.Id) == false)
                // Users who can teach that level
                .Where(user => user.AchievedLevel >= lab.Level)
                // Users who have not been declined for the module
                .Where(user => user.ModulePreferences.Any(mp => mp.ModuleId == lab.ModuleId && mp.Status == Status.Declined) == false)
                // Users who would not exceed the maximum number of work hours
                .Where(user => user.MaxWeeklyWorkHours - user.TimeAvailabilities.Count(x => x.IsAllocated) >= labDuration)
                // Users who are available
                .Where(user => Helpers.IsAvailable(user: user, day: lab.Day, startTime: lab.StartTime, endTime: lab.EndTime))
                // Users who achieved higher level of education take priority
                .OrderByDescending(user => user.AchievedLevel)
                // Users with smaller number of allocated hours take priority
                .ThenBy(x => x.TimeAvailabilities.Count(x => x.IsAllocated))
                .ToList();
        }
    }
}
