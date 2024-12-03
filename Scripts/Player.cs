public class Player
{
    public int actionX = -1, actionY = -1;

    public bool isHuman = false;
    public bool usesRadar = false;
    private bool endTurn = true;

    public int shipsCount { get; private set; } = 5;
    public int radarsCount { get; private set; } = 1;

    public int roundsWon = 0;

    public Field field = new();

    public void GenerateField(int mapSize)
    {
        field.GenerateMap(mapSize, shipsCount);
    }

    public void SetPlayerResources(int radars, int ships)
    {
        radarsCount = radars;
        shipsCount = ships;
    }

    public bool IsDefeat()
    {
        return shipsCount <= 0;
    }

    public bool CanUseRadar()
    {
        return radarsCount > 0 && !usesRadar;
    }

    public bool LastAttackOver(Player target)
    {
        endTurn = true;

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
        usesRadar = false;
        radarsCount--;

        endTurn = true;

        Drawer.DrawMap(target, 1, actionX, actionY);

        Thread.Sleep(5000);
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