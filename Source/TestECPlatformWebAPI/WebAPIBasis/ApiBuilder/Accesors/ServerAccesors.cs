using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using WebAPIBasis.Utilities;
using WebAPIBasis.Literals;
using System.Runtime.CompilerServices;

namespace WebAPIBasis.Accesors
{
    /// <summary>
    /// 【 概要 】APIサーバー用のアクセサオブジェクト群を一括生成する、IAccesorsの具象クラス
    /// 【 対象 】DB接続情報、AppXXX.jsonアクセサ、DB接続プール、ロガーオブジェクト 
    /// </summary>
    /// 
    /// <readme>
    /// 1.ApiBuilderから基本的に生成してください ※例：ApiBuilder.Create().Build(); または ApiBuilder.Create().Code().Build();
    /// </readme>
    /// <author> Y.Ito  </author>
    public class ServerAccesors : IAccesors
    {
        #region Init(Field, Property, Constructor)
        private string _site;
        private string _connectionString;
        private IConfigurationRoot _configurationRoot;
        private OracleConnection _oracleConnection;
        private LoggerAPI _loggerAPI;

        public ServerAccesors(string siteCode)
        {
            _site = siteCode;
            var fnc = new ConfigUtils();
            _configurationRoot = fnc.BuildConfigurationRoot();
            _connectionString = fnc.GetConnectionString("");
            _oracleConnection = new DBUtils().CreateOracleConnection(_connectionString);
            _loggerAPI = new LoggerAPI("");
        }
        #endregion Init(Field, Property, Constructor)

        #region Getterメソッド群
        public string getCode() { return _site; }
        public string getConnectionString() { return _connectionString; }
        public IConfigurationRoot getAppsettings() { return _configurationRoot; }
        public OracleConnection getConnection() { return _oracleConnection; }
        public LoggerAPI getLoggerAPI() { return _loggerAPI; }
        public LoggerClient getLoggerClient() { return null; }
        public object getLockObject() { return null; }

        #endregion Getterメソッド群
    }
}
