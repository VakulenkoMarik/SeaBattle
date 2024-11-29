public class Game
{
    private int mapsSize = 7;

    private Random random = new();

    private Player player = new() {isHuman = true};
    private Player enemy = new();

    private Player attacker;
    private Player target;

    public void Start()
    {
        GameCycle();
    }

    private void GameCycle()
    {
        Init();

        while (!IsEndGame())
        {
            InputProcessing();

            Logic();

            Draw();
        }

        OutputResults();
    }

    private void Init()
    {
        attacker = player;
        target = enemy;

        GenerateMaps();

        Draw();
    }

    private void GenerateMaps()
    {
        player.GenerateField(mapsSize);
        enemy.GenerateField(mapsSize);
    }

    private void Draw()
    {
        Console.Clear();

        Drawer.DrawMap(player);
        Drawer.DrawMap(enemy);
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
        if (enemy.IsDefeat() || player.IsDefeat())
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
        if (player.IsDefeat())
        {
            if (enemy.IsDefeat())
            {
                return "Draw";
            }

            return "Player lost";
        }

        return "Enemy lost";
    }
}