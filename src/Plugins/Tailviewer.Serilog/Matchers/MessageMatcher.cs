﻿using System;
using System.Text.RegularExpressions;
using Tailviewer.BusinessLogic.LogFiles;
using Tailviewer.Core.LogFiles;

namespace Tailviewer.Serilog.Matchers
{
	public sealed class MessageMatcher
		: ISerilogMatcher
	{
		private readonly int _groupIndex;

		public MessageMatcher(string specifier, int groupIndex)
		{
			if (!string.IsNullOrEmpty(specifier))
				throw new NotImplementedException($"Message specifiers are not implemented yet: {specifier}");
			_groupIndex = groupIndex;
		}

		#region Implementation of ISerilogMatcher

		public string Regex
		{
			get { return "([^\n]*)"; }
		}

		public int NumGroups
		{
			get { return 1; }
		}

		public ILogFileColumn Column
		{
			get { return LogFileColumns.Message; }
		}

		public void MatchInto(Match match, SerilogEntry logEntry)
		{
			logEntry.Message = match.Groups[_groupIndex].Value;
		}

		#endregion
	}
}