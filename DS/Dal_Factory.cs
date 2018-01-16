using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS
{
    /// <summary>
    /// a factory for implementations of IDAL
    /// </summary>
     public class Dal_Factory // I know it is odd to have this in DS, but if it weren't for the specific instructions, DS and DAL would be the same namespace anyway...
    {
        /// <summary>
        /// gets an instance of the implementation of the IDAL
        /// </summary>
        public static DAL.Idal Instance { get => DAL.Dal_XML_imp.Instance; } //editable to switch to any other implementation
    }
}
