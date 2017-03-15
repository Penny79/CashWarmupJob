using System.Collections.Generic;

namespace CacheWarmUp.Model
{

    public class Selected
    {
        public BusinessPartner GAS { get; set; }
        public BusinessPartner ELECTRICITY { get; set; }
    }

    public class BusinessPartner
    {
        public string crmId { get; set; }
        public string displayName { get; set; }
        public string commodity { get; set; }
    }


    public class Selectable
    {
        public List<BusinessPartner> GAS { get; set; }
        public List<BusinessPartner> ELECTRICITY { get; set; }
    }

    public class RootObject
    {
        public Selected selected { get; set; }
        public Selectable selectable { get; set; }
        public bool multipleChoicesAvailable { get; set; }
    }

}
