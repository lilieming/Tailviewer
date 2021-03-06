# Why

You want to develop a log file format matcher plugin because:

- You want to change the way tailviewer treats a particular type of files
- You want to develop a plugin for a particular type of file that tailviewer doesn't already recognize

# Where

If you open a particular file with tailviewer (either by opening it directly or by dragging it over tailviewer),
then tailviewer will try to identify the file being opened. This is done by inspecting both the file path (including
its extension) as well as its content (if available). Tailviewer will query all available `ILogFileFormatMatcherPlugin`
implementations to identify the file. The resulting format (or `LogFileFormats.Generic` in case no match is found) will be assigned to the log file's `LogFileProperties.Format` property.

Tailviewer might call plugins multiple times on the same file in case the file's contents have changed and previously plugins did not positively
identify the file's format.

# How

You need to implement three different interfaces in order to identify a file format for tailviewer:

- `ILogFileFormatMatcherPlugin` => Plumbing
- `ILogFileFormatMatcher` => Responsible for testing if a file is of a particular format
- `ILogFileFormat` => Responsible for describing a format

```
public class MyCustomFileFormatMatcherPlugin
  : ILogFileFormatMatcherPlugin
{
  public ILogFileFormatMatcher CreateMatcher(IServiceContainer services)
  {
    return new MyCustomFileFormatMatcher();
  }
}
```

```
class MyCustomFileFormatMatcher
  : ILogFileFormatMatcher
{
  public bool TryMatchFormat(string fileName,
		                         byte[] initialContent,
		                         out ILogFileFormat format)
  {
    if (fileName.EndsWith(".db", StringComparison.CurrentCultureIgnoreCase)
    {
      format = new MyCustomFileFormat();
      return true;
    }
    
    format = null;
    return false;
  }
}
```

```
sealed class MyCustomFormat
  : ILogFileFormat
{
  public string Name => "My custom format";
  public string Description => "Format developed by my company to log messages";
  public bool IsText => true;
  public Encoding Encoding => Encoding.GetEncoding("CP895");
  public override bool Equals(object other)
  {
  	return other is MyCustomFormat;
  }
}
```

Please note that in case `ILogFileFormatMatcher.TryMatchFormat` returns true,
the value of the `format` parameter will be stored as the log file's `LogFileProperties.Format` property and be used
to find out which plugins are applicable for this log file.

Newer plugin interfaces, such as `ITextLogFileParserPlugin`, usually provide a `SupportedFormats` property which returns a list
of `ILogFileFormat` objects. When the time comes to use a particular plugin, tailviewer queries all implementation sof that plugin and tries to find the best match, i.e. a plugin which purports to support that particular format.
Two formats are compared for equality by using `Object.Equals(Object, Object)`.
