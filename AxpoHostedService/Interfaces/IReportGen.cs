using Axpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AxpoHostedService
{
    public interface IReportGen
    {
        public void SaveReport(IEnumerable<PowerTrade> powerTrades, DateTime moment);
    }
}
