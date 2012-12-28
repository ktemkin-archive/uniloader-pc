using System;
using System.IO;
using System.Collections.Generic;


namespace Unilab.Programming.FileFormats
{
	
	
	/// <summary>
	///		
	/// </summary>
	public class XilinxBitstream
	{
	
		#region Constants
		
		/// <summary>
		/// 	Constant header which denotes a valid Xilinx byte-file.
		/// </summary>
		static readonly byte[] bitHeader = new byte[] {0, 9, 15, 240, 15, 240, 15, 240, 15, 240, 0, 0, 1};
		static readonly byte[] syncHeader = new byte[] { 0xff, 0xff, 0xff, 0xff, 0xaa, 0x99, 0x55, 0x66 };
		
		//Page size.
		const int PAGE_SIZE = 128;
		
		#endregion
	
		
		#region Private Fields
		/// <summary>
		/// 	Stores the data content of the raw bitstream.
		/// </summary>
		private byte[] data;
		
        /*
		private string name;
		private string userID;
         */
		
		
		#endregion
		
		#region Properties
		
		public byte this [int index]
		{
			get
			{
				return data[index];
			}
		}
		
		public int Length
		{
			get
			{
				return data.Length;	
			}
		}
		
		public int NumPages
		{
			get
			{
				return (int)Math.Ceiling((float)data.Length / (float)PAGE_SIZE);
			}
		}
		
		public int LastPageLength
		{
			get
			{
				return data.Length % PAGE_SIZE;	
			}
		}
		
		#endregion
		
		#region Constructors
		
		public XilinxBitstream (byte[] data)
		{
			//copy in the bitstream data
			this.data = data;
		}
		
		public XilinxBitstream(byte[] data, string name, string userID)
		{
			//TODO
			this.data = data;	
		}
		
		#endregion
		
		
		#region Public Methods
		
		public byte[] getPage(int number)
		{
			byte[] pageBuffer;			
			
			//don't allow reads past the last page
			if(number > NumPages)
				throw new ArgumentException("Can't read past the last page!");
		
			//FIXME FIXME FIXME
			//handle last page glitch
			//i.e. failure iff:
			//	-the user has enabled compressed bitfiles AND
			//	-the result of the compression happens to satisfy the condition (FILE_SIZE % 128 == 0)
			
			//fix by sending one last packet of 129 bytes (if possible)
			//or by building a special case instruction
			
			//if we're on the last page, the legnth will be equal to the last page's length
			if(number == NumPages - 1)
				pageBuffer = new byte[LastPageLength];
			//otherwise, it will equal the page size
			else
				pageBuffer = new byte[PAGE_SIZE];
			
			//fill the target page with bitstream data
			
			//In this case, the bytes are reversed for compatibility with JTAG
			//programming; this corrects the bit order for LSB-first shifting.
			for(int i = 0; i < pageBuffer.Length; ++i)
				pageBuffer[i] = reverseByte(data[(number * PAGE_SIZE) + i]);
			
			//return the page
			return pageBuffer;	
		}
		
		#endregion
		
		#region Static Methods
		
		/// <summary>
		/// 	Extracts Xilinx bitstream data from a *.bit or *.bin file.
		/// </summary>
		/// <param name="file">
		/// 	The file to parse, as a stream.
		/// </param>
		/// <returns>
		/// 	A XilinxBistream data object.
		/// </returns>
		public static XilinxBitstream FromFile(FileStream file)
		{			
			//read the bitfile's header (first 13 bytes)
			byte[] header = new byte[13];
			file.Read(header, 0, header.Length);
			
			//if the file's header doesn't match the standard Xilinx header
			if(!header.Equals(bitHeader))
			{
				
				bool match = true;
				
				//check to see if matches the generic sync packet
				for(int i = 0; i < syncHeader.Length; ++i)
				{
					if(header[i] != syncHeader[i])
					{
						match = false;
						break;
					}
				}
				
				//if it does, parse the file as a *.bin file
				if(match)
				{
					//'rewind' the stream
					file.Seek(0, SeekOrigin.Begin);
					
					//and parse the file as a bin file
					return new XilinxBitstream(fromBinFile(file));
				}
				else
				{
					//FIXME
					throw new Exception("Not a valid Xilnx bitstream!");
				}
				
			}
			
			
			//TODO
			throw new NotImplementedException();
		}
		
		/// <summary>
		/// 	Extracts Xilinx bitstream data from a *.bin file.
		/// </summary>
		/// <param name="file">
		/// 	The file to parse, as a stream.
		/// </param>
		/// <returns>
		/// 	The binary data.
		/// </returns
		public static byte[] fromBinFile(FileStream file)
		{
			//Create a data buffer of average size.
			//(~73KiB, the size of an uncompressed, 100K gate bit file)
			//
			//This should probably be changed to fit a 250K gate bitfile
			//when the UniLab V1 is produced.
			List<byte> data = new List<byte>(72668);
			
			//byte buffer
			int buffer;
					
			//read the file in its entirety
			while((buffer = file.ReadByte()) != -1)
				data.Add((byte)buffer);
			
			//return a byte array of data
			return data.ToArray();
		}
		
		/// <summary>
		/// 	Reverse the bits in a byte
		/// </summary>
		static byte reverseByte(byte b) 
		{ 
			//simple mask trick to reverse a byte
			//(made uglier by C#)
			int rev = (b >> 4) | ((b & 0xf) << 4); 
		
			rev = ((rev & 0xcc) >> 2) | ((rev & 0x33) << 2);
			rev = ((rev & 0xaa) >> 1) | ((rev & 0x55) << 1); 
		
			return (byte)rev; 
		}
			
		#endregion
	}
}

