using System.Text;
using NotEnoughLogs;
using NotEnoughLogs.Sinks;

namespace SoundShapesServer.Tests.Server;

// thanks jvyden 👍👍👍👍
public class NUnitSink : ILoggerSink
{
    public void Log(LogLevel level, ReadOnlySpan<char> category, ReadOnlySpan<char> content)
    {
        TextWriter stream = level switch
        {
            LogLevel.Critical => TestContext.Error,
            LogLevel.Error => TestContext.Error,
            LogLevel.Warning => TestContext.Progress,
            LogLevel.Info => TestContext.Progress,
            LogLevel.Debug => TestContext.Progress,
            LogLevel.Trace => TestContext.Progress,
            _ => throw new ArgumentOutOfRangeException(nameof(level), level, null),
        };
        
        StringBuilder iHateMicrosoftApis = new();
        iHateMicrosoftApis.Append('[');
        iHateMicrosoftApis.Append(level.ToString());
        iHateMicrosoftApis.Append(']');
        iHateMicrosoftApis.Append(' ');
            
        iHateMicrosoftApis.Append('[');
        iHateMicrosoftApis.Append(category);
        iHateMicrosoftApis.Append(']');
        iHateMicrosoftApis.Append(' ');
            
        iHateMicrosoftApis.Append(content);

        stream.WriteLine(iHateMicrosoftApis.ToString());
    }

    public void Log(LogLevel level, ReadOnlySpan<char> category, ReadOnlySpan<char> format, params object[] args)
    {
        this.Log(level, category, string.Format(format.ToString(), args));
    }
}