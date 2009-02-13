using PaulStovell.Database.Management;

namespace PaulStovell.Database
{
    public interface IDatabaseManager
    {
        bool DoesDatabaseExist();
        void CreateDatabase();
        void DestroyDatabase();
        int GetCurrentVersion();
        int GetApplicationVersion();
        DatabaseUpgradeResult PerformUpgrade();
    }
}