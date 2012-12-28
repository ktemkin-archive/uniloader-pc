using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unilab.OS
{
    public static class Platform
    {
        /// <summary>
        ///     Returns true iff we're currently executing on the Mono platform.
        /// </summary>
        public static bool isMono()
        {
            //If the Mono runtime is there, we're running on Mono.
            return (Type.GetType("Mono.Runtime") != null);
        }

    }
}
