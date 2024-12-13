public enum Gamemode
{
    PvP,
    PvE,
    EvE,
}

public class Round
{
    private int mapsSize = 5;

    private Random random = new();
    private Drawer drawer;

    private Player player1;
    private Player player2;

    private Player attacker;
    private Player target;

    public Player? Winer { get; private set; }

    public void Start(Gamemode gm, Player p1, Player p2)
    {
        Init(gm, p1, p2);

        RoundCycle();
    }

    private void RoundCycle()
    {
        while (!IsEndGame())
        {
            InputProcessing();

            Logic();

            Draw();
        }

        ResultsProcessing();
    }

    private void Init(Gamemode gamemode, Player p1, Player p2)
    {
        ConnectPlayers(p1, p2);
        
        drawer = new Drawer(
            gamemode,
            player1,
            player2
        );

        attacker = player1;
        target = player2;

        GenerateMaps();

        Draw();
    }

    private void ConnectPlayers(Player p1, Player p2)
    {
        (player1, player2) = (p1, p2);

        player1.ResetValues();
        player2.ResetValues();
    }

    private void GenerateMaps()
    {
        player1.GenerateField(mapsSize);
        player2.GenerateField(mapsSize);
    }

    private void Draw()
    {
        Console.Clear();

        drawer.DrawFields();
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

    private void ResultsProcessing()
    {
        Thread.Sleep(1000);
        
        Player? winer = CheckWiner();

        if (winer != null)
        {
            Winer = winer;
        }
    }

    private Player? CheckWiner()
    {
        if (player1.IsDefeat())
        {
            if (player2.IsDefeat())
            {
                return null;
            }

            return player2;
        }

        return player1;
    }
}