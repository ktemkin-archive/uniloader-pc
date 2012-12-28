using System;

namespace Unilab.Hardware
{

    /// <summary>
    ///     Stores model reviison codes for various Unified Lab Kit devices.
    /// </summary>
    public enum Model : int
    {
        Basys100K = 0x01,
        Basys250K = 0x02,
        UnilabProto = 0x03
    }

    /// <summary>
    ///     Abstract class representing a Lab Kit device.
    /// </summary>
    public abstract class LabKit
    {
        /// <summary>
        ///     The microcontroller page size for the given Lab Kit.
        /// </summary>
        /// <returns></returns>
        public abstract ushort PageSize();

        /// <summary>
        ///     The microcontroller flash size for the given Lab Kit.
        /// </summary>
        /// <returns></returns>
        public abstract ushort FlashSize();

        /// <summary>
        ///     The bootloader size for the given lab kit.
        /// </summary>
        /// <returns></returns>
        public abstract ushort BootloadSize();

        /// <summary>
        ///     Returns the writeable memory on the micoprocessor
        /// </summary>
        /// <returns></returns>
        public virtual ushort WriteableFlash()
        {
            return (ushort)(FlashSize() - BootloadSize());
        }

    }

    /// <summary>
    ///     Basic lab kit based on the AT90USB microcontroller.
    /// </summary>
    public class AT90USBLabKit : LabKit
    {

        public override ushort PageSize()
        {
            return 128;
        }

        public override ushort FlashSize()
        {
            return 16 * 1024;
        }

        public override ushort BootloadSize()
        {
            return 4 * 1024;
        }
    }

    public class ATmega32U4LabKit : LabKit
    {

        public override ushort PageSize()
        {
            return 128;
        }

        public override ushort FlashSize()
        {
            return 32 * 1024;
        }

        public override ushort BootloadSize()
        {
            //This might change in the future FIXME
            return 4 * 1024;
        }
    }

    public class Basys100K : AT90USBLabKit
    {}

    public class Basys250K : AT90USBLabKit
    {}

    public class UnilabProto : ATmega32U4LabKit
    {}

	

	public static class CurrentLabKit
	{

        //Assume the unilab prototype.
        static LabKit device = new UnilabProto();
        static Model model = Model.UnilabProto;

        /// <summary>
        ///     Sets the type of the currently connected lab kit.
        /// </summary>
        /// <param name="model">The model of the current lab kit.</param>
        public static void setCurrentKit(Model model)
        {
            //don't bother creating new objects if we have the correct type.
            if (CurrentLabKit.model == model)
                return;
            
            //otherwise, correctly set the device and model
            CurrentLabKit.model = model;
            switch (model)
            {
                case Model.Basys100K:
                    device = new Basys100K();
                    Debug.DebugConsole.WriteLine("Detected 100K-gate Basys board...");
                    break;

                case Model.Basys250K:
                    device = new Basys250K();
                    Debug.DebugConsole.WriteLine("Detected 250K-gate Basys board...");
                    break;

                case Model.UnilabProto:
                    device = new UnilabProto();
                    Debug.DebugConsole.WriteLine("Detected a Unified Lab Kit prototype...");
                    break;
            }
        }

        /// <summary>
        ///     Sets the type of the currently connected lab kit.
        /// </summary>
        /// <param name="revisionCode">The USB revision code of the current model.</param>
        public static void setCurrentKit(int revisionCode)
        {
            //Convert the revision code into a model.
            Model model = (Model)revisionCode;

            //Then operate on the given model.
            setCurrentKit(model);
        }

        /// <summary>
        ///     Refers to the Flash page size of the current device's microcontroller.
        /// </summary>
        public static ushort PageSize
        {
            get
            {
                return device.PageSize();
            }
        }

        /// <summary>
        ///     Refers to the total size of the current device's microcontroller-flash.
        /// </summary>
        public static ushort FlashSize
        {
            get
            {
                return device.FlashSize();
            }
        }

        /// <summary>
        ///     Refers to the total size of the bootloader program built into the device's microcontroller.
        /// </summary>
        public static ushort BootloadSize
        {
            get
            {
                return device.BootloadSize();
            }
        }

        /// <summary>
        ///     Refers to the writeable size of the microcontroller's flash memory.
        /// </summary>
        public static ushort WriteableFlash
        {
            get
            {
                return device.WriteableFlash();
            }
        }
	}
}

