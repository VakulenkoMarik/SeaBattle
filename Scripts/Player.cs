public class Player
{
    public int actionX = -1, actionY = -1;

    public bool isHuman = false;
    private bool endTurn = true;

    public int shipsCount = 5;

    public Field field = new();

    public void GenerateField(int mapSize)
    {
        field.GenerateMap(mapSize, shipsCount);
    }

    public bool IsDefeat()
    {
        return shipsCount <= 0;
    }

    public bool LastAttackOver(Player target)
    {
        endTurn = true;

        if (CanTakeShot(target.field))
        {
            ProcessShot(target);
        }
        else
        {
            endTurn = false;
        }

        return endTurn;
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