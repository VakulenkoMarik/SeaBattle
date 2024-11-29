public enum Gamemode
{
    PvP,
    PvE,
    EvE,
}

public class Game
{
    private Gamemode gamemode;
    private int mapsSize = 7;

    private Random random = new();

    private Player player1;
    private Player player2;

    private Player attacker;
    private Player target;

    public void Start(Gamemode gm)
    {
        Init(gm);

        GameCycle();
    }

    private void GameCycle()
    {
        while (!IsEndGame())
        {
            InputProcessing();

            Logic();

            Draw();
        }

        OutputResults();
    }

    private void Init(Gamemode gamemode)
    {
        this.gamemode = gamemode;

        ConnectPlayers();

        attacker = player1;
        target = player2;

        GenerateMaps();

        Draw();
    }

    private void ConnectPlayers()
    {
        _ = (player1, player2) = gamemode switch
        {
            Gamemode.PvP => (new Player() { showShips = false, isHuman = true }, new Player() { showShips = false, isHuman = true }),
            Gamemode.PvE => (new Player() { showShips = false }, new Player()),
            Gamemode.EvE => (new Player(), new Player())
        };
    }

    private void GenerateMaps()
    {
        player1.GenerateField(mapsSize);
        player2.GenerateField(mapsSize);
    }

    private void Draw()
    {
        Console.Clear();

        Drawer.DrawMap(player1);
        Console.WriteLine(" ");
        Drawer.DrawMap(player2);
    }

    private void InputProcessing()
    {
        if (attacker.isHuman)
        {
            ManualInput();
            return;
        }
        
        (attacker.actionX, attacker.actionY) = GenerateXYShot();
    }

    private void ManualInput()
    {
        string? input = Console.ReadLine();

        if (input == "R")
        {
            if (attacker.CanUseRadar())
            {
                input = Console.ReadLine();

                attacker.usesRadar = true;
            }
        }

        (attacker.actionX, attacker.actionY) = ReadInput(input);
    }

    private (int, int) ReadInput(string? input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return (-1, -1);
        }

        int letterIndex = GetLetterIndex(input);
        int numIndex = GetNumIndex(input);

        return (letterIndex, numIndex);
    }

    private (int, int) GenerateXYShot()
    {
        int x, y;

        do
        {
            (x, y) = (random.Next(0, mapsSize), random.Next(0, mapsSize));
        }
        while (target.field.GetCell(x, y).isShot);

        return (x, y);
    }

    private int GetLetterIndex(string input)
    {
        char letter = input[0];
        
        if (letter >= 'a' && letter <= 'z' && letter - 'a' < mapsSize)
        {
            return letter - 'a';
        }

        return -1;
    }

    private int GetNumIndex(string input)
    {
        int num = 0;

        for (int i = 1; i < input.Length; i++)
        {
            if (char.IsDigit(input[i]))
            {
                num = num * 10 + (input[i] - '0');

                if (num > mapsSize)
                {
                    return -1;
                }
            }
            else
            {
                break;
            }
        }

        return num - 1;
    }

    private void Logic()
    {
        if (attacker.actionX < 0 || attacker.actionY < 0)
        {
            return;
        }

        Thread.Sleep(750);

        if (attacker.LastAttackOver(target))
        {
            NextPlayerMoves();
        }
    }

    private void NextPlayerMoves()
    {
        (attacker, target) = (target, attacker);
    }

    private bool IsEndGame()
    {
        if (player2.IsDefeat() || player1.IsDefeat())
        {
            return true;
        }

        return false;
    }

    private void OutputResults()
    {
        string result = BattleResult();

        Console.WriteLine($"\n{result}");
    }

    private string BattleResult()
    {
        if (player1.IsDefeat())
        {
            if (player2.IsDefeat())
            {
                return "Draw";
            }

            return "Player lost";
        }

        return "Enemy lost";
    }
}