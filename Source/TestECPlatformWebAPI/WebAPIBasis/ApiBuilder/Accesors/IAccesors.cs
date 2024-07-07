using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using WebAPIBasis.Utilities;

namespace WebAPIBasis.Accesors
{
    /// <summary>
    /// 【 概要 】アクセサオブジェクト群、SiteAccesorsを一括更新する、IAccesorsの具象クラス
    /// 【 対象 】DB接続情報、AppXXX.jsonアクセサ、DB接続プール、ロガーオブジェクト 
    /// </summary>
    /// <readme>
    /// </readme>
    /// <author> Y.Ito  </author>
    public interface IAccesors
    {
        public string getCode();
        public string getConnectionString();
        public IConfigurationRoot getAppsettings();
        public OracleConnection getConnection();
        public LoggerAPI getLoggerAPI();
        public LoggerClient getLoggerClient();
        public object getLockObject();
    }
}
