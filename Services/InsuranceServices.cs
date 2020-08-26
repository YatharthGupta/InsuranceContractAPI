using InsuranceContractAPI.Common;
using InsuranceContractAPI.DBConnection;
using InsuranceContractAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;


namespace InsuranceContractAPI.Services
{
    public class InsuranceServices : IInsuraceServices
    {
        private readonly Dictionary<string, InsuranceContracts> _InsuranceContracts;

        public InsuranceServices()
        {
            _InsuranceContracts = new Dictionary<string, InsuranceContracts>();
        }
        public InsuranceContracts AddInsuranceContracts(InsuranceContracts contracts, IDbSqlConnection _connection, string connStr)
        {

            int age;
            bool isValid = false;
            // To be moved to stored procedure or use EnitityFramework ,could not use EntityFramework due to very limited time.
            string sqlEligibility = "select * from CoveragePlanEligibility";
            DataSet dsEligibility = _connection.LoaderDataSet(sqlEligibility, connStr);

            if (dsEligibility.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsEligibility.Tables[0].Rows)
                {
                    if (contracts.customerAddress.Contains(dr["eligibilityCountry"].ToString()))
                    {
                        contracts.customerCountry = dr["eligibilityCountry"].ToString();
                        if (Convert.ToDateTime(contracts.saleDate) >= Convert.ToDateTime(dr["eligibilitydatefrom"]) && Convert.ToDateTime(contracts.saleDate) <= Convert.ToDateTime(dr["eligibilitydateto"]))
                        {
                            contracts.coverageplan = dr["coverageplan"].ToString();
                            isValid = true;
                        }

                    }
                }
            }

            if (isValid)
            {
                age = DateTimeExtensions.Age((Convert.ToDateTime(contracts.customerDOB)));

                string sqlRate = "select * from CoveragePlanRateChart"; // To be moved to stored procedure or use EnitityFramework ,could not use EntityFramework due to very limited time.
                DataSet dsRate = _connection.LoaderDataSet(sqlRate, connStr);
                string agefilter = string.Empty;
                string coveragePlanFromRateChart = string.Empty;

                if (age <= 40)
                {
                    agefilter = "<=40";
                }
                else if (age > 40)
                {
                    agefilter = ">40";
                }

                if (dsRate.Tables[0].Rows.Count > 0)
                {
                    DataRow[] dr = dsRate.Tables[0].Select("coverageplan='" + contracts.coverageplan.ToString() + "' and gender='" + contracts.customerGender.ToString() + "' and age ='" + agefilter + "'");
                    coveragePlanFromRateChart = dr[0].ItemArray[4].ToString();
                }

                contracts.netPrice = coveragePlanFromRateChart;



                string sqlInsertcontracts = "INSERT INTO [InsuranceContract].[dbo].[Contracts] ([customerName] ,[customerAddress] ,[customerGender] ,[customerCountry] ,[customerDOB] ,[saleDate] ,[coverageplan] ,[netPrice]) VALUES " +
                    "('" + contracts.customerName + "','" + contracts.customerAddress + "','" + contracts.customerGender + "','" + contracts.customerCountry + "','" + contracts.customerDOB + "','" + contracts.saleDate + "','" + contracts.coverageplan + "','" + contracts.netPrice + "')";


                int result = _connection.ExecuteNonQuery(sqlInsertcontracts, connStr);

                contracts = new InsuranceContracts();

                contracts = RetrieveUpdatedInsuranceContracts(_connection, connStr);

                //  _InsuranceContracts.Add(contracts.customerName, contracts);
                return contracts;
            }

            return null;
        }


        public InsuranceContracts RetrieveUpdatedInsuranceContracts(IDbSqlConnection _connection, string connStr)
        {

            InsuranceContracts contracts = new InsuranceContracts();
            string sqlSelectContracts = "Select * from [InsuranceContract].[dbo].[Contracts]";

            DataSet dsContracts = _connection.LoaderDataSet(sqlSelectContracts, connStr);

            if (dsContracts.Tables[0].Rows.Count > 0)
            {

                foreach (DataRow dr in dsContracts.Tables[0].Rows)
                {
                    contracts.contractId = Convert.ToInt32(dr["contractid"]);
                    contracts.customerName = dr["customerName"].ToString();
                    contracts.customerAddress = dr["customerAddress"].ToString();
                    contracts.customerGender = dr["customerGender"].ToString();
                    contracts.customerCountry = dr["customerCountry"].ToString();
                    contracts.customerDOB = Convert.ToDateTime(dr["customerDOB"]).Date.ToString();
                    contracts.saleDate = Convert.ToDateTime(dr["saleDate"].ToString()).Date.ToString();
                    contracts.coverageplan = dr["coverageplan"].ToString();
                    contracts.netPrice = dr["netPrice"].ToString();


                }

                return contracts;
            }

            return null;
        }


        public Dictionary<string, InsuranceContracts> GetInsuranceContracts(IDbSqlConnection _connection, string connStr)
        {
            InsuranceContracts contracts = new InsuranceContracts();
            string sqlSelectContracts = "Select * from [InsuranceContract].[dbo].[Contracts]";

            _InsuranceContracts.Clear();
            DataSet dsContracts = _connection.LoaderDataSet(sqlSelectContracts, connStr);

            if (dsContracts.Tables[0].Rows.Count > 0)
            {

                foreach (DataRow dr in dsContracts.Tables[0].Rows)
                {
                    contracts = new InsuranceContracts();
                    contracts.contractId = Convert.ToInt32(dr["contractid"]);
                    contracts.customerName = dr["customerName"].ToString();
                    contracts.customerAddress = dr["customerAddress"].ToString();
                    contracts.customerGender = dr["customerGender"].ToString();
                    contracts.customerCountry = dr["customerCountry"].ToString();
                    contracts.customerDOB = Convert.ToDateTime(dr["customerDOB"]).Date.ToString();
                    contracts.saleDate = Convert.ToDateTime(dr["saleDate"].ToString()).Date.ToString();
                    contracts.coverageplan = dr["coverageplan"].ToString();
                    contracts.netPrice = dr["netPrice"].ToString();

                    _InsuranceContracts.Add(contracts.contractId.ToString(), contracts);
                }
                return _InsuranceContracts;
            }

            return null;
        }

        public InsuranceContracts DeleteInsuranceContract(InsuranceContracts contracts, IDbSqlConnection _connection, string connStr)
        {
            string sqlInsertcontracts = "DELETE From Contracts where contractId =" + contracts.contractId;

            int result = _connection.ExecuteNonQuery(sqlInsertcontracts, connStr);

            return contracts;
        }


    }

}