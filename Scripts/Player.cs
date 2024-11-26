public class Player
{
    public int actionX = -1, actionY = -1;

    public bool isHuman = false;
    public bool usesRadar = false;

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

    public void UseRadar(Player target)
    {
        usesRadar = false;
        radarsCount--;

        DrawRadarMap(target);
    }

    public bool CanTakeShot(Field fieldOfAttack)
    {
        return !fieldOfAttack.GetCell(actionX, actionY).isShot;
    }

    public void DrawRadarMap(Player player)
    {
        Console.Clear();

        player.DrawHiddenMap(actionX, actionY, 1);

        Thread.Sleep(5000);
    }

    public void DrawHiddenMap(int x, int y, int radius)
    {
        Drawer.DrawMap(field, isHuman, x, y, radius);
    }

    public void DrawMap()
    {
        Drawer.DrawMap(field, isHuman);
        Drawer.WriteTools(boatsCount, radarsCount, usesRadar, isHuman);
    }
}