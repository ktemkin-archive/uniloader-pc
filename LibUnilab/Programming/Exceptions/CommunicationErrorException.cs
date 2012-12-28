using System;
namespace Unilab
{
	public class CommunicationErrorException : Exception
	{
		public CommunicationErrorException ()
			: base("A communications error occurred!")
		{}
	}
}

