using System;

using LibUsbDotNet;
using LibUsbDotNet.LibUsb;
using LibUsbDotNet.Main;
using LibUsbDotNet.Info;
using LibUsbDotNet.WinUsb;
using MonoLibUsb;
using Unilab.Win32;

namespace Unilab.Programming
{
    /// <summary>
    /// 	Lower-level API provider for Programmer connections.
    ///		(Note the lack of checksums or CRCs; these are handled by the USB stack.)
    /// </summary>
    internal class Win32ProgrammingConnection : ProgrammingConnection
    {
        UsbDevice device;


        /// <summary>
        /// 	Creates a new ProgrammingConnection; if the Unified Lab Kit can't be found, 
        /// 	it throws a DeviceNotConnected excpetion.
        /// </summary>	
        /// <param name="Timeout">
        /// 	The amount of time to wait before the system decides the device isn't connected.
        /// </param>
        public Win32ProgrammingConnection()
        {
            //attempt to open the USB device
            device = LibUsbDevice.OpenUsbDevice(new UsbDeviceFinder(VID, PID));





            
        }

        #region Public Methods


        public override void Close()
        {
            //Release the interface and close the device.
            device.Close();
        }



        #endregion

        #region Protected Methods

        /// <summary>
        /// 	Sends a packet to the boot-loader. Typically contains 2 bytes of control and 256 bytes of payload.
        /// </summary>
        /// <param name="data">
        /// A <see cref="System.Byte[]"/>
        /// </param>
        internal override void sendPacket(byte[] packet)
        {
            //These 'magic numbers' are from the Teensy project, and match code in the UniLoader.
            const byte REQ_TYPE = 0x21;
            const byte REQ = 9;
            const short TRANSFER_VALUE = 0x200;
            const short TRANSFER_INDEX = 0;

            //send the packet as a control transfer [TODO: throw on error]
            //int retVal = MonoUsbApi.ControlTransfer(device, REQ_TYPE, REQ, TRANSFER_VALUE, TRANSFER_INDEX, packet, (short)packet.Length, timeout);
            
            
            //UsbSetupPacket setupPacket = new UsbSetupPacket(REQ_TYPE, REQ, TRANSFER_VALUE, TRANSFER_INDEX, (short)packet.Length);
            
            
            
            //device.

            //if (retVal < 0)
              //  throw new CommunicationErrorException();
        }

        #endregion
    }
}

