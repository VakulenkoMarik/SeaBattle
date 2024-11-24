public class Cell
{
    public Cell()
    {
        isShip = false;
        isShot = false;
    }

    public bool isShot;
    public bool isShip;

    public char GetCellSymbol(bool isHuman)
    {
        if (isShip && isShot)
        {
            return 'X';
        }
        
        if (isShot)
        {
            return 'O';
        }
        
        // Player
        if (isShip && isHuman)
        {
            return '#';
        }

        return '.';
    }
}