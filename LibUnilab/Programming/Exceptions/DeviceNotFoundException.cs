using System;

namespace Unilab.Programming
{
	public class DeviceNotFoundException : Exception
	{
		public DeviceNotFoundException ()
			: base("Could not find the Unified Lab Kit!")
		{}

        public DeviceNotFoundException(string message)
            : base(message)
        {
        }
	}
}

