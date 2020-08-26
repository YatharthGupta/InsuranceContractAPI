using InsuranceContractAPI.DBConnection;
using InsuranceContractAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceContractAPI.Services
{
    public interface IInsuraceServices
    {
        InsuranceContracts AddInsuranceContracts(InsuranceContracts contracts, IDbSqlConnection _connection, string connStr);
        Dictionary<String, InsuranceContracts> GetInsuranceContracts(IDbSqlConnection _connection, string connStr);
        InsuranceContracts DeleteInsuranceContract(InsuranceContracts contracts,IDbSqlConnection _connection, string connStr);

    }
}
