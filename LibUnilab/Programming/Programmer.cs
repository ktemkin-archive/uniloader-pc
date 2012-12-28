using System;
using Unilab.Programming.FileFormats;
using Unilab.Hardware;


namespace Unilab.Programming
{
	public class Programmer
	{
		ProgrammingConnection device = new ProgrammingConnection(1000);
			
		/// <summary>
		/// 	Delegate used to indicate percent completion for long operations.
		/// </summary>
		public delegate void statusUpdate(float percentComplete);
		

        #region Static Methods

        public enum DeviceStatus
        {
            Available,
            InUse,
            NotFound,
            ErrorUnknown
        }

        public static DeviceStatus deviceStatus()
        {
            return ProgrammingConnection.deviceStatus();
        }


        #endregion


        public void EraseAVR()
		{
			//Clear each page in Flash to all zeroes.
            for (ushort addr = 0; addr < CurrentLabKit.WriteableFlash; addr += CurrentLabKit.PageSize)
                device.WriteAVRPage(addr, new byte[CurrentLabKit.PageSize]);
		}
		
		public void ProgramAVR(IntelHex program)
		{
			ProgramAVR(program, null); 	
		}
		
		
		public void ProgramAVR(IntelHex program, statusUpdate target)
		{
			//program each non-bootloader section of Flash sequentially
            for (int i = 0; i < CurrentLabKit.WriteableFlash / CurrentLabKit.PageSize; ++i)
			{
				//Write the page to the AVR.
				device.WriteAVRPage(i*128, program.GetPage(i));
			
				//If a delegate has been provided to accept status updates, inform it of the update.
				if(target != null)
                    target((i * 128.0f) / (float)(CurrentLabKit.WriteableFlash));
			}
			
			//and then reset the microprocessor
			device.ResetAVR(true);
			
			//indicate completion
			if(target!=null)
				target(1);
		}
		
		public void ConfigureFPGA(XilinxBitstream bitstream)
		{
			ConfigureFPGA(bitstream, null);
		}
		
		public void ConfigureFPGA(XilinxBitstream bitstream, statusUpdate target)
		{			
			for(int i=0; i < bitstream.NumPages; ++i)
			{
				PageQualifier type;
					
				//get the current page to be configured
				byte[] page = bitstream.getPage(i);	
				
				//determine any special characteristics of this page
				if(i == 0)
				{
					//First page starts configuration.
					type = PageQualifier.First;
				}
				else if(i == bitstream.NumPages - 1 )
				{
					//Last page finalizes configuration- and has a slightly different structure.
					type = PageQualifier.Last;
					
					//As the last page isn't neccesarily full, it is prefixed with a byte
					//specifying its length.
					
					//create a page buffer to accomodate the extra byte
					byte[] newPage = new byte[page.Length + 1];
					
					//copy the old data
					page.CopyTo(newPage, 1);
					
					//and prefix the page-data appropriately
					newPage[0] = (byte)page.Length;
					
					//replace the pagebuffer with the new, updated pagebuffer
					page = newPage;
				}
				else
				{
					type = PageQualifier.Normal;
				}

                //If a delegate has been provided to accept status updates, inform it of the update.
                if (target != null)
                    target((float)i / (float)(bitstream.NumPages - 1));
                
                //send the configuration data to the FPGA
                device.SendFPGAPage(page, type);

			}
		}
		
		public void Close()
		{
			device.Close();	
		}
		
		
		#region Protected Methods
		
			

		
		#endregion
		
	}
}

