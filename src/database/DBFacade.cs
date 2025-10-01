namespace database;

using Microsoft.Data.Sqlite;

public class DBFacade
{
    private string? DBpath;
    private bool tempDb; 

    public DBFacade(string? DBpath)
    {
        this.DBpath = DBpath;
        tempDb = false;
    }
    
    public void initDB()
    {
        if (string.IsNullOrEmpty(DBpath))
        {
            DBpath = Path.Combine(Path.GetTempPath(), "chirp.db");
            tempDb = true;
        }
        
        using (var connection = new SqliteConnection($"Data Source={DBpath}"))
        {
            
            SetupTables(connection);

            if (tempDb) {initDump(connection);}



            Console.WriteLine("Database was successfully created in location:");
            Console.WriteLine(DBpath);
        
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

    public List<String> Get() //Print alt 
    {
        List<CheepViewModel> list  = new List<CheepViewModel>();
        
        using (var connection = new SqliteConnection($"Data Source={DBpath}"))
        {
            connection.Open();
            
            
            
            String SqlCommand = @"SELECT * FROM message
            left outer join user on user_id = message.author_id

                                  ";
            

            // query author, text og timeestamp istedet for bare text
            // put record
            // put record i liste
            // returner liste
            
            using (var command = new SqliteCommand(SqlCommand, connection))
            {
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
    
    public List<String> Get(String? author) //Print fra speciel author
    {
        List<CheepViewModel> list  = new List<CheepViewModel>();
        
        using (var connection = new SqliteConnection($"Data Source={DBpath}"))
        {
            connection.Open();
            String SqlCommand;

           
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