namespace Sharpener.Injection.Tests.Models;

public class ComplexService(ITestService testService, ITestRepository testRepository) : IComplexService
{
    public string Process()
    {
        return $"{testService.GetMessage()}-{testRepository.GetData()}";
    }
}
