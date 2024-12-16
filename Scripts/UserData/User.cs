public record User(string Name, bool IsHuman)
{
    public int Wins { get; set; } = 0;
}