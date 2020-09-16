﻿using System.Linq;
using Tailviewer.BusinessLogic.LogFiles;
using Tailviewer.BusinessLogic.Plugins;

namespace Tailviewer.Serilog
{
	/// <summary>
	///     Responsible for finding out if a particular log file matches any of the well known serilog formats.
	/// </summary>
	public sealed class SerilogFileFormatMatcher
		: ILogFileFormatMatcher
	{
		private readonly ILogFileFormatRegistry _registry;

		public SerilogFileFormatMatcher(ILogFileFormatRegistry registry)
		{
			_registry = registry;
		}

		#region Implementation of ILogFileFormatMatcher

		public bool TryMatchFormat(string fileName, byte[] initialContent, out ILogFileFormat format)
		{
			var formats = _registry.Formats.OfType<SerilogFileFormat>();

			foreach (var serilogFormat in formats)
			{
				var parser = serilogFormat.Parser;
				var content = serilogFormat.Encoding.GetString(initialContent);
				if (parser.TryParse(content, out _))
				{
					format = serilogFormat;
					return true;
				}
			}

			format = null;
			return false;
		}

		#endregion
	}
}