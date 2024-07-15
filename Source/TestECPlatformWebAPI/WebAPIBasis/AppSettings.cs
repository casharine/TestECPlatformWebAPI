using Microsoft.Extensions.Configuration;

namespace WebAPIBasis
{
    /// <summary>
    /// 旧WebConfig代用、コンフィグ値用JSONファイルとPGのマッピングを行います
    /// </summary>
    /// <author>Y.Ito</author>
    public class AppSettings
    {
        public string Secret { get; set; } = "";
        public static string ConnectionString { get; set;} = "";

        // 入れ子の配列として定義
        public static LinkToFugaSettings[]? LinkToFuga { get; set; }
        // 入れ子内部の定義
        public class LinkToFugaSettings
        {
            public static string? UserId { get; set; }
            public static string? Password { get; set; }
        }
        // 入れ子の配列として定義
        public static LinkToFugaSettings[]? LinkToPiyo { get; set; }
        // 入れ子内部の定義
        public class LinkToPiyoSettings
        {
            public static string? UserId { get; set; }
            public static string? Password { get; set; }
        }
    }
}
