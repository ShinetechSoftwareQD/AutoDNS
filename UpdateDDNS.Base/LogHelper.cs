using log4net;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config", ConfigFileExtension = "config", Watch = true)]
namespace UpdateDDNS.Base
{
    public class LogHelper
    {
        private static ILog log = LogManager.GetLogger(Assembly.GetCallingAssembly(), "UpdateDDNSLog");

        public static void Info(string message)
        {
            //string msg = string.Format("{0}:{1}", da)
            log.Info(message);
            Console.WriteLine(message);
        }

        public static void Error(string message)
        {

            log.Error(message);
        }
        //public static void Info(string message, Action RegistedProperties)
        //{
        //    RegistedProperties();
        //    log.Info(message);
        //}

        //public static void Info(string message, Exception exception, Action RegistedProperties)
        //{
        //    RegistedProperties();
        //    log.Debug(message, exception);
        //}

        //public static void Info(Exception exception, Action RegistedProperties)
        //{
        //    RegistedProperties();
        //    log.Info("系统Info信息", exception);
        //}

    }
}
