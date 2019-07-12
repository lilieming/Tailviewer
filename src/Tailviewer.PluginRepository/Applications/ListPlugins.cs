﻿using System;
using System.IO;

namespace Tailviewer.PluginRepository.Applications
{
	public sealed class ListPlugins
		: IApplication<ListPluginsOptions>
	{
		public ExitCode Run(IFilesystem filesystem, IInternalPluginRepository repository, ListPluginsOptions options)
		{
			foreach (var plugin in repository.FindAllPlugins())
			{
				Console.WriteLine("\t{0}", plugin);
			}

			return ExitCode.Success;
		}
	}
}