using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceContractAPI.DBConnection
{
    public interface IDbSqlConnection
    {
        int ExecuteNonQuery(string sql,string connection);
        DataSet LoaderDataSet(string StrSql, string connection);
    }
}
