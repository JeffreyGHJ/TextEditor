using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module_7_Team_4
{
    interface IAddText
    {

        string FormText { get; set; }
    }

    [FlagsAttribute]
    enum KeyState
    {
        Left = 1,
        Right = 2,
        Shift = 4,
        Control = 8,
        Middle = 16,
        Alt = 32
    }
}
