public enum Gamemode
{
    PvP,
    PvE,
    EvE,
}

public class Game
{
    private Gamemode gamemode;

    private int mapsSize = 5;
    private int rounds = 3;
    private int currentRound = 0;

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
            StartNewRound();
        }

        EndResultsProcesing();
    }

    private void Init(Gamemode gamemode)
    {
        this.gamemode = gamemode;
        Drawer.SetDrawerMode(gamemode);

        SetPlayers();
    }

    public void StartNewRound()
    {
        InitRound();

        RoundCycle();
    }

    private void InitRound()
    {
        currentRound++;

        attacker = player1;
        target = player2;

        ResourcesToPlayers();

        GenerateMaps();

        Draw();
    }

    private void ResourcesToPlayers()
    {
        int radars = 1;
        int ships = 5;

        player1.SetPlayerResources(radars, ships);
        player2.SetPlayerResources(radars, ships);
    }

    private void RoundCycle()
    {
        while (!IsEndRound())
        {
            InputProcessing();

            Logic();

            Draw();
        }

        RoundResultProcesing();
    }

    private void SetPlayers()
    {
        (player1, player2) = gamemode switch
        {
            Gamemode.PvP => (MakeHumanPlayer(), MakeHumanPlayer()),
            Gamemode.PvE => (MakeHumanPlayer(), MakeBotPlayer()),
            Gamemode.EvE => (MakeBotPlayer(), MakeBotPlayer()),
            _ => (player1, player2)
        };
    }

    private Player MakeHumanPlayer()
    {
        return new() { isHuman = true };
    }

    private Player MakeBotPlayer()
    {
        return new();
    }

    private void GenerateMaps()
    {
        player1.GenerateField(mapsSize);
        player2.GenerateField(mapsSize);
    }

    private void Draw()
    {
        Console.Clear();

        Drawer.DrawFullMap(player1);
        Console.WriteLine(" ");
        Drawer.DrawFullMap(player2);
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

    private bool IsEndRound()
    {
        return player2.IsDefeat() || player1.IsDefeat();
    }

    private bool IsEndGame()
    {
        return currentRound >= rounds;
    }

    private void EndResultsProcesing()
    {
        string outputText = "BATTLE WINER IS ";

        outputText += player1.roundsWon > player2.roundsWon ? "PLAYER 1" : "PLAYER 2";

        Drawer.DrawOnlyText(outputText);
    }

    private void RoundResultProcesing()
    {
        Thread.Sleep(2000);

        Player? winer = CheckRoundWiner();

        if (winer != null)
        {
            winer.roundsWon++;
        }
        
        OutputRoundResult(winer);
    }

    private void OutputRoundResult(Player? winer)
    {
        string endText = "";

        if (winer == null)
        {
            endText = "Draw";
        }
        else
        {
            endText = $"Player {(winer == player1 ? 1 : 2)} is winer   ({player1.roundsWon} / {player2.roundsWon})";
        }

        Drawer.DrawOnlyText(endText, 2000);
    }

    private Player? CheckRoundWiner()
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