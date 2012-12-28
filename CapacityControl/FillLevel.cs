using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FilledContainer
{
    /// <summary>
    /// Describes any partially filled container.
    /// </summary>
    public class FillLevel
    {
        /// <summary>
        ///     Internal measure of device occupation.
        /// </summary>
        int actual = 0;

        /// <summary>
        ///     Internal measure of device capacity.
        /// </summary>
        int max = 0;
        
        /// <summary>
        ///     Description of the device.
        /// </summary>
        string description = "Device";

        /// <summary>
        ///     Describes the occupied space in the container.
        /// </summary>
        public string LabelActual = "";

        /// <summary>
        ///     Describes the 'free' space in the container.
        /// </summary>
        public string LabelRemainder = "";

        /// <summary>
        ///     Descrives the space which has been allocated beyond container capacity.
        /// </summary>
        public string LabelOverflow = "";

        /// <summary>
        ///     The actual amount the container is filled. Must not be greated than the maximum.
        /// </summary>
        public int Actual
        {
            get
            {
                return actual;
            }
            set
            {
                actual = value;
            }
        }

        /// <summary>
        ///     The maxmium container of the measure.
        /// </summary>
        public int Max
        {
            get
            {
                return max;
            }
            set
            {
                max = value;
            }
        }

        /// <summary>
        ///     The amount of free space in the container.
        /// </summary>
        public int Remaining
        {
            get
            {
                return Math.Max(Max - Actual, 0);
            }
        }

        /// <summary>
        ///     Returns the extent to which the contents exceed their container.
        /// </summary>
        public int Overflow
        {
            get
            {
                return Math.Max(Actual - Max, 0);
            }
        }

        /// <summary>
        ///     Returns true iff the container is overflowing.
        /// </summary>
        public bool IsOverflowing
        {
            get
            {
                return (actual > max);
            }
        }


        /// <summary>
        ///     A description of the described measure.
        /// </summary>
        public string Description
        {
            get
            {
                return description;
            }
        }



        
        /// <summary>
        ///     Create a new Fill Level, with initial values.
        /// </summary>
        /// <param name="description">A description of the device being filled.</param>
        /// <param name="actual">The actual amount the device is filled.</param>
        /// <param name="max">The maximum capacity of the device.</param>
        /// <param name="labelActual"></param>
        /// <param name="labelRemainder"></param>
        public FillLevel(string description, int actual, int max, string labelActual, string labelRemainder, string labelOverflow)
            : this(description, actual, max)
        {
            LabelActual = labelActual;
            LabelRemainder = labelRemainder;
            labelOverflow = labelOverflow;
        }

        /// <summary>
        ///     Create a new Fill Level, with initial values.
        /// </summary>
        /// <param name="description">A description of the device being filled.</param>
        /// <param name="actual">The actual amount the device is filled.</param>
        /// <param name="max">The maximum capacity of the device.</param>
        public FillLevel(string description, int actual, int max)
            : this(actual, max)
        {
            this.description = description;
        }

        /// <summary>
        ///     Create a new Fill Level, with initial values.
        /// </summary>
        /// <param name="actual">The actual amount the device is filled.</param>
        /// <param name="max">The maximum capacity of the device.</param>
        public FillLevel(int actual, int max)   
        {
            this.actual = actual;
            this.max = max;
        }

    }
}
