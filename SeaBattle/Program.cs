class SeaBattle
{
    static Random random = new Random();

    // Shooting
    static int attempts = 20;
    static int shotX = -1, shotY = -1; // X --> A-E; Y --> 1-5

    // Boats
    static int boatsCount = 7;

    // Field
    static int fieldSize = 5;
    static char[,] hiddenField = {};
    static char[,] visualField = {};

    static void Main(string[] args)
    {
        CreateField();
        PlaceBoats();

        Draw();

        while (!IsEndGame())
        {
            GetInput();
            TryTakeShot();

            Draw();
        }
    }

    static void Draw()
    {
        Console.Clear();

        for (int i = 0; i < fieldSize; i++)
        {
            for (int j = 0; j < fieldSize; j++)
            {
                Console.Write(visualField[j, i]);
            }

            Console.WriteLine();
        }

        Console.WriteLine($"Attemps: {attempts} || Boats: {boatsCount}");
    }

    static void CreateField()
    {
        hiddenField = new char[fieldSize, fieldSize];
        visualField = new char[fieldSize, fieldSize];

        for (int i = 0; i < fieldSize; i++)
        {
            for (int j = 0; j < fieldSize; j++)
            {
                hiddenField[j, i] = '.';
                visualField[j, i] = '.';
            }
        }
    }

    static void PlaceBoats()
    {
        for(int i = 0; i < boatsCount; i++)
        {
            (int boatX, int boatY) = GenerateUniqueXY();

            hiddenField[boatX, boatY] = '#';
        }
    }

    static (int, int) GenerateUniqueXY()
    {
        int X = random.Next(0, fieldSize);
        int Y = random.Next(0, fieldSize);
        
        if (hiddenField[X, Y] == '#')
        {
            (X, Y) = GenerateUniqueXY();
        }

        return (X, Y);
    }

    static bool IsEndGame()
    {
        if (boatsCount <= 0) // Win
        {
            return true;
        }

        if (attempts <= 0 || boatsCount > attempts) // Fail
        {
            return true;
        }

        return false;
    }

    static void TryTakeShot()
    {
        if (CanTakeShot())
        {
            TakeShot();
        }
    }

    static bool CanTakeShot()
    {
        if ((shotX >= 0 && shotX < fieldSize) && (shotY >= 0 && shotY < fieldSize))
        {
            if (visualField[shotX, shotY] == '.')
            {
                return true;
            }
        }

        return false;
    }

    static void TakeShot()
    {
        if (hiddenField[shotX, shotY] == '#')
        {
            visualField[shotX, shotY] = 'X';
            boatsCount--;
        }
        else
        {
            visualField[shotX, shotY] = 'O';
        }

        attempts--;
    }

    static void GetInput()
    {
        string? inp = Console.ReadLine();

        if (string.IsNullOrEmpty(inp) || inp.Length < 2)
        {
            return;
        }

        shotX = inp[0] switch
        {
            'A' or 'a' => 0,
            'B' or 'b' => 1,
            'C' or 'c' => 2,
            'D' or 'd' => 3,
            'E' or 'e' => 4,
            _ => -1
        };

        shotY = int.TryParse(inp[1].ToString(), out int result) ? result - 1 : -1;
    }
}