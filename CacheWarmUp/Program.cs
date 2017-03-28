using CacheWarmUp.Core;
using NLog;
using System.Diagnostics;
using System.Linq;

namespace CacheWarmUp
{
    class Program
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();


        static void Main(string[] args)
        {
            try
            {
                ApiClient client = new ApiClient(Settings.Default.BaseUrl,
                                                 Settings.Default.Username,
                                                 Settings.Default.Password);
                log.Info("Logging in....");
                client.Logon();

                log.Info("Loading list of business partners...");
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
            catch (System.Exception ex)
            {
                log.Error(ex);
            }
        }
    }
}
