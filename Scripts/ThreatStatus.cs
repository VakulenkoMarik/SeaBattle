public class ThreatStatus()
{
    public bool isRadarAttack = false;
    private int radarX, radarY;
    private int radarRadius = 1;

    public void ResetInfo()
    {
        isRadarAttack = false;
        radarX = -1;
        radarY = -1;
    }

    public void SetRadarAttack(int x, int y)
    {
        isRadarAttack = true;
        radarX = x;
        radarY = y;
    }

    public (int x, int y, int radius) GetRadarData()
    {
        return (radarX, radarY, radarRadius);
    }
}