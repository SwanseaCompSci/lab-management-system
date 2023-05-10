namespace SwanseaCompSci.LabManagementSystem.Infrastructure.Persistence.Common.Helpers
{
    /// <summary>
    /// Provides helper methods for log messages.
    /// </summary>
    internal static class RepositoryLogMessages
    {
        /// <summary>
        /// Gets a log message that should be logged before adding an entity.
        /// </summary>
        /// <param name="entityName">Name of the entity which will be added.</param>
        /// <returns>The log message in a format "Adding entity '{entityName}' to the database.".</returns>
        internal static string GetAddingEntityLogMessage(string entityName)
        {
            return $"Adding entity '{entityName}' to the database.";
        }
        /// <summary>
        /// Gets a log message that should be logged after an entity was added.
        /// </summary>
        /// <param name="entityName">Name of the added entity.</param>
        /// <param name="key">A key of the added entity.</param>
        /// <returns>The log message in a format "Entity '{entityName}' ({key}) was added to the database.".</returns>
        internal static string GetEntityAddedLogMessage(string entityName, string key)
        {
            return $"Entity '{entityName}' ({key}) was added to the database.";
        }

        /// <summary>
        /// Gets a log message that should be logged before adding an entity.
        /// </summary>
        /// <param name="entityName">Name of the entity which will be added.</param>
        /// <returns>The log message in a format "Adding entity '{entityName}' to the database.".</returns>
        internal static string GetAddingEntitiesLogMessage(string entityName)
        {
            return $"Adding entities '{entityName}' to the database.";
        }

        /// <summary>
        /// Gets a log message that should be logged before updating an entity.
        /// </summary>
        /// <param name="entityName">Name of the entity which will be updated.</param>
        /// <param name="key">A key of the entity which will be updated.</param>
        /// <returns>The log message in a format "Updating entity '{entityName}' ({key}) in the database.".</returns>
        internal static string GetUpdatingEntityLogMessage(string entityName, string key)
        {
            return $"Updating entity '{entityName}' ({key}) in the database.";
        }
        /// <summary>
        /// Gets a log message that should be logged after an entity was updated.
        /// </summary>
        /// <param name="entityName">Name of the updated entity.</param>
        /// <param name="key">A key of the updated entity.</param>
        /// <returns>The log message in a format "Entity '{entityName}' ({key}) was updated in the database.".</returns>
        internal static string GetEntityUpdatedLogMessage(string entityName, string key)
        {
            return $"Entity '{entityName}' ({key}) was updated in the database.";
        }

        /// <summary>
        /// Gets a log message that should be logged before updating entities.
        /// </summary>
        /// <param name="entityName">Name of the entities which will be added.</param>
        /// <returns>The log message in a format "Updating entities '{entityName}' in the database.".</returns>
        internal static string GetUpdatingEntitiesLogMessage(string entityName)
        {
            return $"Updating entities '{entityName}' in the database.";
        }

        /// <summary>
        /// Gets a log message that should be logged before deleting an entity.
        /// </summary>
        /// <param name="entityName">Name of the entity which will be deleted.</param>
        /// <param name="key">A key of the entity which will be deleted.</param>
        /// <returns>The log message in a format "Deleting entity '{entityName}' ({key}) from the database.".</returns>
        internal static string GetDeletingEntityLogMessage(string entityName, string key)
        {
            return $"Deleting entity '{entityName}' ({key}) from the database.";
        }
        /// <summary>
        /// Gets a log message that should be logged after an entity was deleted.
        /// </summary>
        /// <param name="entityName">Name of the deleted entity.</param>
        /// <param name="key">A key of the deleted entity.</param>
        /// <returns>The log message in a format "Entity '{entityName}' ({key}) was deleted from the database.".</returns>
        internal static string GetEntityDeletedLogMessage(string entityName, string key)
        {
            return $"Entity '{entityName}' ({key}) was deleted from the database.";
        }

        /// <summary>
        /// Gets a log message that should be logged before deleting entities.
        /// </summary>
        /// <param name="entityName">Name of the entities which will be deleted.</param>
        /// <param name="key">A key of the entities which will be deleted.</param>
        /// <returns>The log message in a format "Deleting entities '{entityName}' ({key}) from the database.".</returns>
        internal static string GetDeletingEntitiesLogMessage(string entityName, string key)
        {
            return $"Deleting entities '{entityName}' ({key}) from the database.";
        }
        /// <summary>
        /// Gets a log message that should be logged after deleting entities.
        /// </summary>
        /// <param name="entityName">Name of the entities which were deleted.</param>
        /// <param name="key">A key of the entities which were deleted.</param>
        /// <returns>The log message in a format "Deleted entities '{entityName}' ({key}) from the database.".</returns>
        internal static string GetDeletedEntitiesLogMessage(string entityName, string key)
        {
            return $"Deleted entities '{entityName}' ({key}) from the database.";
        }

        /// <summary>
        /// Gets a log message that should be logged before deleting all entities.
        /// </summary>
        /// <param name="entityName">Name of the entities which will be deleted.</param>
        /// <returns>The log message in a format "Deleting all entities '{entityName}' from the database.".</returns>
        internal static string GetDeletingAllEntitiesLogMessage(string entityName)
        {
            return $"Deleting all entities '{entityName}' from the database.";
        }
        /// <summary>
        /// Gets a log message that should be logged after deleting all entities.
        /// </summary>
        /// <param name="entityName">Name of the entities which were deleted.</param>
        /// <returns>The log message in a format "Deleted all entities '{entityName}' from the database.".</returns>
        internal static string GetDeletedAllEntitiesLogMessage(string entityName)
        {
            return $"Deleted all entities '{entityName}' from the database.";
        }
    }
}
