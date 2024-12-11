public class Player
{
    public int actionX = -1, actionY = -1;

    public bool isHuman = false;
    public bool usesRadar = false;
    private bool endTurn = true;

    public int shipsCount = 5;
    public int radarsCount = 1;

    public Field field = new();
    public ThreatStatus Threat { get; private set; } = new ThreatStatus();

    public void GenerateField(int mapSize)
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
        radarsCount = 1;
        shipsCount = 5;

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