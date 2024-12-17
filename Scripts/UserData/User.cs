public record User(string Name, bool IsHuman)
{
    public int Wins { get; set; } = 0;
    public int Games { get; set; } = 0;
    public double Winrate { get; set; } = 0;
}