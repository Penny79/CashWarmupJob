using CacheWarmUp.Core;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CacheWarmUp
{
    class Program
    {
        private static Logger log = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {

            ApiClient client = new ApiClient(Settings.Default.BaseUrl, 
                                             Settings.Default.Username, 
                                             Settings.Default.Password);

            client.Logon();

            var commodities = client.ListBusinessPartners().GroupBy(x => x.commodity);

       

            foreach (var currentCommodity in commodities)
            {
                log.Info("------------------------------------");
                log.Info("Processing {0} business partners", currentCommodity.Key);
                log.Info("------------------------------------");

                log.Info("Fetched {0} business partners to process.", currentCommodity.Count());
                
                foreach (var businessPartner in currentCommodity)
                {
                    log.Info("Processing {0}", businessPartner.displayName);

                    client.SetBusinessPartner(businessPartner);

                    client.LoadMasterData(businessPartner);
                }
            }

            log.Info("Job completed");
        }
    }
}
