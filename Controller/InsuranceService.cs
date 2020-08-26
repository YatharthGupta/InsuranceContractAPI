using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InsuranceContractAPI.DBConnection;
using InsuranceContractAPI.Models;
using InsuranceContractAPI.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Data;
using InsuranceContractAPI.Common;

namespace InsuranceContractAPI.Controller
{
    [Route("v1/")]
    [ApiController]
    public class InsuranceService : ControllerBase
    {
        private readonly IInsuraceServices _services;
        private readonly IDbSqlConnection _connection;
        IConfiguration _iconfiguration;
        public string connStr = String.Empty;

        public InsuranceService(IInsuraceServices services, IConfiguration iconfiguration, IDbSqlConnection iconnection)
        {

            _iconfiguration = iconfiguration;
            _connection = iconnection;
            connStr = _iconfiguration.GetSection("Data").GetSection("ConnectionStrings").Value;
            

            _services = services;
        }

        [HttpPost]
        [Route("AddInsuranceContracts")]

        public ActionResult<InsuranceContracts> AddInsuranceContracts(InsuranceContracts contracts)
        {

            var insuranceContracts = _services.AddInsuranceContracts(contracts, _connection, connStr);

            if (insuranceContracts == null)
            {
                return NotFound();
            }
            return insuranceContracts;

        }

        [HttpGet]
        [Route("GetInsuranceContracts")]
        public ActionResult<Dictionary<string, InsuranceContracts>> GetInsuranceContracts()
        {

            var insuranceContracts = _services.GetInsuranceContracts(_connection, connStr);
            if (insuranceContracts == null)
            {
                return NotFound();
            }

            return insuranceContracts;
        }

        [HttpDelete]
        [Route("DeleteInsuranceContract")]
        public ActionResult<InsuranceContracts> DeleteInsuranceContract(InsuranceContracts contracts)
        {
            var insuranceContracts = _services.DeleteInsuranceContract(contracts, _connection, connStr);

            return Ok("Successfully Deleted the Contract");
        }
    }
}