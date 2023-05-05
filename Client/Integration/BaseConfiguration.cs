namespace Client
{
    public class BaseConfiguration : IServiceConfiguration
    {
        public string BasePath { get; set; }

        public int RequestLifetime { get; set; }

        public Dictionary<string, string> DefaultHeaders { get; set; }

        public int RetryCount { get; set; }

        public double RetryBase { get; set; }

        public List<int> RetryStatusCodes { get; set; }

        public int RetryFractionalDigits { get; set; } = 4;
        public bool PropagateHeaders { get; set; }
    }
}
