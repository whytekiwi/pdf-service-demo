public record FeaturedEvent(string title, string description)
{
    public static FeaturedEvent FromEvent(Event e) => new FeaturedEvent(e.title, e.description);
}