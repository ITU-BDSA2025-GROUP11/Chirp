namespace SimpleDB;

public record Cheep(string Author, string Message, long Timestamp)
{
    public string getMessage()
    {
        return Message;
    }
}

