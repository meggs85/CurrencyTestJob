using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CurrencyScanner
{
    public class FormLogger : ILogger
    {
        ListBox list;
        ErrorProvider ep;
        
        public FormLogger(ListBox list, ErrorProvider ep)
        {
            this.list = list;
            this.ep = ep;
        }

        public void Log(string msg, bool error = false)
        {
            // на скорую руку костыль-убийца лишних пробелов
            msg.Replace("         ", ""); 
            msg.Replace("        ", ""); 
            msg.Replace("       ", ""); 
            msg.Replace("      ", ""); 
            msg.Replace("     ", ""); 
            msg.Replace("    ", "");
            msg.Replace("   ", "");
            msg.Replace("  ", "");
            msg.Replace(" ", "");
            list.Items.Insert(0, msg);
            if (ep!= null && error) ep.SetError(list, msg);
        }
    }
}
