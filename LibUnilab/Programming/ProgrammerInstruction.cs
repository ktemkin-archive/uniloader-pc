using System;

namespace Unilab.Programming
{
	/// <summary>
	/// 	Instructions for the Unified Lab Kit bootloader.
	/// </summary> 
	///
	public enum ProgrammerInstruction : ushort
	{
		#region Reset codes

		HardReset = 0xF000,
		SoftReset = 0xF001,

        #endregion

        FPGAConfigStart = 0xF022,
		FPGAConfigSend = 0xF023,
		FPGAConfigEnd = 0xF024
		

		
	}
}

