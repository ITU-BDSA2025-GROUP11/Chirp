namespace database;

using Microsoft.Data.Sqlite;

public class DBFacade
{
    private string? DBpath;

    public DBFacade(string? DBpath)
    {
        this.DBpath = DBpath;
    }

    public void initDB()
    {
        if (string.IsNullOrEmpty(DBpath))
        {
            DBpath = Path.Combine(Path.GetTempPath(), "chirp.db");
        }
        
        using (var connection = new SqliteConnection($"Data Source={DBpath}"))
        {
            
            SetupTables(connection);
            
            initDump(connection);
            
            
            
            Console.WriteLine("Database was successfully created in location:");
            Console.WriteLine(DBpath);
            
            while (true) {
            }
        }
    }

    private void SetupTables(SqliteConnection connection)
    {
        connection.Open();
        using var command = connection.CreateCommand();

        command.CommandText = @"
                drop table if exists user; 
                create table user (
                user_id integer primary key autoincrement,
                username text not null,
                email text not null,
                pw_hash text not null
                 );
            ";
        command.ExecuteNonQuery();
            
        command.CommandText = @"
                drop table if exists message;
                create table message (
                message_id integer primary key autoincrement,
                author_id integer not null,
                text string not null,
                pub_date integer
                    );
            ";
            
        command.ExecuteNonQuery();
    }

    private void initDump(SqliteConnection connection)
    {
        string dumpPath = Path.Combine(FindSolutionFolder(), "data", "dump.sql");
        
        dumpPath = File.ReadAllText(dumpPath);
        string[] sqlCommands = dumpPath.Split(";",  StringSplitOptions.RemoveEmptyEntries);
     
        using var transaction = connection.BeginTransaction();
        
        foreach (var sqlCommand in sqlCommands)
        {
            var sqlCommandTrimmed = sqlCommand.Trim();
            
            if (string.IsNullOrEmpty(sqlCommandTrimmed)) continue;

            using var command = connection.CreateCommand();
            command.CommandText = sqlCommandTrimmed;
            command.ExecuteNonQuery();
        }
        transaction.Commit();
    }

    private String FindSolutionFolder()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        
        while (directory != null && !directory.GetFiles("*.sln").Any())
        {
            directory = directory.Parent;
        }
    
        return directory?.FullName ?? throw new Exception("Could not find solution folder");
    }
}