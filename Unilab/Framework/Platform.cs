using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unilab.Framework
{
    static class Platform
    {
        /// <summary>
        /// Returns true iff the platform is Mono or a derivative
        /// </summary>
        /// <returns>True if and only the currently intepreter is Mono or Mono-derived.</returns>
        internal static bool isMonoDerived()
        {
           //Mono _always_ defines the Mono.Runtime type
           return (Type.GetType("Mono.Runtime") != null);
        }

    }
}
