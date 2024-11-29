public class Player
{
    public int actionX = -1, actionY = -1;

    public bool isHuman = false;
    public bool usesRadar = false;
    private bool endTurn = true;

    public int boatsCount = 5;
    public int radarsCount = 1;

    public Field field = new();

    public void GenerateField(int mapSize)
    {
        field.GenerateMap(mapSize, boatsCount);
    }

    public bool IsDefeat()
    {
        return boatsCount <= 0;
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

        return endTurn;
    }

    private void RadarEspionage(Player target)
    {
        usesRadar = false;
        radarsCount--;

        endTurn = true;

        Drawer.DrawMap(target, false, 1, actionX, actionY);

        Thread.Sleep(5000);
    }

    private void ProcessShot(Player target)
    {
        Cell targetCell = target.field.GetCell(actionX, actionY);
        targetCell.GetShot();

        if (targetCell.isShip)
        {
            target.boatsCount--;
            endTurn = false;

            return;
        }
    }

    public bool CanTakeShot(Field fieldOfAttack)
    {
        return !fieldOfAttack.GetCell(actionX, actionY).isShot;
    }
}