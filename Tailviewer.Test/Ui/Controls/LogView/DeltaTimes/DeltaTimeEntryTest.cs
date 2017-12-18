﻿using System;
using System.Threading;
using FluentAssertions;
using NUnit.Framework;
using Tailviewer.Ui.Controls.LogView.DeltaTimes;

namespace Tailviewer.Test.Ui.Controls.LogView.DeltaTimes
{
	[TestFixture]
	[RequiresThread(ApartmentState.STA)]
	public sealed class DeltaTimeEntryTest
	{
		[Test]
		public void TestMagnitudeDays1()
		{
			var entry = new DeltaTimeEntry(TimeSpan.FromDays(1));
			entry.ToString().Should().Be("+1 day");

			entry = new DeltaTimeEntry(-TimeSpan.FromDays(1));
			entry.ToString().Should().Be("-1 day");
		}

		[Test]
		public void TestMagnitudeDays2()
		{
			var entry = new DeltaTimeEntry(TimeSpan.FromDays(2));
			entry.ToString().Should().Be("+2 days");

			entry = new DeltaTimeEntry(-TimeSpan.FromDays(2));
			entry.ToString().Should().Be("-2 days");
		}

		[Test]
		public void TestMagnitudeHours1()
		{
			var entry = new DeltaTimeEntry(TimeSpan.FromHours(1));
			entry.ToString().Should().Be("+1 hour");

			entry = new DeltaTimeEntry(-TimeSpan.FromHours(1));
			entry.ToString().Should().Be("-1 hour");
		}

		[Test]
		public void TestMagnitudeHours2()
		{
			var entry = new DeltaTimeEntry(TimeSpan.FromHours(2));
			entry.ToString().Should().Be("+2 hours");

			entry = new DeltaTimeEntry(-TimeSpan.FromHours(2));
			entry.ToString().Should().Be("-2 hours");
		}

		[Test]
		public void TestMagnitudeMinutes1()
		{
			var entry = new DeltaTimeEntry(TimeSpan.FromMinutes(1));
			entry.ToString().Should().Be("+1 min");

			entry = new DeltaTimeEntry(-TimeSpan.FromMinutes(1));
			entry.ToString().Should().Be("-1 min");
		}

		[Test]
		public void TestMagnitudeMinutes2()
		{
			var entry = new DeltaTimeEntry(TimeSpan.FromMinutes(2));
			entry.ToString().Should().Be("+2 min");

			entry = new DeltaTimeEntry(-TimeSpan.FromMinutes(2));
			entry.ToString().Should().Be("-2 min");
		}

		[Test]
		public void TestMagnitudeSeconds1()
		{
			var entry = new DeltaTimeEntry(TimeSpan.FromSeconds(1));
			entry.ToString().Should().Be("+1 sec");

			entry = new DeltaTimeEntry(-TimeSpan.FromSeconds(1));
			entry.ToString().Should().Be("-1 sec");
		}

		[Test]
		public void TestMagnitudeSeconds2()
		{
			var entry = new DeltaTimeEntry(TimeSpan.FromSeconds(2));
			entry.ToString().Should().Be("+2 sec");

			entry = new DeltaTimeEntry(-TimeSpan.FromSeconds(2));
			entry.ToString().Should().Be("-2 sec");
		}

		[Test]
		public void TestMagnitudeMilliseconds1()
		{
			var entry = new DeltaTimeEntry(TimeSpan.FromMilliseconds(1));
			entry.ToString().Should().Be("+1 ms");

			entry = new DeltaTimeEntry(-TimeSpan.FromMilliseconds(1));
			entry.ToString().Should().Be("-1 ms");
		}

		[Test]
		public void TestMagnitudeMilliseconds2()
		{
			var entry = new DeltaTimeEntry(TimeSpan.FromMilliseconds(2));
			entry.ToString().Should().Be("+2 ms");

			entry = new DeltaTimeEntry(-TimeSpan.FromMilliseconds(2));
			entry.ToString().Should().Be("-2 ms");
		}
	}
}