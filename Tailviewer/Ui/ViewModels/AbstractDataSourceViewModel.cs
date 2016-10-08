﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Metrolib;
using Tailviewer.BusinessLogic.DataSources;
using Tailviewer.BusinessLogic.Filters;
using Tailviewer.BusinessLogic.LogFiles;
using Tailviewer.BusinessLogic.Searches;

namespace Tailviewer.Ui.ViewModels
{
	internal abstract class AbstractDataSourceViewModel
		: IDataSourceViewModel
	{
		private readonly IDataSource _dataSource;
		private readonly ICommand _removeCommand;
		private readonly ICommand _startSearchCommand;
		private readonly ICommand _stopSearchCommand;

		private int _debugCount;
		private int _errorCount;
		private int _fatalCount;
		private Size _fileSize;
		private int _infoCount;
		private bool _isGrouped;
		private bool _isVisible;
		private int _lastSeenLogLine;
		private TimeSpan _lastWrittenAge;
		private int _noTimestampCount;
		private int _otherCount;
		private IDataSourceViewModel _parent;
		private int _totalCount;
		private int _warningCount;
		private string _searchTerm;
		private int _currentMatchIndex;
		private List<LogMatch> _searchMatches;
		private int _searchMatchCount;

		protected AbstractDataSourceViewModel(IDataSource dataSource)
		{
			if (dataSource == null) throw new ArgumentNullException("dataSource");

			_dataSource = dataSource;

			_removeCommand = new DelegateCommand(OnRemoveDataSource);
			_startSearchCommand = new DelegateCommand(StartSearch);
			_stopSearchCommand = new DelegateCommand(StopSearch);

			Update();
		}

		public int NewLogLineCount
		{
			get
			{
				int diff = TotalCount - _lastSeenLogLine;
				return Math.Max(diff, 0);
			}
		}

		public Guid Id
		{
			get { return _dataSource.Id; }
		}

		public bool IsGrouped
		{
			get { return _isGrouped; }
			private set
			{
				if (value == _isGrouped)
					return;

				_isGrouped = value;
				EmitPropertyChanged();
			}
		}

		public bool IsVisible
		{
			get { return _isVisible; }
			set
			{
				if (value == _isVisible)
					return;

				_isVisible = value;
				if (value)
				{
					LastViewed = DateTime.Now;
				}
				EmitPropertyChanged();

				UpdateLastSeenLogLine();
			}
		}

		public abstract ICommand OpenInExplorerCommand { get; }

		public abstract string DisplayName { get; }

		public int TotalCount
		{
			get { return _totalCount; }
			protected set
			{
				if (value == _totalCount)
					return;

				_totalCount = value;
				EmitPropertyChanged();
			}
		}

		public int NoTimestampCount
		{
			get { return _noTimestampCount; }
			protected set
			{
				if (value == _noTimestampCount)
					return;

				_noTimestampCount = value;
				EmitPropertyChanged();
			}
		}

		public int OtherCount
		{
			get { return _otherCount; }
			protected set
			{
				if (value == _otherCount)
					return;

				_otherCount = value;
				EmitPropertyChanged();
			}
		}

		public int DebugCount
		{
			get { return _debugCount; }
			protected set
			{
				if (value == _debugCount)
					return;

				_debugCount = value;
				EmitPropertyChanged();
			}
		}

		public int InfoCount
		{
			get { return _infoCount; }
			protected set
			{
				if (value == _infoCount)
					return;

				_infoCount = value;
				EmitPropertyChanged();
			}
		}

		public int WarningCount
		{
			get { return _warningCount; }
			protected set
			{
				if (value == _warningCount)
					return;

				_warningCount = value;
				EmitPropertyChanged();
			}
		}

		public int ErrorCount
		{
			get { return _errorCount; }
			protected set
			{
				if (value == _errorCount)
					return;

				_errorCount = value;
				EmitPropertyChanged();
			}
		}

		public int FatalCount
		{
			get { return _fatalCount; }
			protected set
			{
				if (value == _fatalCount)
					return;

				_fatalCount = value;
				EmitPropertyChanged();
			}
		}

		public Size FileSize
		{
			get { return _fileSize; }
			protected set
			{
				if (value == _fileSize)
					return;

				_fileSize = value;
				EmitPropertyChanged();
			}
		}

		public LogLineIndex VisibleLogLine
		{
			get { return _dataSource.VisibleLogLine; }
			set { _dataSource.VisibleLogLine = value; }
		}

		public double HorizontalOffset
		{
			get { return _dataSource.HorizontalOffset; }
			set { _dataSource.HorizontalOffset = value; }
		}

		public HashSet<LogLineIndex> SelectedLogLines
		{
			get { return _dataSource.SelectedLogLines; }
			set { _dataSource.SelectedLogLines = value; }
		}

		public TimeSpan LastWrittenAge
		{
			get { return _lastWrittenAge; }
			private set
			{
				if (value == _lastWrittenAge)
					return;

				_lastWrittenAge = value;
				EmitPropertyChanged();
			}
		}

		public ICommand RemoveCommand
		{
			get { return _removeCommand; }
		}

		public bool FollowTail
		{
			get { return _dataSource.FollowTail; }
			set
			{
				if (value == FollowTail)
					return;

				_dataSource.FollowTail = value;
				EmitPropertyChanged();
			}
		}

		public bool ShowLineNumbers
		{
			get { return _dataSource.ShowLineNumbers; }
			set
			{
				if (value == ShowLineNumbers)
					return;

				_dataSource.ShowLineNumbers = value;
				EmitPropertyChanged();
			}
		}

		public bool ColorByLevel
		{
			get { return _dataSource.ColorByLevel; }
			set
			{
				if (value == ColorByLevel)
					return;

				_dataSource.ColorByLevel = value;
				EmitPropertyChanged();
			}
		}

		#region Searches

		public string SearchTerm
		{
			get { return _searchTerm; }
			set
			{
				if (value == _searchTerm)
					return;

				_searchTerm = value;
				EmitPropertyChanged();
			}
		}

		public ICommand StartSearchCommand { get { return _startSearchCommand; } }
		public ICommand StopSearchCommand { get { return _stopSearchCommand; } }

		public IEnumerable<LogMatch> SearchMatches
		{
			get { return _searchMatches; }
			private set
			{
				if (value == _searchMatches)
					return;

				_searchMatches = new List<LogMatch>(value);
				EmitPropertyChanged();
			}
		}

		public int SearchMatchCount
		{
			get { return _searchMatchCount; }
			private set
			{
				if (value == _searchMatchCount)
					return;

				_searchMatchCount = value;
				EmitPropertyChanged();
			}
		}

		public int CurrentMatchIndex
		{
			get { return _currentMatchIndex; }
			set
			{
				if (value == _currentMatchIndex)
					return;

				_currentMatchIndex = value;
				EmitPropertyChanged();
			}
		}

		private void StartSearch()
		{
			if (!string.IsNullOrEmpty(_searchTerm))
			{
				_dataSource.SearchTerm = _searchTerm;
				CurrentMatchIndex = -1;
			}
		}

		private void StopSearch()
		{
			SearchMatchCount = 0;
			CurrentMatchIndex = -1;
			_dataSource.SearchTerm = null;
			_searchMatches.Clear();
		}

		#endregion

		public DateTime LastViewed
		{
			get { return _dataSource.LastViewed; }
			set
			{
				if (value == LastViewed)
					return;

				_dataSource.LastViewed = value;
				EmitPropertyChanged();
			}
		}

		public IDataSource DataSource
		{
			get { return _dataSource; }
		}

		public LevelFlags LevelsFilter
		{
			get { return _dataSource.LevelFilter; }
			set
			{
				if (value == LevelsFilter)
					return;

				_dataSource.LevelFilter = value;
				EmitPropertyChanged();
			}
		}

		public IDataSourceViewModel Parent
		{
			get { return _parent; }
			set
			{
				if (value == null)
				{
					_dataSource.Settings.ParentId = Guid.Empty;
				}
				else
				{
					_dataSource.Settings.ParentId = value.DataSource.Id;
				}
				_parent = value;
				IsGrouped = value != null;
			}
		}

		public IEnumerable<ILogEntryFilter> QuickFilterChain
		{
			get { return _dataSource.QuickFilterChain; }
			set { _dataSource.QuickFilterChain = value; }
		}

		public event PropertyChangedEventHandler PropertyChanged;
		public event Action<IDataSourceViewModel> Remove;

		public virtual void Update()
		{
			int newBefore = NewLogLineCount;

			OtherCount = _dataSource.NoLevelCount;
			DebugCount = _dataSource.DebugCount;
			InfoCount = _dataSource.InfoCount;
			WarningCount = _dataSource.WarningCount;
			ErrorCount = _dataSource.ErrorCount;
			FatalCount = _dataSource.FatalCount;
			TotalCount = _dataSource.TotalCount;
			FileSize = _dataSource.FileSize;
			NoTimestampCount = _dataSource.NoTimestampCount;
			LastWrittenAge = DateTime.Now - _dataSource.LastModified;

			if (NewLogLineCount != newBefore)
			{
				if (_dataSource.LastModified >= LastViewed && !IsVisible)
				{
					EmitPropertyChanged("NewLogLineCount");
				}
				else
				{
					_lastSeenLogLine = TotalCount;
				}
			}
		}

		private void UpdateLastSeenLogLine()
		{
			int before = NewLogLineCount;
			if (IsVisible)
			{
				_lastSeenLogLine = TotalCount;
				if (before != NewLogLineCount)
					EmitPropertyChanged("NewLogLineCount");
			}
		}

		private void OnRemoveDataSource()
		{
			Action<IDataSourceViewModel> fn = Remove;
			if (fn != null)
				fn(this);
		}

		protected void EmitPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}