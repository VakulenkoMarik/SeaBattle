public class Game
{
    bool isEndGame = false;

    int playerBoatsCount = 5;
    int enemyBoatsCount = 5;
    int mapsSize = 5;

    int shotX = -1, shotY = -1;

    Random random = new Random();
    Field playerField = new() {isPlayer = true};
    Field enemyField = new();

    public void Start()
    {
        GameCycle();
    }

    void GameCycle()
    {
        GenerateMaps();

        while (!IsEndGame())
        {
            Logic();
        }

        OutputResults();
    }

    void Logic()
    {
        if (!TryAttack(playerField, enemyField)) return;

        TryAttack(enemyField, playerField);
    }

    bool TryAttack(Field attackedField, Field target)
    {
        bool isHit = false;

        do
        {
            Draw();

            DefineInputData(attackedField);

            if (shotX < 0 || shotY < 0 || IsEndGame()) return false;

            if (attackedField.CanTakeShot(target, shotX, shotY))
            {
                attackedField.TakeShot(target, shotX, shotY);

                isHit = target.cells[shotX, shotY].isBoat;
            }
            else
            {
                return false;
            }

            CheckEnd();

        } while (isHit == true && !IsEndGame());

        return true;
    }

    void DefineInputData(Field field)
    {
        if (field.isPlayer)
        {
            GetInput();
        }
        else
        {
            (shotX, shotY) = GenerateXYEnemyShot(playerField);
        }
    }

    void CheckEnd()
    {
        if (enemyField.IsDefeat() || playerField.IsDefeat())
        {
            isEndGame = true;
        }
    }

    void GenerateMaps()
    {
        playerField.GenerateMap(mapsSize, playerBoatsCount);
        enemyField.GenerateMap(mapsSize, enemyBoatsCount);
    }

    void Draw()
    {
        Console.Clear();

        DrawMap(playerField);
        DrawMap(enemyField);
    }

    void DrawMap(Field field)
    {
        Console.Write("  ");

        for (char c = 'a'; c < 'a' + field.MapSize; c++)
        {
            Console.Write(c + "");
        }

        for (int i = 0; i < field.MapSize; i++)
        {
            Console.Write($"\n{i + 1} ");

            for (int j = 0; j < field.MapSize; j++)
            {
                Console.Write(GetCell(field, j, i));
            }
        }

        Console.WriteLine($"\n Boats: {field.boatsCount}\n");
    }

    char GetCell(Field field, int x, int y)
    {
        if (field.cells[x,y].isBoat && field.cells[x,y].isShoted)
        {
            return 'X';
        }
        else if (field.cells[x,y].isShoted)
        {
            return 'O';
        }
        
        // Player
        if (field.cells[x,y].isBoat && !field.cells[x,y].isShoted && field.isPlayer)
        {
            return '#';
        }

        return '.';
    }

    void GetInput()
    {
        string? input = Console.ReadLine();

        (shotX, shotY) = ReadInput(input);
    }

    (int, int) ReadInput(string? input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return (-1, -1);
        }

        int letterIndex = GetLetterIndex(input);
        int numIndex = GetNumIndex(input);

        return (letterIndex, numIndex);
    }

    int GetLetterIndex(string input)
    {
        char letter = input[0];
        
        if (letter >= 'a' && letter <= 'z' && letter - 'a' < mapsSize)
        {
            return letter - 'a';
        }

        return -1;
    }

    int GetNumIndex(string input)
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

    bool IsEndGame()
    {
        return isEndGame;
    }

    (int, int) GenerateXYEnemyShot(Field field)
    {
        int x = random.Next(0, mapsSize);
        int y = random.Next(0, mapsSize);

        if (field.cells[x, y].isShoted) 
        {
            return GenerateXYEnemyShot(field);
        }

        return (x, y);
    }

    void OutputResults()
    {
        string result = BattleResult();

        Console.WriteLine($"\n{result}");
    }

    string BattleResult()
    {
        if (playerField.IsDefeat())
        {
            if (enemyField.IsDefeat())
            {
                return "Draw";
            }

            return "Player lost";
        }

        return "Enemy lost";
    }
}

class Field
{
    Random random = new Random();
    public Cell[,] cells = {};

    int mapSize;
    public int boatsCount;
    public bool isPlayer;
    
    public int MapSize { get {return mapSize;} private set {} }

    public void GenerateMap(int mapSize, int boatsCount)
    {
        if (mapSize * mapSize < boatsCount)
        {
            boatsCount = mapSize * mapSize - 1;
        }

        this.boatsCount = boatsCount;
        this.mapSize = mapSize;

        cells = new Cell[mapSize, mapSize];

        for (int i = 0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++)
            {
                cells[i, j] = new Cell();
            }
        }

        PlaceBoats();
    }

    void PlaceBoats()
    {
        for (int i = 0; i < boatsCount; i++)
        {
            (int x, int y) = GenerateUniqueXY();

            cells[x, y].isBoat = true;
        }
    }

    (int, int) GenerateUniqueXY()
    {
        int x = random.Next(0, mapSize);
        int y = random.Next(0, mapSize);

        if (cells[x, y].isBoat)
        {
            return GenerateUniqueXY();
        }

        return (x, y);
    }

    public bool CanTakeShot(Field fieldOfAttack, int x, int y)
    {
        if (fieldOfAttack.cells[x, y].isShoted)
        {
            return false;
        }

        return true;
    }

    public void TakeShot(Field fieldOfAttack, int x, int y)
    {
        if (!fieldOfAttack.cells[x, y].isShoted)
        {
            if (fieldOfAttack.cells[x, y].isBoat)
            {
                fieldOfAttack.boatsCount--;
            }

            fieldOfAttack.cells[x, y].isShoted = true;
        }
    }

    public bool IsDefeat()
    {
        if (boatsCount <= 0)
        {
            return true;
        }

        return false;
    }
}

public class Cell
{
    public Cell()
    {
        isBoat = false;
        isShoted = false;
    }

    public bool isShoted;
    public bool isBoat;
}