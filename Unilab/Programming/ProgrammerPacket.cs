using System;


namespace Unilab.Programming
{
	/// <summary>
	/// The data structure which represents a Programmer packet. 
	/// 
	/// Very much subject to change.
	/// </summary>
	public class ProgrammerPacket
	{
		
		
		
		/// <summary>
		/// 	The one-byte data which represents a programming instruction.
		/// </summary>
		public readonly ProgrammerInstruction Instruction; 
		
		/// <summary>
		/// 	A set of eight (8) bytes representing the address argument for a given instruction.
		/// 	If an address is not required, this may be used as additional control.
		/// </summary>
		public readonly byte[] address;
		
		/// <summary>
		/// 	A set of three (3) bytes which provide an argument to the programmer.
		/// </summary>
		public readonly byte[] control;
		
		/// <summary>
		/// 48 bytes of payload data.
		/// </summary>
		public readonly byte[] payload;
		
		public ProgrammerPacket(ProgrammerInstruction instr, byte[] control, byte[] address, byte[] payload)
		{
			if(control.Length != 3)
				throw new InvalidPacketException();
				
		}
	}
}

