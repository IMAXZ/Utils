using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Log4NetTest
    {
        static void Main(string[] args)
        {
            ILog log = LogManager.GetLogger(typeof(Log4NetTest));
            log.Info("info");
            log.Error("Error");
        }
    }
}
