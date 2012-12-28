using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//This file liberally modified from a journal article from the Visual Studio Journal
//by Ashley Deakin

namespace Unilab.Win32
{
    public abstract class HIDReport
    {
        #region Member variables

        /// <summary>Buffer for raw report bytes</summary>
        private byte[] m_arrBuffer;
        /// <summary>Length of the report</summary>
        private int m_nLength;
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="oDev">Constructing device</param>
        public HIDReport(HIDDevice oDev)
        {
            // Do nothing
        }

        /// <summary>
        /// Sets the raw byte array.
        /// </summary>
        /// <param name="arrBytes">Raw report bytes</param>
        protected void SetBuffer(byte[] arrBytes)
        {
            m_arrBuffer = arrBytes;
            m_nLength = m_arrBuffer.Length;
        }

        /// <summary>
        /// Accessor for the raw byte buffer
        /// </summary>
        public byte[] Buffer
        {
            get
            {
                return m_arrBuffer;
            }
        }
        /// <summary>
        /// Accessor for the buffer length
        /// </summary>
        public int BufferLength
        {
            get
            {
                return m_nLength;
            }
        }
    }
    /// <summary>
    /// Defines a base class for output reports. To use output reports, just put the bytes into the raw buffer.
    /// </summary>
    public abstract class HIDOutputReport : HIDReport
    {
        /// <summary>
        /// Construction. Setup the buffer with the correct output report length dictated by the device
        /// </summary>
        /// <param name="oDev">Creating device</param>
        public HIDOutputReport(HIDDevice oDev)
            : base(oDev)
        {
            SetBuffer(new byte[oDev.OutputReportLength]);
        }
    }
    /// <summary>
    /// Defines a base class for input reports. To use input reports, use the SetData method and override the 
    /// ProcessData method.
    /// </summary>
    public abstract class HIDInputReport : HIDReport
    {
        /// <summary>
        /// Construction. Do nothing
        /// </summary>
        /// <param name="oDev">Creating device</param>
        public HIDInputReport(HIDDevice oDev)
            : base(oDev)
        {
        }
        /// <summary>
        /// Call this to set the buffer given a raw input report. Calls an overridable method to
        /// should automatically parse the bytes into meaningul structures.
        /// </summary>
        /// <param name="arrData">Raw input report.</param>
        public void SetData(byte[] arrData)
        {
            SetBuffer(arrData);
            ProcessData();
        }
        /// <summary>
        /// Override this to process the input report into something useful
        /// </summary>
        public abstract void ProcessData();
    }
}
