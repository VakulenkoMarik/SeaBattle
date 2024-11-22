public class Game
{
    private int mapsSize = 5;

    private Random random = new();

    private Player player = new() {isHuman = true};
    private Player enemy = new();

    private Player attacker;
    private Player target;

    public Game()
    {
        attacker = player;
        target = enemy;
    }

    public void Start()
    {
        GameCycle();
    }

    private void GameCycle()
    {
        GenerateMaps();

        Draw();

        while (!IsEndGame())
        {
            InputProcessing();

            Logic();

            Draw();
        }

        OutputResults();
    }

    private void GenerateMaps()
    {
        player.GenerateField(mapsSize);
        enemy.GenerateField(mapsSize);
    }

    private void Draw()
    {
        Console.Clear();

        player.DrawMap();
        enemy.DrawMap();
    }

    private void InputProcessing()
    {
        if (attacker.isHuman)
        {
            ManualInput();
            return;
        }
        
        (attacker.shotX, attacker.shotY) = GenerateXYShot();
    }

    private void ManualInput()
    {
        string? input = Console.ReadLine();

        (attacker.shotX, attacker.shotY) = ReadInput(input);
    }

    private (int, int) GenerateXYShot()
    {
        int x, y;

        do
        {
            (x, y) = (random.Next(0, mapsSize), random.Next(0, mapsSize));
        }
        while (player.field.GetCell(x, y).isShot);

        return (x, y);
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
        Thread.Sleep(750);

        if (attacker.CanTakeShot(target.field))
        {
            if (!target.DevastatingShot(attacker.shotX, attacker.shotY))
            {
                (attacker, target) = (target, attacker);
            }
        }
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