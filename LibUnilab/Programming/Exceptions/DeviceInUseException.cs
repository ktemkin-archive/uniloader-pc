using System;
namespace Unilab
{
	public class DeviceInUseException : Exception
	{
		public DeviceInUseException () 
			: base("The Unified Lab Kit is in use by another program.")
		{}
	}
}

