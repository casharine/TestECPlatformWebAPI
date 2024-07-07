using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Data.Common;
using System.Runtime.CompilerServices;
using Microsoft.VisualBasic.CompilerServices;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;


namespace WebAPIBasis.Utilities
{


    /// <summary>
    /// DBに関する便利機能クラス
    /// </summary>
    /// <author>Y.Ito </author>
    public class DBUtils
    {
        [CompilerGenerated]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DbProviderFactory providerFactory = null!;
        /// <summary> コネクションストリング </summary>
        private string strCn { get; set; } = null!;

        /// <summary>
        /// コネクションを生成
        /// </summary>
        /// <returns></returns>
        public OracleConnection CreateOracleConnection(string strCn = "")
        {
            if (strCn == "")
            {
                var cnf = new ConfigUtils().BuildConfigurationRoot();
                strCn = cnf[$"AppSettings:ConnectionString"]!;
            }

            var cn = new OracleConnection(strCn);

            return cn;
        }

        /// <summary>
        /// 接続プール制御：クライアント依存 - SQL文を実行しDataTableで値を返す
        /// </summary>
        /// <param name="strSQL"></param>
        /// <param name="cn"></param>
        /// <returns></returns>
        public DataTable executeSelectQuery(string strSQL, OracleConnection cn)
        {
            var stateChange = false;
            if (cn.State.ToString() == "Closed")
            {
                cn.Open();
                stateChange = true;
            }

            OracleCommand cmd = new OracleCommand(strSQL, cn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataTable dt = new DataTable();

            da.Fill(dt);

            if (stateChange == true) cn.Close();

            return dt;
        }

        /// <summary>
        /// 接続プール制御：ホスト依存 - SQL文を実行、DataTableで値を返す
        /// </summary>
        /// <param name="strSQL"></param>
        /// <param name="iSITE"></param>
        /// <returns></returns>
        public DataTable executeSelectQuery(string strSQL, string iStrCn = "")
        {
            var ds = new DataTable();

            // default(コード空)の場合はECPlatformとなる
            using (var cn = CreateOracleConnection(iStrCn))
            {
                try
                {
                    OracleCommand cmd = new OracleCommand(strSQL, cn);
                    OracleDataAdapter da = new OracleDataAdapter(cmd);
                    ds = new DataTable();

                    cn.Open();
                    da.Fill(ds);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    cn.Close();
                }
            }
            return ds;
        }

            /// <summary>
        /// 接続プール制御：ホスト依存 - SQL文を実行、DataTableで値を返す
        /// </summary>
        /// <param name="strSQL"></param>
        /// <param name="iSITE"></param>
        /// <returns></returns>
        public int executeNonQuery(string strSQL, ref string msg, string iStrCn = "")
        {
            var res = 0;

            using (var cn = CreateOracleConnection(iStrCn))
            {
                cn.Open();

                OracleCommand command = cn.CreateCommand();
                OracleTransaction transaction = cn.BeginTransaction(IsolationLevel.ReadCommitted);
                command.Transaction = transaction;

                try
                {
                    command.Connection = cn;
                    command.CommandText = strSQL;

                    //クエリ実行（戻り値:更新件数）
                    res = command.ExecuteNonQuery();
                    msg = $"[コミット:{res}件][Query:{strSQL}]";

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    msg = $"[コミット:{res}件、例外が発生しました][Query:{strSQL}][例外Msg:{ex.Message}]";
                }
                cn.Close();
            }
            return res;
        }
    }
}
