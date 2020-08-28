﻿using log4net;
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
            //master再次添加
            //master添加注释
            //dev添加注释
            ILog log = LogManager.GetLogger(typeof(Log4NetTest));
            log.Info("info");
            log.Error("Error");
        }
    }
}
