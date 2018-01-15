using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    /// <summary>
    /// a factory for implementations of BL
    /// </summary>
    public class BL_Factory
    {
        /// <summary>
        /// gets an instance of the implementation of the IDAL
        /// </summary>
        public static IBL Instance { get => BL_imp.Instance; } //editable to switch to any other implementation
        //static BL_imp instance = null;
        //public static BL_imp Instance
        //{
        //    get
        //    {
        //        if (instance == null)
        //            instance = new BL_imp();
        //        return instance;
        //    }
        //}
    }
}
