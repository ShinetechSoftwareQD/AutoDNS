using GodaddyDDNS;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UpdateDDNS.Base;
using UpdateDDNS.Base.Properties;

namespace UpdateDDNS.GodaddyDDNS
{
    public class GodaddyService : DDNSBase, IService
    {
        private readonly IOptions<GodaddySettings> _settings;
        private HttpClient _client;

        // private string getRecords = "https://api.ote-godaddy.com/v1/domains/{domain}/records/{type}/{name}";

        public GodaddyService(IOptions<GodaddySettings> setting)
        {
            _settings = setting;

            string authHeader = $"{_settings.Value.Key}:{_settings.Value.Secret}";
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("sso-key", authHeader);
        }

        public async Task CheckAndUpdate()
        {
            LogHelper.Info(string.Format(Resources.DetectName, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), _settings.Value.Name, _settings.Value.Domain));
       
            string requestUrl = string.Format(Constant.getRecordsPath, _settings.Value.Domain, "A", _settings.Value.Name);
            HttpResponseMessage response = await _client.GetAsync(requestUrl);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                List<DomainModel> records = JsonConvert.DeserializeObject<List<DomainModel>>(result);
                if (records.Count == 1)
                {
 
                    string ip = records[0].Data;
                    string localIp = this.GetLocalIP(_settings.Value.MyIP);
                    if (ip != localIp)
                    {
                        LogHelper.Info(string.Format(Resources.NeedUpdate, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), localIp, ip));
                        try
                        {
                            LogHelper.Info(string.Format(Resources.StartUpdate, 
                                DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),  
                                _settings.Value.Name, 
                                _settings.Value.Domain, 
                                localIp));

                            await UpdateDNS(localIp);
                        }
                        catch (Exception exp)
                        {
                            LogHelper.Info($"Godaddy解析记录更新异常.{exp.Message}");
                        }
                    }
                    else
                    {
                        LogHelper.Info(string.Format(Resources.NoUpdate,
                           DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")));
                    }
                }
                else
                {
                    LogHelper.Info(string.Format(Resources.NoRecord,
                   DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),
                   _settings.Value.Name,
                   _settings.Value.Domain));
                }
            }
            else
            {
             
                LogHelper.Info(response.ReasonPhrase);
            }
        }


        //put
        public async Task UpdateDNS(string ip)
        {
            //var jsonString = "[{\"data\": \"string\",\"port\": 0,\"priority\": 0,\"protocol\": \"string\",\"service\": \"string\",\"ttl\": 0,\"weight\": 0}]";

            List<RecordModel> models = new List<RecordModel>();
            models.Add(new RecordModel()
            {
                Data = ip,
                TTL = 3600,
                Port = 1,
                Weight = 1,
                Priority = 1,
                Protocol = "string",
                Service = "string",
            });

            string jsonString = JsonConvert.SerializeObject(models);
            var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

            string requestUrl = string.Format(Constant.getRecordsPath, _settings.Value.Domain, "A", _settings.Value.Name);

            var response = await _client.PutAsync(requestUrl, httpContent);
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
          
                LogHelper.Info(string.Format(Resources.EndUpdate,
               DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),
               _settings.Value.Name,
               _settings.Value.Domain,
               ip));
            }
            else
            {
                LogHelper.Info(response.ReasonPhrase);
            }

        }




    }
}
