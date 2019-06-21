namespace Rocket.Surgery.Extensions.Marten
{
    public interface IMartenContext
    {
        IMartenUser User { get; set; }
    }
    class MartenContext : IMartenContext
    {
        public IMartenUser User { get; set; }
    }
}
