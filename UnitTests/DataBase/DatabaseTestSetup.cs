using Microsoft.Extensions.Logging;
using Moq;

namespace UnitTests.DataBase;

public class DatabaseTestSetup
{
    public DatabaseTestSetup(string dbName)
    {
        TestPath = Path.Combine(Path.GetTempPath(), dbName);
        if (!Directory.Exists(TestPath)) Directory.CreateDirectory(TestPath);
        else ClearDatabase();
    }

    public readonly string TestPath;

    public static Mock<ILogger<T>> GetMockLogger<T>()
    {
        return new Mock<ILogger<T>>();
    }

    public void ClearDatabase()
    {
        var di = new DirectoryInfo(TestPath);
        di.Delete(true);
        Directory.CreateDirectory(TestPath);
    }
    
}