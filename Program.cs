using Microsoft.VisualBasic.FileIO;

List<String[]> tweets = new List<String[]>();

String filepath = "chirp_cli_db.csv";
TextFieldParser parser = new TextFieldParser(filepath);
parser.SetDelimiters(",");

while (!parser.EndOfData) {
    String[] currentTweet = parser.ReadFields();
    try
    {
        if (currentTweet[0] != null && currentTweet[1] != null && currentTweet[2] != null)
        {
            tweets.Add(currentTweet);
        }
        else
        {
            Console.WriteLine("Tweet had null parts");
        }
    }
    catch (IndexOutOfRangeException e)
    {
        Console.WriteLine(e.Message + "Tweets parts were not found");
    }
}

foreach (String[] tweet in tweets)
{
    try
    {
        int unixTime = int.Parse(tweet[2]);
        DateTime realTime = DateTimeOffset.FromUnixTimeSeconds(unixTime).UtcDateTime;
        Console.WriteLine(tweet[0] + " @ " + realTime + ": " + tweet[1]);
    }
    catch (FormatException ee)
    {
        Console.WriteLine(ee.Message + " Tried to parse invalid string to int");
        Console.WriteLine("");
    }
}
