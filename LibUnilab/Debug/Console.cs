using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unilab.Debug
{
    /// <summary>
    ///     Debug and status console for normal-priority status messages (low verbosity).
    /// </summary>
    public static class DebugConsole
    {
        //Callback function for console writes.
        public delegate void writeTarget(string line);

        /// <summary>
        ///     Internal list of console write callbacks.
        /// </summary>
        static List<writeTarget> targets = new List<writeTarget>();

        /// <summary>
        ///     Binds a new callback function which will handle debug console writes.
        ///     Multiple callbacks are accepted.
        /// </summary>
        /// <param name="target">The callback function to add.</param>
        public static void bindConsole(writeTarget target)
        {
            //Add the given callback to the list
            targets.Add(target);
        }

        /// <summary>
        ///     Removes a callback function from the callback queue.
        /// </summary>
        /// <param name="target">The callback function to remove.</param>
        public static void unbindConsole(writeTarget target)
        {
            //Remove the given callback from the list
            targets.Remove(target);
        }

        /// <summary>
        ///     Writes an unterminated line to each of the bound writeback queues.
        /// </summary>
        /// <param name="line">The line to write.</param>
        public static void Write(string line)
        {
            DateTime now = DateTime.Now;

            foreach (writeTarget target in targets) 
                target("[" + now.ToShortTimeString() + "]  " + line);
        }

        /// <summary>
        ///     Writes a newline-terminated line to each of the bound writeback queues.
        /// </summary>
        /// <param name="line">The line to write.</param>
        public static void WriteLine(string line)
        {
            //Writes the message, with a newline character (or character pair) appended.
            Write(line + Environment.NewLine);
        }

    }
    
    /// <summary>
    ///     Debug and status console for low-priority status messages (high verbosity).
    /// </summary>
    public static class VerboseConsole
    {
        //Callback function for console writes.
        public delegate void writeTarget(string line);

        /// <summary>
        ///     Internal list of console write callbacks.
        /// </summary>
        static List<writeTarget> targets = new List<writeTarget>();

        /// <summary>
        ///     Binds a new callback function which will handle debug console writes.
        ///     Multiple callbacks are accepted.
        /// </summary>
        /// <param name="target">The callback function to add.</param>
        public static void bindConsole(writeTarget target)
        {
            //Add the given callback to the list
            targets.Add(target);
        }

        /// <summary>
        ///     Removes a callback function from the callback queue.
        /// </summary>
        /// <param name="target">The callback function to remove.</param>
        public static void unbindConsole(writeTarget target)
        {
            //Remove the given callback from the list
            targets.Remove(target);
        }

        /// <summary>
        ///     Writes an unterminated line to each of the bound writeback queues.
        /// </summary>
        /// <param name="line">The line to write.</param>
        public static void Write(string line)
        {
            DateTime now = DateTime.Now;

            foreach (writeTarget target in targets)
                target("<" + now.ToShortTimeString() + ">  " + line);
        }

        /// <summary>
        ///     Writes a newline-terminated line to each of the bound writeback queues.
        /// </summary>
        /// <param name="line">The line to write.</param>
        public static void WriteLine(string line)
        {
            //Writes the message, with a newline character (or character pair) appended.
            Write(line + Environment.NewLine);
        }

    }
}
