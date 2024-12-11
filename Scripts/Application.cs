using System.Text.Json;

public class Application
{
    private const string FilePath = "gameData.json";

    private List<User> players = new List<User>();
    private Gamemode gamemode = Gamemode.PvE;

    public void Start()
    {
        players = LoadGameData(FilePath);

        SaveGameData(players, FilePath);
    }

    private void CreateUser(string name, bool isHuman)
    {
        players.Add(new User(name)
        {
            IsHuman = isHuman,
            Wins = 0
        });
    }

    private void SaveGameData(List<User> players, string filePath)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        string jsonData = JsonSerializer.Serialize(players, options);
        File.WriteAllText(filePath, jsonData);
    }

    private List<User> LoadGameData(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return new List<User>();
        }

        string jsonData = File.ReadAllText(filePath);

        return JsonSerializer.Deserialize<List<User>>(jsonData);
    }
}