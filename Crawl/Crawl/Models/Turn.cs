namespace Crawl.Models
{
    // Types of Hits during a Turn.
    public enum HitStatusEnum
    {
        Unknown = 0,
        Hit = 10,
        CriticalHit = 15,
        Miss = 20,
        CriticalMiss = 25,
    }
}
