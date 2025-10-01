namespace database;

using Microsoft.Data.Sqlite;

public class DBFacade
{
    private string? DBpath;

    public DBFacade(string? DBpath = null)
    {
        this.DBpath = DBpath;
        initDB();
    }
    
    public void initDB() {
        using (var connection = new SqliteConnection($"Data Source={DBpath}"))
            
            if (string.IsNullOrEmpty(DBpath)) {
                DBpath = Path.Combine(Path.GetTempPath(), "chirp.db");
                
                SetupTables(connection);
                initDump(connection);

                Console.WriteLine($"A temporary database has been created: {DBpath}");
            } else {
                if (!File.Exists(DBpath))
                {
                    File.Create(DBpath);
                    SetupTables(connection);
                    
                    Console.WriteLine($"Database was not found at: {DBpath} created new database and setup tables");
                }
                else
                {
                    Console.WriteLine($"Connected to existing database: {DBpath}");
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
        int commandCount = 0;
        
        string dumpPath = Path.Combine(FindSolutionFolder(), "data", "dump.sql");
        var dumplines = File.ReadLines(dumpPath);

        using var transaction = connection.BeginTransaction();

        foreach (var line in dumplines)
        {
            commandCount++;
            
            if (string.IsNullOrWhiteSpace(line)) continue;
            
            var lineTrimmed = line.Trim();
            
            using var command = connection.CreateCommand();
            command.CommandText = lineTrimmed;
            command.ExecuteNonQuery();
        }
        transaction.Commit();
        Console.WriteLine($"executed {commandCount} commands");
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

    public void Post(String post)
    {
        using (var connection = new SqliteConnection($"Data Source={DBpath}")) {
            connection.Open();
            
            string SqlCommand = "INSERT INTO message (author_id, text, pub_date) VALUES (@AuthorId, @Text, @PubDate)";

            using (var command = new SqliteCommand(SqlCommand, connection))
            {
                command.Parameters.AddWithValue("@AuthorId", Environment.UserName);
                command.Parameters.AddWithValue("@Text", post);

                long unixTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                command.Parameters.AddWithValue("@PubDate", unixTime);

                command.ExecuteNonQuery();
            }
        } 
    }

    public List<String> Get()
    {
        List<CheepViewModel> list  = new List<CheepViewModel>();
        
        using (var connection = new SqliteConnection($"Data Source={DBpath}"))
        {
            connection.Open();
            String SqlCommand;

            if (author == null)
            {
                SqlCommand = "SELECT text FROM message";
            }
            else
            {
                SqlCommand = "SELECT text FROM message WHERE author_id = @AuthorId";
            }

            // query author, text og timeestamp istedet for bare text
            // put record
            // put record i liste
            // returner liste
            
            using (var command = new SqliteCommand(SqlCommand, connection))
            {
                if (author != null)
                {
                    command.Parameters.AddWithValue("@AuthorId", author);
                }
                
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(reader.GetString(0));
                    }
                }
            }
        }
        return list;  
    }
    
    public List<String> Get(String? author)
    {
        List<CheepViewModel> list  = new List<CheepViewModel>();
        
        using (var connection = new SqliteConnection($"Data Source={DBpath}"))
        {
            connection.Open();
            String SqlCommand;

            if (author == null)
            {
                SqlCommand = "SELECT text FROM message";
            }
            else
            {
                SqlCommand = "SELECT text FROM message WHERE author_id = @AuthorId";
            }

            // query author, text og timeestamp istedet for bare text
            // put record
            // put record i liste
            // returner liste
            
            using (var command = new SqliteCommand(SqlCommand, connection))
            {
                if (author != null)
                {
                    command.Parameters.AddWithValue("@AuthorId", author);
                }
                
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(reader.GetString(0));
                    }
                }
            }
        }
        return list;
    }
    
}