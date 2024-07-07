using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPIBasis.Accesors;
using WebAPIBasis.Utilities;

namespace WebAPIBasis.ApiBuilder
{
    /// <summary>
    /// ApiBuiderを定義するクラス
    /// </summary>
    /// <typeparam name="TCode"></typeparam>
    public class ApiBuilder<TCode> where TCode : IOpt
    {
        internal readonly string Code = "";

        internal ApiBuilder(string code) => (Code) = (code);
    }

    /// <summary>
    /// 本Apiやクライアント側の設定情報を一括で取得するオブジェクトのBuider
    /// </summary>
    public static class ApiBuilder
    {
        /// <summary> APIの設定情報取得のためのBuilderObject生成部 *引数不要 </summary>
        public static ApiBuilder<None> Create() =>
            new ApiBuilder<None>(default(string)!);

        /// <summary> Builderパターンの引数部 【なし】：Apiの設定情報のみ 【コード指定】：ApiServer＋ApiClientの設定情報を取得 </summary>
        public static ApiBuilder<None> Code (this ApiBuilder<None> builder, string code = "") =>
            new ApiBuilder<None>(code);

        /// <summary> ビルド部（サーバーまたはクライアントのAccesorオブジェクト生成） *引数不要 </summary>
        public static IAccesors Build<TCode>(this ApiBuilder<TCode> builder) where TCode : IOpt =>
            builder.Code == "" ? new ServerAccesors() : new ClientAccesors(builder.Code);
    }
}
