public class Cell
{
    public bool isShot = false;
    public bool isShip = false;

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