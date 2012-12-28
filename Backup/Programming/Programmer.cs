using System;
using Unilab.Programming.FileFormats;

namespace Unilab.Programming
{
	public class Programmer
	{
		ProgrammingConnection device = new ProgrammingConnection(3000);
			
		/// <summary>
		/// 	Delegate used to indicate percent completion for long operations.
		/// </summary>
		public delegate void statusUpdate(float percentComplete);
		
		public Programmer ()
		{
			
		}
		
		
		public void EraseAVR()
		{
			//Clear each page in Flash to all zeroes.
			for(ushort addr = 0; addr < AVR.HW.FlashSize - AVR.HW.BootloadSize; addr += AVR.HW.PageSize)
				device.WriteAVRPage(addr, new byte[AVR.HW.PageSize]);
		}
		
		public void ProgramAVR(IntelHex program)
		{
			ProgramAVR(program, null); 	
		}
		
		
		public void ProgramAVR(IntelHex program, statusUpdate target)
		{
			//program each non-bootloader section of Flash sequentially
			for(ushort addr = 0; addr < AVR.HW.FlashSize - AVR.HW.BootloadSize; addr += AVR.HW.PageSize)
			{
				//Write the page to the AVR.
				device.WriteAVRPage(addr, program.GetPage(addr));
			
				//If a delegate has been provided to accept status updates, inform it of the update.
				if(target != null)
					target((float) addr / (float)(AVR.HW.FlashSize - AVR.HW.BootloadSize));
					       
			}
			
			//and then reset the microprocessor
			device.ResetAVR(true);
			
			//indicate completion
			if(target!=null)
				target(1);
		}
		
		public void Close()
		{
			device.Close();	
		}
		
		
		#region Protected Methods
		
			

		
		#endregion
		
	}
}

