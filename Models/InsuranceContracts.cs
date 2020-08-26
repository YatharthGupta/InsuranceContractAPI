using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceContractAPI.Models
{
    public class InsuranceContracts
    {
        public int contractId { get; set; }
        public string customerName { get; set; }
        public string customerAddress { get; set; }
        public string customerGender { get; set; }
        public string customerCountry { get; set; }
        public string customerDOB { get; set; }
        public string saleDate { get; set; }
        public string coverageplan { get; set; }
        public string netPrice { get; set; }


    }
}
