using System;
namespace Unilab
{
	/// <summary>
	/// 	Thrown if an invalid payload is queued for sending. 
	/// 	(This is usually because you provided a longer payload than would fit in a packet.)
	/// </summary>
	public class InvalidPacketException : Exception
	{
		public InvalidPacketException (String message)
		{
			
		}
		
		public InvalidPacketException()
		{	
		}
	}
}

