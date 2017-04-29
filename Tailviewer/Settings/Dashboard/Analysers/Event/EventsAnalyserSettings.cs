﻿using System.Collections.Generic;
using System.Xml;
using Metrolib;
using Tailviewer.BusinessLogic.Dashboard.Analysers;
using Tailviewer.BusinessLogic.Dashboard.Analysers.Event;

namespace Tailviewer.Settings.Dashboard.Analysers.Event
{
	/// <summary>
	/// Contains all settings for a <see cref="EventsLogAnalyser"/>.
	/// Consists of mainly a list of <see cref="EventSettings"/>.
	/// </summary>
	public sealed class EventsAnalyserSettings
		: AnalyserSettings
	{
		public readonly List<EventSettings> Events;
		public int MaxEvents;

		public EventsAnalyserSettings()
		{
			Events = new List<EventSettings>();
		}

		protected override void RestoreInternal(XmlReader reader)
		{
			for (int i = 0; i < reader.AttributeCount; ++i)
			{
				reader.MoveToAttribute(i);
				switch (reader.Name)
				{
					case "maxevents":
						MaxEvents = reader.ReadContentAsInt();
						break;
				}
			}

			reader.MoveToElement();
			XmlReader subtree = reader.ReadSubtree();

			while (subtree.Read())
			{
				switch (subtree.Name)
				{
					case "event":
						var @event = new EventSettings();
						@event.Restore(reader);
						Events.Add(@event);
						break;
				}
			}
		}

		protected override void SaveInternal(XmlWriter writer)
		{
			writer.WriteAttributeInt("maxevents", MaxEvents);

			foreach (var @event in Events)
			{
				writer.WriteStartElement("event");
				@event.Save(writer);
				writer.WriteEndElement();
			}
		}
	}
}