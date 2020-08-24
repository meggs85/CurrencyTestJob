using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyScanner
{
    public interface ILogger
    {
        void Log(string msg, bool error = false);
    }
}
