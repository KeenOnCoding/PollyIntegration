namespace Client
{
    public interface IServiceConfiguration
    {
        int RequestLifetime { get; }

        bool PropagateHeaders => true;
    }
}
