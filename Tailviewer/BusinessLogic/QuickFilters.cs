﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Tailviewer.BusinessLogic
{
	internal sealed class QuickFilters
	{
		private readonly object _syncRoot;
		private readonly List<QuickFilter> _quickFilters;
		private readonly Settings.QuickFilters _settings;

		public QuickFilters(Settings.QuickFilters settings)
		{
			if (settings == null) throw new ArgumentNullException("settings");

			_syncRoot = new object();
			_settings = settings;
			_quickFilters = new List<QuickFilter>();
			foreach (var setting in settings)
			{
				_quickFilters.Add(new QuickFilter(setting));
			}
		}

		public IEnumerable<QuickFilter> Filters
		{
			get
			{
				lock (_syncRoot)
				{
					return _quickFilters.ToList();
				}
			}
		}

		/// <summary>
		/// Adds a new quickfilter.
		/// </summary>
		/// <returns></returns>
		public QuickFilter Add()
		{
			lock (_syncRoot)
			{
				var settings = new Settings.QuickFilter();
				var filter = new QuickFilter(settings);
				_quickFilters.Add(filter);
				_settings.Add(settings);
				return filter;
			}
		}

		public void Remove(Guid id)
		{
			lock (_syncRoot)
			{
				_quickFilters.RemoveAll(x => x.Id == id);
				int idx = _settings.FindIndex(x => Equals(x.Id, id));
				if (idx != -1)
				{
					_settings.RemoveAt(idx);
				}
			}
		}
	}
}