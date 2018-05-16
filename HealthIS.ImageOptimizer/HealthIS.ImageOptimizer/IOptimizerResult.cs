using System.IO;

namespace HealthIS.Validations.Compression
{
    public interface IOptimizerResult
    {
        bool Success { get; }
        string ErrorMessage { get; }
        int SizeBefore { get; }
        int SizeAfter { get; }
        Stream CreateResultStream();
    }
}
