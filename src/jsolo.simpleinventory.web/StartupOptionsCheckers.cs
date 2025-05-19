namespace jsolo.simpleinventory.web;

static class STARTUP_OPTIONS_CHECKER
{
    public static bool MIGRATE_DATABASE(string[] program_args)
    {
        bool migrateDB = false;

        foreach (var arg in program_args)
        {
            migrateDB |= (
                arg.ToLower().Contains("migrate") || arg.ToLower().Contains("migrate")
            ) && (
                arg.ToLower().Contains("database") || arg.ToLower().Contains("db")
            ) && !arg.ToLower().Contains("no");

            if (migrateDB == true) break;
        }
        return migrateDB;
    }


    public static bool ATTEMPT_SEED_DATABASE(string[] program_args, bool isMigratingDB)
    {
        bool seedDB = false;

        if (isMigratingDB)
        {
            foreach (var arg in program_args)
            {
                seedDB |= (
                    arg.ToLower().Contains("seed")
                ) && (
                    arg.ToLower().Contains("database") || arg.ToLower().Contains("db")
                ) && !(
                    arg.ToLower().Contains("no")
                );

                if (seedDB == true) break;
            }
        }

        return seedDB;
    }


    public static bool LAUNCH_APP(string[] program_args, bool isMigratingDB)
    {
        bool launchAPP = false;

        foreach (var arg in program_args)
        {
            launchAPP |= (
                arg.ToLower().Contains("run") ||
                arg.ToLower().Contains("start") ||
                arg.ToLower().Contains("launch")
            ) && (
                arg.ToLower().Contains("app")
            );

            if (launchAPP == true) break;
        }
        return launchAPP || !isMigratingDB;
    }
}