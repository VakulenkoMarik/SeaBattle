public class Cell
{
    public Cell()
    {
        isShip = false;
        isShot = false;
    }

    public bool isShot;
    public bool isShip;

    public void GetShot()
    {
        isShot = true;
    }

    public char GetCellSymbol(bool showShips)
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
        if (isShip && showShips)
        {
            return '#';
        }

        return '.';
    }
}