using System;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using Unilab.Hardware;

namespace Unilab.Programming.FileFormats
{
	/// <summary>
	/// 	Handles the interpretation of a Intel Hex file, an ASCII
	/// 	description of memory contents.
	/// </summary>
	public class IntelHex
	{
		Dictionary<Int32, byte> progmem;
	
		#region Properties
		
		public byte this [int index]
		{
			get
			{
				//Sparse array via a dictionary: 
				//	
				//If the Program Memory sparse array contains the key,
				//return it; otherwise, return 0.
				if(progmem.ContainsKey(index))
					return progmem[index];	
				else
					return 0;
			}
		}
		
		public int MemoryLength
		{
			get
			{
				int max = 0;
				
				//Find the greatest valued key.
				foreach(int key in progmem.Keys)
					max = Math.Max(max, key);
				
				return max;
			}
		}
		
		public int MemoryStart
		{
			get
			{
				int min = 0;
				foreach(int key in progmem.Keys)
					min = Math.Min(min, key);
				
				return min;
			}
		}
		
		#endregion
		
		/// <summary>
		/// 	EOF line for Intel Hex files.
		/// </summary>
		private const string EOF = ":00000001FF";
		
		private Int32 segmentBase;
		
		public IntelHex()
		{
			//Start off with a segment base of 0x0000.
			segmentBase = 0x0000;
			
			//Initialize the sparse array.
			progmem = new Dictionary<int, byte>();
		}
	
		
		#region Public Methods
		
		/// <summary>
		/// 	Parses a Intel Hexfile line given context (knowledge of past entries) and adds it to the given data list.
		/// </summary>
		/// <param name="line">
		/// 	The line to process.
		/// </param>
		public void AddLine(string line)
		{
			//Keep track of the current sum and checksum for validation purposes.
			byte sum = 00;
			byte checksum = 00;
			
			//A valid Data Entry must be at least 12 characters long, and start with ":"
			if( line[0] != ':' && line.Length <= 12)
				throw new ArgumentException();
			
			//Get the length of the data line
			byte length = Convert.ToByte(line.Substring(1, 2), 16);
			
			//Read the address from the line
			Int32 address = Convert.ToInt32(line.Substring(3, 4), 16);
			
			//Read the record type
			byte type = Convert.ToByte(line.Substring(7, 2), 16);
			
			//Add the parsed items to the checksum.
			sum += (byte)(address + type + length);
			
			//Handle records according to their type declaration byte.
			switch(type)
			{
				case 0:
					//Data Record
				
					//for each hex pair in question, attempt to parse the byte
					for(int i = 0; i < length; i++)
					{
						progmem.Add(address + segmentBase + i, Convert.ToByte(line.Substring(9 + i*2, 2), 16));
				
						//and add it to the checksum
						sum += progmem[address + segmentBase + i];
					}
					
					//Read the checksum from the file
					checksum = Convert.ToByte(line.Substring(9 + length*2, 2), 16);
				
					//and compare it to the calculated sum
					//if(checksum != 0x100 - sum)
						//throw new Exception("Checksum fail! " + Convert.ToString(checksum, 16) + " != " + Convert.ToString(0x100 - sum, 16));
				
					return;
				
				case 1:
				
					//Read the checksum from the file
					checksum = Convert.ToByte(line.Substring(9, 2), 16);
				
					//and compare it to the calculated sum
					//if(checksum != 0x100 - sum)
					//	throw new Exception("Checksum fail! " + Convert.ToString(checksum, 16) + " != " + Convert.ToString(0x100 - sum, 16));
				   
				
					//EOF Record
					return;
				
				
				case 2:
					//Extended Segment Address Record
				
					//set the segment base accordingly
					segmentBase = Convert.ToInt32(line.Substring(9, 4) + "0", 16);
				   
				   	//Read the checksum from the file
					checksum = Convert.ToByte(line.Substring(13, 2), 16);
				
					//and compare it to the calculated sum
					//if(checksum != 0x100 - sum)
					//	throw new Exception("Checksum fail! " + Convert.ToString(checksum, 16) + " != " + Convert.ToString(0x100 - sum, 16));
				   
					return;
						
				default:
					//Handle unimplemented types.
					throw new NotImplementedException("Record type not implemented: " + type.ToString());
		
			}
		}

		
		/// <summary>
		/// 	Creates an Intel Hex record from a readable stream.
		/// </summary>
		/// <param name="file">
		/// 	An input stream (usually a file).
		/// </param>
		/// <returns>
		/// 	An Intel Hex record.
		/// </returns>
		public static IntelHex FromStream(StreamReader file)
		{
			IntelHex data = new IntelHex();
			
			//For each line in the File/Stream
			while(!file.EndOfStream)
			{
				//read the line
				String line = file.ReadLine();
				
				//If we've reached an EOF record, break.
				if(line.StartsWith(EOF))
					break;
				
				//otherwise, parse the line and continue
				data.AddLine(line);
			}
			
			//return the IntelHex file created
			return data;
		}
		
		/// <summary>
		/// 	Returns the contents of a given page.
		/// </summary>
		/// <param name="pageNum">
		/// 	
		/// </param>
		public byte[] GetPage(int pageNum, int pageSize)
		{
			//Create a new array to store the page size.
			byte[] data = new byte[pageSize];
			
			//And copy in each value from the internal sparse array.
			for(int i = 0; i < pageSize; ++i)
				data[i] = this[i + pageNum * pageSize];
			
			//Return the data.
			return data;
		}
		
		/// <summary>
		/// 	Returns the contents of a given page, using the default AVR hardware parameters.
		/// </summary>
		/// <param name="pageNum">
		/// 	The address, which must be page-aligned (evenly divisible by 128).
		/// </param>
		public byte[] GetPage(int pageNum)
		{
			return GetPage(pageNum, CurrentLabKit.PageSize);	
		}
		
		/// <summary>
		/// 	Compares one IntelHex record to another to determine equivalence.
		/// </summary>
		/// <param name="a">
		/// 	The IntelHex record to compare with this one.
		/// </param>
		/// <returns>
		/// 	True iff the two records are identical.
		/// </returns>
		public bool Equals(IntelHex a)
		{
			//This item cannot equal null.
			if(a==null)
				return false;
			
			//To compare to IntelHex records, compare their memory maps.
			byte[] memoryA = a.GetPage(0, a.MemoryLength);
			byte[] memoryB = this.GetPage(0, this.MemoryLength);
			
			//Array with nonequal length cannot be equivalent
			if(memoryA.Length != memoryB.Length)
				return false;
			
			//Search for any inequalities in the two arrays.
			for(int i=0; i < memoryA.Length; ++i)
				if(memoryA[i] != memoryB[i])
					return false;
			
			//Otherwise, they must be equal.
			return true;
		}
		
		#endregion	
	}
}

