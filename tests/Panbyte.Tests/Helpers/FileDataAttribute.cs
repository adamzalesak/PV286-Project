using System.Reflection;
using Xunit.Sdk;

namespace Panbyte.Tests.Helpers;

public class FileDataAttribute : DataAttribute
{
    private readonly string _filePath;
    private readonly object[] _dataParams;

    public FileDataAttribute(string filePath, params object[] dataParams)
    {
        _filePath = filePath;
        _dataParams = dataParams;
    }

    public override IEnumerable<object[]> GetData(MethodInfo testMethod)
    {
        if (testMethod == null)
        {
            throw new ArgumentNullException(nameof(testMethod));
        }

        var paramList = new List<object>
        {
            FileHelper.ReadFile(_filePath)
        };
        paramList.AddRange(_dataParams);
        return new[] { paramList.ToArray() };
    }
}
