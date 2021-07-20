using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module_7_Team_4
{
    interface ITextDialog
    {

        Document document { get; set; }
        int docPos { get; set; }
        event EventHandler Apply;

    }
}
