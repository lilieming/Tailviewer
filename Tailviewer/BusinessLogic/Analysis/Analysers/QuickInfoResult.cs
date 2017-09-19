﻿namespace Tailviewer.BusinessLogic.Analysis.Analysers
{
	public sealed class QuickInfoResult
		: ILogAnalysisResult
	{
		public long Count { get; set; }

		public object Clone()
		{
			return new QuickInfoResult {Count = Count};
		}
	}
}