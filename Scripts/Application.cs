using System.Text.Json;

public class Application
{
    private const string FilePath = "gameData.json";

    private List<User> users = new List<User>();

    private User user1;
    private User user2;

    public void Start()
    {
        Console.WriteLine("! SEA BATTLE !");

        users = LoadGameData();

        StartOfNewBattle();
    }

    private void StartOfNewBattle()
    {
        UsersChoice();

        (Player player1, Player player2) = CreateNewPlayers();

        Game game = new();
        game.Start(player1, player2);

        AfterBattleProcessing(game.BattleWiner);

        SaveGameData();
    }

    private void UsersChoice()
    {
        user1 = SelectUser();
        user2 = SelectUser();

        ShowPlayerList();

        Thread.Sleep(1000);
    }
    
    private (Player, Player) CreateNewPlayers()
    {
        Player p1 = user1.CreateNewPlayer();
        Player p2 = user2.CreateNewPlayer();

        return (p1, p2);
    }

    private void AfterBattleProcessing(Player? winer)
    {
        if (winer != null)
        {
            _ = winer == user1.player ? user1.Wins++ : user2.Wins++;
        }
    }

    private User SelectUser()
    {
        User? user = null;

        while (user == null)
        {
            ShowPlayerList();

            user = InputOfUserSelect();
        }

        return user;
    }

    private void ShowPlayerList()
    {
        Console.Clear();

        Console.WriteLine("current users:");

        ShowUserData(user1);
        ShowUserData(user2);

        Console.WriteLine("\nSELECT A USER \nList of users:");

        for (int i = 0; i < users.Count; i++)
        {
            Console.WriteLine((i + 1) + $". {users[i].Name} ({users[i].Wins} wins) --- is human = {users[i].IsHuman}");
        }

        Console.WriteLine("\nOR CREATE A NEW ONE (C)");
    }

    private void ShowUserData(User user)
    {
        if (user != null)
        {
            Console.WriteLine(user.Name + (user.IsHuman ? " (is human)" : " (is not human)"));
        }
    }

    private User? InputOfUserSelect()
    {
        string? input = Console.ReadLine();

        if (string.IsNullOrEmpty(input))
        {
            return null;
        }
        else if (input[0] == 'C')
        {
            return CreatingNewUser();
        }

        int indexOfUser = GetNum(input);

        if (indexOfUser < 0 || indexOfUser >= users.Count)
        {
            return null;
        }

        return users[indexOfUser];
    }

    private int GetNum(string input)
    {
        int num = 0;

        for (int i = 0; i < input.Length; i++)
        {
            if (char.IsDigit(input[i]))
            {
                num = num * 10 + (input[i] - '0');
            }
            else
            {
                break;
            }
        }

        return num - 1;
    }

    private User CreatingNewUser()
    {
        (string name, bool isHuman) = InputUserData();

        User user = CreateUser(name, isHuman);
        users.Add(user);

        SaveGameData();

        return user;
    }

    private (string name, bool isHuman) InputUserData()
    {
        string? name;

        do
        {
            Console.WriteLine("Enter a username");
            name = Console.ReadLine();
        }
        while (string.IsNullOrEmpty(name));
        
        bool isHuman = false;
        bool isValidBoolInput = false;

        while (!isValidBoolInput)
        {
            Console.WriteLine("Is human? (true/false)");
            string? isHumanInput = Console.ReadLine();

            isValidBoolInput = bool.TryParse(isHumanInput, out isHuman);
        }

        return (name, isHuman);
    }

    private User CreateUser(string name, bool isHuman)
    {
        User user = new User(name, isHuman);

        return user;
    }

    private void SaveGameData()
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        string jsonData = JsonSerializer.Serialize(users, options);
        File.WriteAllText(FilePath, jsonData);
    }

    private List<User> LoadGameData()
    {
        if (!File.Exists(FilePath))
        {
            return [];
        }

        string jsonData = File.ReadAllText(FilePath);

        return JsonSerializer.Deserialize<List<User>>(jsonData);
    }
}