using System;
using System.Text;
using System.IO;
using Gtk;

using Unilab;
using Unilab.AVR;
using Unilab.Programming;
using Unilab.Programming.FileFormats;

using ProgrammerGUI.Utilities;

public partial class MainWindow : Gtk.Window
{
	IntelHex hexRecord;
	
	public MainWindow () : base(Gtk.WindowType.Toplevel)
	{
		Build ();
		
		//set up the hex finder
		FileFilter filter = new FileFilter();
		filter.Name = "Intel Hex File (*.hex)";
		filter.AddPattern("*.hex");
		fileHex.AddFilter(filter);
		fileHex.FileSet += HandleFileHexFileSet;
		
		
		//refresh button
		btnHexRefresh.Clicked += HandleFileHexFileSet;
		
		//program button
		btnHexProgram.Clicked += HandleBtnHexProgramClicked;
		
	}

	#region GUI Event Handlers
	
	/// <summary>
	/// 	Handles the user's request to load a Hex file.
	/// </summary>
	void HandleFileHexFileSet (object sender, EventArgs e)
	{
		if(!File.Exists(fileHex.Filename))
			return;
		
		//Open a stream connected to the input hex file.
		FileStream hexFile = new FileStream(fileHex.Filename, FileMode.Open);
		
		//And wrap it with a StreamReader
		StreamReader hexStream = new StreamReader(hexFile);
		
		//Parse the file stream to yield an intel hex file.
		IntelHex newRecord = IntelHex.FromStream(hexStream);
		
		
		//Close the stream/file.
		hexStream.Close();
		hexFile.Close();
		
		//If the new record matches the old, show a warning.
		if(newRecord.Equals(hexRecord))
			lblOldHexWarning.Show();
		else
			lblOldHexWarning.Hide();
		
		//and accept the new record
		hexRecord = newRecord;
		
		//Derive statistics from the hex record.
		deriveStatistics();
		
		//enable the relevant buttons
		btnHexProgram.Sensitive = true;
		btnHexRefresh.Sensitive = true;
		
		
	}
	
	void HandleBtnHexProgramClicked (object sender, EventArgs e)
	{
		btnHexProgram.Sensitive = false;
		
		
		try
		{
			//TODO: abstract to background connectivity
			Programmer device = new Programmer();	
			
			//Program the Hex file to the device.
			device.ProgramAVR(hexRecord, statusUpdate);
			
			device.Close();
		}
		catch(DeviceNotFoundException)
		{
			//TODO: do something?	
		}
		
		btnHexProgram.Sensitive = true;
	}
	
	#endregion
	
	#region API Event Handlers
	
	void statusUpdate(float status)
	{
		//Update the progress bar
		progressBar.Fraction = status;	
		
		if(status==1)
			progressBar.Text = "Done!";
		else
			progressBar.Text = " ";
		
		GUI.processQueuedEvents();
	}
	
	
	#endregion
	
	/// <summary>
	/// 	Derives statistics about the current hex file.
	/// </summary>
	void deriveStatistics()
	{
		//Create a new string buffer from which to build statistics
		StringBuilder statistics = new StringBuilder();
		
		//Required program memory
		statistics.AppendLine("<b>Required Program Memory:</b> " + hexRecord.MemoryLength + " <i>bytes</i>");
		
		//Available program memory
		statistics.AppendLine("<b>Available Program Memory:</b> " + (HW.FlashSize - HW.BootloadSize).ToString() + " <i>bytes</i>");
		
		//display the statistics
		lblHexStatistics.Text = statistics.ToString();
		lblHexStatistics.UseMarkup = true;
		lblHexStatistics.Show();
	}
	
	
	


	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}
}

