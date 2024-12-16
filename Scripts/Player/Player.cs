public class Player(string name, bool isHuman)
{
    public string Name { get; private set; } = name;
    public bool IsHuman { get; private set; } = isHuman;

    public int actionX = -1, actionY = -1;
    
    public bool usesRadar = false;
    private bool endTurn = true;

    public int shipsCount;
    public int radarsCount;
    private DefaultPlayerValues defaultValues;

    public Field field = new();
    public ThreatStatus Threat { get; private set; } = new ThreatStatus();

    public void CreatingMapProcessing(int mapSize, int ships, int radars)
    {
        SetDefaultValues(mapSize, ships, radars);

        GenerateField(mapSize);
    }

    private void SetDefaultValues(int mapSize, int shipsCount, int radarsCount)
    {
        int mapArea = mapSize * mapSize;

        if (shipsCount > mapArea)
        {
            shipsCount = mapArea - 1;
        }

        defaultValues = new(shipsCount, radarsCount);

        this.shipsCount = shipsCount;
        this.radarsCount = radarsCount;
    }

    private void GenerateField(int mapSize)
    {
        field.GenerateMap(mapSize, shipsCount);
    }

    public bool IsDefeat()
    {
        return shipsCount <= 0;
    }

    public bool CanUseRadar()
    {
        return radarsCount > 0;
    }

    public void ResetValues()
    {
        Threat.ResetInfo();
        
        shipsCount = defaultValues.shipsCount;
        radarsCount = defaultValues.radarsCount;

        endTurn = true;
        usesRadar = false;
    }

    public bool LastAttackOver(Player target)
    {
        endTurn = true;
        
        target.Threat.ResetInfo();

        if (usesRadar)
        {
            RadarEspionage(target);
        }
        else if (CanTakeShot(target.field))
        {
            ProcessShot(target);
        }
        else
        {
            endTurn = false;
        }

        return endTurn;
    }

    private void RadarEspionage(Player target)
    {
        radarsCount--;

        target.Threat.SetRadarAttack(actionX, actionY);
        usesRadar = false;
    }

    private void ProcessShot(Player target)
    {
        Cell targetCell = target.field.GetCell(actionX, actionY);
        targetCell.GetShot();

        if (targetCell.isShip)
        {
            target.shipsCount--;
            endTurn = false;
        }
    }

    public bool CanTakeShot(Field fieldOfAttack)
    {
        return !fieldOfAttack.GetCell(actionX, actionY).isShot;
    }
}

public struct DefaultPlayerValues(int shipsCount, int radarsCount)
{
    public int shipsCount = shipsCount;
    public int radarsCount = radarsCount;
}