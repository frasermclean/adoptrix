using Microsoft.Extensions.Logging;

namespace Adoptrix.Tests.Shared.Extensions;

public static class LoggerMockExtensions
{
    public static void VerifyLog<T>(
        this Mock<ILogger<T>> loggerMock,
        string expectedMessage,
        LogLevel expectedLogLevel = LogLevel.Information,
        Times? times = null)
    {
        times ??= Times.AtLeastOnce();

        loggerMock.Verify(
            x => x.Log(
                It.Is<LogLevel>(l => l == expectedLogLevel),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => CompareMessage(o, t, expectedMessage)),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)!), (Times)times);
    }

    private static bool CompareMessage(object? obj, Type _, string expectedMessage)
    {
        return obj?.ToString() == expectedMessage;
    }
}
