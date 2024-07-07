using Microsoft.Extensions.Configuration;

namespace WebAPIBasis
{
    //Todo Staticプロパティ辞める

    /// <summary>
    /// 旧WebConfig代用、コンフィグ値用JSONファイルとPGのマッピングを行います
    /// </summary>
    /// <author>Y.Ito 2022年10月25日</author>
    public class AppSettings
    {
        public static IConfiguration Configuration { get; set; } = null!;

        /// <summary>
        /// UseSecret
        /// </summary>
        public string Secret { get; set; } = "";

        /// <summary>
        /// コネクションストリング
        /// </summary>
        public static string? ConnectionString { get; set; } = "";
    }
}
