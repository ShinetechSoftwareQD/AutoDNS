using System;
using System.Threading.Tasks;

namespace UpdateDDNS.Base
{
    public interface IService
    {
        Task CheckAndUpdate();
    }
}
