using System;

namespace ProgrammerGUI.Utilities
{
	public static class GUI
	{
		/// <summary>
		/// 	Process any GUI events waiting on the context queue.
		/// </summary>
		public static void processQueuedEvents()
		{
			//process all queued events
			while(GLib.MainContext.Iteration());
		}
		
		/// <summary>
		/// 	Process up to a given amount of events waiting on the context queue.
		/// </summary>
		/// <param name="max">
		/// 	The maximum amount of events to process.
		/// </param>
		public static void processQueuedEvents(uint max)
		{
			uint i = 0;
			
			//process queued events up the maximum amount specified
			while(GLib.MainContext.Iteration() && i <= max)
				++i;
		}
	}
}

