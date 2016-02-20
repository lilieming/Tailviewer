﻿using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using Tailviewer.BusinessLogic;
using Tailviewer.BusinessLogic.DataSources;
using DataSource = Tailviewer.Settings.DataSource;
using DataSources = Tailviewer.Settings.DataSources;

namespace Tailviewer.Test.BusinessLogic
{
	[TestFixture]
	public sealed class DataSourcesTest
	{
		private DataSources _settings;
		private Tailviewer.BusinessLogic.DataSources.DataSources _dataSources;

		[SetUp]
		public void SetUp()
		{
			_settings = new DataSources();
			_dataSources = new Tailviewer.BusinessLogic.DataSources.DataSources(_settings);
		}

		[TearDown]
		public void TearDown()
		{
			_dataSources.Dispose();
		}

		[Test]
		[Description("Verifies that a business object is created for every data source")]
		public void TestCtor1()
		{
			var settings = new DataSources
				{
					new DataSource(@"E:\Code\test.log")
						{
							Id = Guid.NewGuid()
						}
				};
			using (var dataSources = new Tailviewer.BusinessLogic.DataSources.DataSources(settings))
			{
				dataSources.Count.Should().Be(1);
				var dataSource = dataSources.First();
				dataSource.FullFileName.Should().Be(settings[0].File);
				dataSource.Id.Should().Be(settings[0].Id);
			}
		}

		[Test]
		[Description("Verifies that data sources are added to a parent, when specified")]
		public void TestCtor2()
		{
			var settings = new DataSources
				{
					new DataSource("test1.log")
						{
							Id = Guid.NewGuid()
						},
					new DataSource("test2.log")
						{
							Id = Guid.NewGuid()
						},
					new DataSource("test.log")
						{
							Id = Guid.NewGuid()
						}
				};
			var merged = new DataSource{Id = Guid.NewGuid()};
			settings.Add(merged);
			settings[0].ParentId = merged.Id;
			settings[1].ParentId = merged.Id;

			using (var dataSources = new Tailviewer.BusinessLogic.DataSources.DataSources(settings))
			{
				dataSources.Count.Should().Be(4, "Because we've loaded 4 data sources");
				var mergedDataSource = dataSources[3] as MergedDataSource;
				mergedDataSource.Should().NotBeNull();
				mergedDataSource.DataSourceCount.Should().Be(2, "Because 2 of the data sources are part of this group");
				var dataSource1 = dataSources[0];
				var dataSource2 = dataSources[1];

				mergedDataSource.DataSources.Should().Equal(new object[] {dataSource1, dataSource2});
				dataSource1.ParentId.Should().Be(merged.Id);
				dataSource2.ParentId.Should().Be(merged.Id);
			}
		}

		[Test]
		[Description("Verifies that data sources are added to the correct parent")]
		public void TestCtor3()
		{
			var settings = new DataSources
				{
					new DataSource("test1.log")
						{
							Id = Guid.NewGuid()
						},
					new DataSource("test2.log")
						{
							Id = Guid.NewGuid()
						},
					new DataSource("test.log")
						{
							Id = Guid.NewGuid()
						}
				};
			var merged1 = new DataSource { Id = Guid.NewGuid() };
			settings.Add(merged1);
			var merged2 = new DataSource { Id = Guid.NewGuid() };
			settings.Add(merged2);
			var merged3 = new DataSource { Id = Guid.NewGuid() };
			settings.Add(merged3);
			settings[0].ParentId = merged1.Id;
			settings[1].ParentId = merged2.Id;
			settings[2].ParentId = merged3.Id;

			using (var dataSources = new Tailviewer.BusinessLogic.DataSources.DataSources(settings))
			{
				dataSources.Count.Should().Be(6, "Because we've loaded 6 data sources");
				var mergedDataSource1 = dataSources[3] as MergedDataSource;
				mergedDataSource1.Should().NotBeNull();
				mergedDataSource1.DataSources.Should().Equal(new object[] { dataSources[0] });

				var mergedDataSource2 = dataSources[4] as MergedDataSource;
				mergedDataSource2.Should().NotBeNull();
				mergedDataSource2.DataSources.Should().Equal(new object[] { dataSources[1] });

				var mergedDataSource3 = dataSources[5] as MergedDataSource;
				mergedDataSource3.Should().NotBeNull();
				mergedDataSource3.DataSources.Should().Equal(new object[] { dataSources[2] });
			}
		}

		[Test]
		[Description("Verifies that adding a group also creates and adds a corresponding settings object")]
		public void TestAddGroup1()
		{
			var settings = new DataSources();
			using (var dataSources = new Tailviewer.BusinessLogic.DataSources.DataSources(settings))
			{
				var group = dataSources.AddGroup();
				group.Should().NotBeNull();
				group.Settings.Should().NotBeNull();
				settings.Should().Equal(group.Settings);
			}
		}

		[Test]
		[Description("Verifies that a newly added group has a unique id")]
		public void TestAddGroup2()
		{
			var group = _dataSources.AddGroup();
			group.Id.Should().NotBe(Guid.Empty);
		}

		[Test]
		public void TestAdd()
		{
			var source = _dataSources.AddDataSource(@"E:\Code\test.log");
			source.Should().NotBeNull();
			source.FullFileName.Should().Be(@"E:\Code\test.log");
			source.FollowTail.Should().BeFalse();
			source.Id.Should().NotBe(Guid.Empty, "Because a newly added data source should have a unique id");

			_settings.Count.Should().Be(1);
			_settings[0].File.Should().Be(@"E:\Code\test.log");
		}

		[Test]
		public void TestRemove()
		{
			var source1 = _dataSources.AddDataSource(@"E:\Code\test1.log");
			var source2 = _dataSources.AddDataSource(@"E:\Code\test2.log");

			_dataSources.Remove(source1);
			_settings.Count.Should().Be(1);
			_settings[0].File.Should().Be(@"E:\Code\test2.log");

			_dataSources.Remove(source2);
			_settings.Should().BeEmpty();
		}
	}
}