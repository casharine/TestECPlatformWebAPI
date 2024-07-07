using Microsoft.Extensions.Configuration;
using System.Data;
using System.Reflection;
using WebAPIBasis.Utilities;

namespace WebAPIBasis.Utilities
{
    public class ConfigUtils
    {
        /// <summary>
        /// appsettings.json から値を取得するためのBuilder、IConfigurationを実装します
        /// </summary>
        /// <returns></returns>
        /// <author>Y.Ito </author>
        public IConfigurationRoot BuildConfigurationRoot()
        {
            var directory = Directory.GetParent(Assembly.GetExecutingAssembly().Location)!.ToString();

            var builder = new ConfigurationBuilder()
                .SetBasePath(directory)
                .AddJsonFile("appsettings.json", optional: false);
            var cnf = builder.Build();

            return cnf;
        }

        /// <summary>
        /// 引数でログを残すDBを切り替えます
        /// デフォルト値：Jsonからサイト名のコネクションストリングを取得
        /// コード指定：DBからのコネクションストリングを取得  
        /// </summary>
        /// <returns></returns>
        /// <author>Y.Ito </author>
        public string GetConnectionString(string iSITE_CD = "")
        {
            var cnf = BuildConfigurationRoot();
            var strCn = cnf[$"AppSettings:ConnectionString"];

            if (iSITE_CD != "")
            {
                var cn = new DBUtils().CreateOracleConnection(strCn);
                // 連携管理用DBから接続先情報を取得
                var dt = new DataTable();
                strCn = dt.Rows.Count <= 0 ? "" : dt.Rows[0]["CONNECTION_STRINS"].ToString()!;
            }

            return strCn;
        }
    }
}
