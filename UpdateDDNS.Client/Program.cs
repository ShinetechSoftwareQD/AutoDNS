using GodaddyDDNS;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using UpdateDDNS.Aliyun;
using UpdateDDNS.Base;
using UpdateDDNS.Base.Properties;
using UpdateDDNS.GodaddyDDNS;

namespace UpdateDDNS.Client
{
    class Program
    {
        static void Main(string[] args)
        {

            LogHelper.Info(string.Format(Resources.ProgramStart, DateTime.Now.ToString("yyy-MM-dd hh:mm:ss")));
            LogHelper.Info(string.Format(Resources.LoadSettings, DateTime.Now.ToString("yyy-MM-dd hh:mm:ss")));

            var config = new ConfigurationBuilder()
                .AddInMemoryCollection()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings.json", optional: false, reloadOnChange: false)
                .Build();

            LogHelper.Info(string.Format(Resources.LoadObjects, DateTime.Now.ToString("yyy-MM-dd hh:mm:ss")));

            var service = new ServiceCollection()
               .AddOptions()
               .Configure<AliyunSettings>(config.GetSection("AliyunSettings"))
               .Configure<GodaddySettings>(config.GetSection("GodaddySettings"))
               .AddSingleton<AliyunService, AliyunService>()
               .AddSingleton<GodaddyService, GodaddyService>()
               .BuildServiceProvider();



            IService helper;
            // var services  = service.GetServices<IService>();
            LogHelper.Info(string.Format(Resources.LoadObject, DateTime.Now.ToString("yyy-MM-dd hh:mm:ss"), config["Provider"]));
            switch (config["Provider"])
            {
                case "Godaddy":
                    helper = service.GetService<GodaddyService>();
                    break;
                case "Aliyun":
                    helper = service.GetService<AliyunService>();
                    break;
                default:
                    LogHelper.Error("Provide参数只能接受'Godaddy'或者是'Aliyun'");
                    throw new NotImplementedException("Provide参数只能接受'Godaddy'或者是'Aliyun'");
            }


            try
            {
                helper.CheckAndUpdate().GetAwaiter().GetResult();
            }
            catch (Exception exp)
            {
                LogHelper.Error(exp.Message);
            }


            LogHelper.Info(string.Format(Resources.ProgramEnd, DateTime.Now.ToString("yyy-MM-dd hh:mm:ss")));



        }
    }
}
