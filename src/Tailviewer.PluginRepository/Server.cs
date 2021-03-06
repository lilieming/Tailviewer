﻿using System;
using System.Net;
using SharpRemote;
using Tailviewer.Archiver.Repository;

namespace Tailviewer.PluginRepository
{
	public sealed class Server
		: IDisposable
	{
		private readonly SocketServer _socket;
		private readonly IPluginRepository _repository;

		public Server(IPEndPoint endPoint, IPluginRepository repository)
		{
			_repository = repository;

			_socket = new SocketServer($"{Constants.ApplicationTitle} Socket");
			_socket.RegisterSubject(Archiver.Repository.Constants.PluginRepositoryV1Id, _repository);
			_socket.Bind(endPoint);
		}

		#region IDisposable

		public void Dispose()
		{
			_socket?.Dispose();
		}

		#endregion
	}
}