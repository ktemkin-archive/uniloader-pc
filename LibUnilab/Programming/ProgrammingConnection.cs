using System;

using LibUsbDotNet;
using LibUsbDotNet.LibUsb;
using LibUsbDotNet.Main;
using LibUsbDotNet.Info;
using MonoLibUsb;

//This might have to be removed for Mono compatibility
//(and each use below done in longform.)
using System.Management;
using Unilab.Debug;


namespace Unilab.Programming
{
    enum PageQualifier
    {
        First,
        Last,
        Normal
    }

    /// <summary>
    /// 	Lower-level API provider for Programmer connections.
    ///		(Note the lack of checksums or CRCs; these are handled by the USB stack.)
    /// </summary>
    internal class ProgrammingConnection
    {
        //TODO: These should be abstracted to a configuration XML.
        //(Afterwards they can be represented by a ReadOnly reference.)
        const int VID = 0x16D0;
        const int PID = 0x05A5;

        public static UsbDeviceFinder UnilabDevice = new UsbDeviceFinder(VID, PID);

        static ProgrammingConnection()
        {
            LibUsbDevice.ForceLibUsbWinBack = true;
        }

        public static Programmer.DeviceStatus deviceStatus()
        {

            //Use LibUSB to get a list of currently running devices on Mono, as it works
            //well on *nix platforms.
            if (OS.Platform.isMono())
            {

                //This method works on Linux's libusb, and appears to work on Windows,
                //but causes further libusb queries to fail.
                UsbRegistry reg = LibUsbDevice.AllDevices.Find(UnilabDevice);

                //If no device was found, indicate this.
                if (reg == null)
                    return Programmer.DeviceStatus.NotFound;

                //set the type of the unified lab kit from the revision code
                try
                {
                    Hardware.CurrentLabKit.setCurrentKit(reg.Rev);
                }
                catch (InvalidCastException)
                {
                    return Programmer.DeviceStatus.ErrorUnknown;
                }

                //This doesn't appear to be accurate. More research is required (?)
                if (reg.IsAlive)
                    return Programmer.DeviceStatus.InUse;
                else
                    return Programmer.DeviceStatus.Available;


            }
            else
            {
                //Otherwise, attempt to use the Windows Management framework to enumerate all USB devices.
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(@"Select * From Win32_PnPEntity where Description like ""%Human Interface Device%""");

                //for each human interface device
                foreach (ManagementBaseObject device in searcher.Get())
                {
                    //get the Device ID (which contains the VID, PID, and revision) and the device's status
                    string deviceID = device.GetPropertyValue("DeviceID") as string;
                    string status = device.GetPropertyValue("Status") as string;

                    //covert the device ID into a full USB PID/VID/Revision tuple
                    OS.Win32.USBDeviceID localID = new OS.Win32.USBDeviceID(deviceID);

                    //if we've found a device other than the unilab kit, continue
                    if (localID.VendorID != VID || localID.ProductID != PID)
                        continue;

                    //If the device is ready, return the status available.
                    if (status == "OK")
                    {
                        //set the type of the unified lab kit from the revision code
                        try
                        {
                            Hardware.CurrentLabKit.setCurrentKit(localID.Revision);
                        }
                        catch (InvalidCastException)
                        {
                            return Programmer.DeviceStatus.ErrorUnknown;
                        }

                        return Programmer.DeviceStatus.Available;
                    }
                    else
                    {
                        //FIXME
                        throw new NotImplementedException();
                    }
                }

                return Programmer.DeviceStatus.NotFound;

            }
        }


        /// <summary>
        /// 	Connection the Unified Lab Kit device.
        /// </summary>
        MonoUsbDeviceHandle device;
        MonoUsbSessionHandle session;

        short timeout;


        /// <summary>
        /// 	Creates a new ProgrammingConnection; if the Unified Lab Kit can't be found, 
        /// 	it throws a DeviceNotConnected excpetion.
        /// </summary>	
        /// <param name="Timeout">
        /// 	The amount of time to wait before the system decides the device isn't connected.
        /// </param>
        public ProgrammingConnection(short timeout)
        {
            int retries = 5;


            //store the timeout field from the constructor
            this.timeout = timeout;

            //create a new local USB session
            session = new MonoUsbSessionHandle();


            while (retries-- > 0)
            {
                try
                {
                    //find the device by its descriptor
                    device = MonoUsbApi.OpenDeviceWithVidPid(session, VID, PID);
                }
                catch (ObjectDisposedException ex) 
                {
                    if (retries == 0)
                    {
                        //DEBUG
                        DebugConsole.WriteLine("Windows could not create the USB connection... try again in a few seconds?");
                        throw ex;
                    }

                    //DEBUG
                    DebugConsole.WriteLine("Too soon! Retrying in 15s.");
                    System.Threading.Thread.Sleep(15000);

                    continue;
                }

                //break on success
                break;
            }

            if (device == null)
                throw new DeviceNotFoundException();

            //if the kernel has a driver operating on our interface, ask it to detach
            if (MonoUsbApi.KernelDriverActive(device, 0) >= 0)
                if (MonoUsbApi.DetachKernelDriver(device, 0) < 0)
                    throw new DeviceInUseException();

            //claim our interface for exclusive access
            MonoUsbApi.ClaimInterface(device, 0);
        }

        #region Public Methods


        public void Close()
        {
            //Release the interface and close the device.
            
            device.SetHandleAsInvalid();
            device.Close();
        }

        /// <summary>
        /// 	Writes a page to the AVR.
        /// </summary>
        /// <param name="address">
        /// 	The page address is the address of the page to be written; it must be aligned to the page size.
        /// </param>
        /// <param name="data">
        /// 	The 256 bytes (128 words) to write.
        /// </param>
        public void WriteAVRPage(int address, byte[] data)
        {
            byte[] packet = new byte[data.Length + 2];

            //convert the address to two bytes: the packet header
            packet[0] = (byte)(address & 0xFF);
            packet[1] = (byte)((address >> 8) & 0xFF);

            //copy the rest of the data in
            data.CopyTo(packet, 2);

            //and send the packet
            sendPacket(packet);
        }

        /// <summary>
        /// 	Sends a page of configuration data to the FPGA.
        /// </summary>
        /// <param name="page">
        /// 	The set of bytes to be sent to the FPGA.
        /// </param>
        /// <param name="qualifier">
        /// 	A qualifier determining if this is a special (first/last) pa
        /// </param>
        public void SendFPGAPage(byte[] page, PageQualifier qualifier)
        {
            byte[] packet;

            //prefix the data with the appropriate instruction code
            switch (qualifier)
            {
                case PageQualifier.First:
                    packet = prefixCommand(ProgrammerInstruction.FPGAConfigStart, page);
                    break;

                case PageQualifier.Last:
                    packet = prefixCommand(ProgrammerInstruction.FPGAConfigEnd, page);
                    break;

                default:
                    packet = prefixCommand(ProgrammerInstruction.FPGAConfigSend, page);
                    break;
            }

            //and send the packet to the device
            sendPacket(packet);
        }

        /// <summary>
        /// 	Writes a page to the MicroSD card.
        /// </summary>
        /// <param name="address">
        /// 	The address, on the SD card, to write to. Should be evenly divisible by 512.
        /// </param>
        /// <param name="data">
        /// 	The data to be written to the SD card. Should be <= 512 bytes.
        /// </param>
        public void WriteSDPage(uint address, byte[] data)
        {
            //This is a process that takes five commands:
            //First, each of four SD card buffers must be filled.
            //Then, a commit packet instructs the AVR to write the page to the SD card.
        }

        /// <summary>
        /// 	Resets the AVR device. In most cases, this will run the user program.
        /// </summary>
        public void ResetAVR(bool softReset)
        {
            byte[] packet;

            if (softReset)
                packet = prefixCommand(ProgrammerInstruction.SoftReset, null);
            else
                packet = prefixCommand(ProgrammerInstruction.HardReset, null);

            try
            {
                sendPacket(packet);
            }
            //CommunicationErrors are expected when we reset the device.
            catch (CommunicationErrorException) { }
        }

        /// <summary>
        /// 	Hard resets the AVR device.
        /// </summary>
        public void ResetAVR()
        {
            ResetAVR(false);
        }


        #endregion

        #region Private Methods

        /// <summary>
        /// 	Sends a packet to the boot-loader. Typically contains 2 bytes of control and 256 bytes of payload.
        /// </summary>
        /// <param name="data">
        /// A <see cref="System.Byte[]"/>
        /// </param>
        private void sendPacket(byte[] packet)
        {
            //These 'magic numbers' are from the Teensy project, and match code in the UniLoader.
            const byte REQ_TYPE = 0x21;
            const byte REQ = 9;
            const short TRANSFER_VALUE = 0x200;
            const short TRANSFER_INDEX = 0;

            //send the packet as a control transfer [TODO: throw on error]
            int retVal = MonoUsbApi.ControlTransfer(device, REQ_TYPE, REQ, TRANSFER_VALUE, TRANSFER_INDEX, packet, (short)packet.Length, timeout);


            if (retVal < 0)
                throw new CommunicationErrorException();
        }


        /// <summary>
        /// 	Prefix a given command to the packet in question.
        /// </summary>
        /// <param name="instr">
        /// 	The instruction to be prefixed.
        /// </param>
        /// <param name="data">
        /// 	The data to which the instruction will be prefixed, or null to create an empty packet with a 128-byte null payload.
        /// </param>
        /// <returns>
        /// 	The fully formed packet.
        /// </returns>
        private byte[] prefixCommand(ProgrammerInstruction instr, byte[] data)
        {
            //Get the ushort code for the instruction.
            ushort instruction = (ushort)instr;

            //Create a new USB-HID packet.
            byte[] packet = new byte[130]; //FIXME in special compressed 128-byte case

            //If we received a data packet, copy it in.
            if (data != null)
                data.CopyTo(packet, 2);

            //Copy in the instruction code
            packet[0] = (byte)(instruction & 0xFF);
            packet[1] = (byte)((instruction >> 8) & 0xFF);

            //return the completed packet
            return packet;
        }

        #endregion
    }
}

