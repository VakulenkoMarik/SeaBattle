using System.Collections;

public class Game
{
    private int mapsSize = 5;

    private int shotX = -1, shotY = -1;

    private Random random = new();

    private Player player = new() {isHuman = true};
    private Player enemy = new();

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
        string? input = Console.ReadLine();

        (shotX, shotY) = ReadInput(input);
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
        if (shotX < 0 || shotY < 0)
        {
            return;
        }

        if (player.CanTakeShot(enemy.field, shotX, shotY))
        {
            (int xEnemyShot, int yEnemyShot) = GenerateXYEnemyShot(player.field);

            enemy.GetShot(shotX, shotY);
            player.GetShot(xEnemyShot, yEnemyShot);
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

    private (int, int) GenerateXYEnemyShot(Field field)
    {
        int x, y;

        do
        {
            (x, y) = (random.Next(0, mapsSize), random.Next(0, mapsSize));
        }
        while (field.GetCell(x, y).isShot);

        return (x, y);
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