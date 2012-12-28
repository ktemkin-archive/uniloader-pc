using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unilab.Programming
{
        /// <summary>
        /// 	Lower-level API provider for Programmer connections.
        ///		(Note the lack of checksums or CRCs; these are handled by the USB stack.)
        /// </summary>
        abstract class ProgrammingConnection
        {
            //TODO: These should be abstracted to a configuration XML.
            //(Afterwards they can be represented by a ReadOnly reference.)
            protected const int VID = 0x16C0;
            protected const int PID = 0x0478;

            protected short timeout = 3000;

            #region Static Functions

            /// <summary>
            /// Decisive 'constructor'; creates a ProgrammingConnection based on the supporting framework.
            /// Should work on Win32/Mono.
            /// </summary>
            /// <param name="timeout">The amount of time, in milliseconds, to wait for a response.</param>
            /// <returns></returns>
            public static ProgrammingConnection Connect(short timeout)
            {
                //this.timeout = timeout;

                if (Framework.Platform.isMonoDerived())
                    return new MonoProgrammingConnection();
                else
                    return new MonoProgrammingConnection();
                    //return new Win32ProgrammingConnection();
            }

            #endregion

            #region Public Methods

            /// <summary>
            /// Closes the target connection.
            /// </summary>
            abstract public void Close();

            /// <summary>
            /// 	Writes a page to the AVR.
            /// </summary>
            /// <param name="address">
            /// 	The page address is the address of the page to be written; it must be aligned to the page size.
            /// </param>
            /// <param name="data">
            /// 	The 256 bytes (128 words) to write.
            /// </param>
            public void WriteAVRPage(ushort address, byte[] data)
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

            #region Protected Methods

            /// <summary>
            /// 	Sends a packet to the boot-loader. Typically contains 2 bytes of control and 256 bytes of payload.
            /// </summary>
            /// <param name="data">
            /// A <see cref="System.Byte[]"/>
            /// </param>
            internal abstract void sendPacket(byte[] packet);

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
            protected byte[] prefixCommand(ProgrammerInstruction instr, byte[] data)
            {
                //Get the ushort code for the instruction.
                ushort instruction = (ushort)instr;

                //Create a new USB-HID packet.
                byte[] packet = new byte[130];

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


