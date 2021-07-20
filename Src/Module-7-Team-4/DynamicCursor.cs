using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Module_7_Team_4
{
    class DynamicCursor
    {
        [DllImport("User32.dll")]
        private static extern IntPtr LoadCursorFromFile(String str);
        public static Cursor Create(string fname)
        {
            // how to handle cursor.
            IntPtr HC = LoadCursorFromFile(fname);
            // Check  wheather it is succeeded or not.
            if (!IntPtr.Zero.Equals(HC))
            {
                return new Cursor(HC);
            }
            else
            {
                throw new ApplicationException("cursor was not created from file " + fname);
            }
        }
    }
}

