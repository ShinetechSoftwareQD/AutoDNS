using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Core.Http;
using System.Linq;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UpdateDDNS.Base;
using Aliyun.Acs.Ecs.Model.V20140526;
using Aliyun.Acs.Alidns.Model.V20150109;
using UpdateDDNS.Base.Properties;

namespace UpdateDDNS.Aliyun
{
    public class AliyunService : DDNSBase, IService
    {
        private readonly IOptions<AliyunSettings> _settings;

        private IAcsClient _client;

        public AliyunService(IOptions<AliyunSettings> setting)
        {
            _settings = setting;
            IClientProfile profile = DefaultProfile.GetProfile("cn-hangzhou", _settings.Value.Key, _settings.Value.Secret);
            _client = new DefaultAcsClient(profile);

        }

        public async Task CheckAndUpdate()
        {
            LogHelper.Info(string.Format(Resources.DetectName, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), _settings.Value.Name, _settings.Value.Domain));

            DescribeDomainRecordsRequest request = new DescribeDomainRecordsRequest();
            request.DomainName = _settings.Value.Domain;
            try
            {

                DescribeDomainRecordsResponse response = _client.GetAcsResponse(request);
                var records = response.DomainRecords;
                var record = records.Find(s => s.RR == _settings.Value.Name);
                if (record != null)
                {
                    string localIp = this.GetLocalIP(_settings.Value.MyIP);
                    string ip = record.Value;
                    if (ip != localIp)
                    {
                        LogHelper.Info(string.Format(Resources.NeedUpdate, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), localIp, ip));

                        await this.UpdateDNS(localIp, record.RecordId);
                    }
                    else
                    {
                        LogHelper.Info(string.Format(Resources.NoUpdate, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")));
                    }
                }
                else
                {
                    LogHelper.Info(string.Format(Resources.NoRecord, 
                        DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), _settings.Value.Name, _settings.Value.Domain));

                }
            }
            catch (Exception exp)
            {
                LogHelper.Error(exp.Message);
            }

        }


        public async Task UpdateDNS(string ip, string recordId)
        {
            UpdateDomainRecordRequest request = new UpdateDomainRecordRequest();
            request.RecordId = recordId;
            request.RR = _settings.Value.Name;
            request.Value = ip;
            request.Type = "A";
            try
            {
                UpdateDomainRecordResponse response = _client.GetAcsResponse(request);
                LogHelper.Info(string.Format(Resources.EndUpdate,
                    DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),
                    _settings.Value.Name,
                    _settings.Value.Domain,
                    ip));

            }
            catch (Exception exp)
            {
                LogHelper.Error(exp.Message);
            }
        }


    }
}
