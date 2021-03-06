﻿using System;

namespace Tailviewer.Core
{
	/// <summary>
	///     Methods which suplement the <see cref="Math" /> class.
	/// </summary>
	public static class MathEx
	{
		/// <summary>
		///     Clamps the given <paramref name="value" /> to the range of [0, 1].
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static double Saturate(double value)
		{
			return Clamp(value, min: 0, max: 1);
		}

		/// <summary>
		///     Clamps the given <paramref name="value" /> to the range of [<paramref name="min" />, <paramref name="max" />].
		/// </summary>
		/// <param name="value"></param>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <returns></returns>
		public static double Clamp(double value, double min, double max)
		{
			return Math.Max(Math.Min(value, max), min);
		}
	}
}