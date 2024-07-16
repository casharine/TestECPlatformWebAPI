# ［GOF］Factoryパターン - AbstractFactoryパターン&FactoryMethodパターン -

## はじめに

### AbstractFactoryパターン について
GOFの中でも初めて設計と実装に挑戦した時は難しいと感じました。.NETのフレームワークにも複雑な生成部のロジック層をクライアント（呼出し元）側から分離するためによく利用されているのを見かけます。（FormのUIコントロールなど）

このパターンは、関連するオブジェクト群を一貫性を持って生成する事ができます。今回で言うとServerAccesors(APIサーバーを利用する上で共通の各種データやオブジェクトへのアクセサー群) と ClientAccesors（クライアント側ECサイトの毎に異なる各種データやオブジェクトへのアクセサー群）です。

既存のクライアントコード(呼出元)に影響を与えずに拡張可能できたりテストが容易だったりと良い事ずくめですが、小規模プロジェクトの場合は必要以上に複雑になるので不向きだったり、抽象クラスの制限上具象クラスの柔軟なカスタマイズに支障がでる場合があるので注意が必要です。

#### Abstract Factoryパターンの要素
構成要素として以下の要素が挙げられます。必ずしもAbstractクラスを使用しなければならないわけではなく抽象化するためInterfaceを使用しても考え方は同じため本パターンとなります。今回は抽象側はInterfaceで統一しています。

- 抽象ファクトリ (Abstract Factory): 関連する一連のオブジェクトを生成するためのインターフェースを定義します。
- 具体ファクトリ (Concrete Factory): 抽象ファクトリインターフェースを実装し、具体的なオブジェクトを生成します。
- 抽象プロダクト (Abstract Product): 各製品のためのインターフェースを宣言します。
- 具体プロダクト (Concrete Product): 抽象プロダクトのインターフェースを実装し、具体的な製品を生成します。

### FactoryMethodパターンについて
FactoryメソッドはFactoryパターンの一部を指し、特徴として特にメソッドレベルでオブジェクトの生成を抽象化する事を指します。具体的には、オブジェクトの生成方法をサブクラスで定義し、基底クラスでは抽象化することで、生成方法の多様性(ポリモーフィズム)を持たせる事ができます。今回で言うとcreateメソッドが本パターンを指します。


## クラス図
```
-------------------------------------------------
|                   IAccesorsFactory              |
-------------------------------------------------
| + Create(string siteCode): IAccesors            |
-------------------------------------------------
                           ^
                           |
          ---------------------------------------------------------------------
          |                                                                    |
-------------------------------------------------   -----------------------------------------------
|              ServerAccesorsFactory           |   |              ClientAccesorsFactory           |
-------------------------------------------------   -----------------------------------------------
| + Create(string): IAccesors                  |   | + Create(string): IAccesors                  |
-------------------------------------------------   -----------------------------------------------
           ^                                                                 ^
           |                                                                 |
           -------------------------------------------------------------------
                          |
-------------------------------------------------
|                    IAccesors                   |
-------------------------------------------------
| + GetCode(): string                            |
| + GetConnectionString(): string                |
| + GetAppsettings(): IConfigurationRoot         |
| + GetConnection(): OracleConnection            |
| + GetLoggerAPI(): LoggerAPI                    |
| + GetLoggerClient(): LoggerClient              |
| + GetLockObject(): object                      |
-------------------------------------------------
                           ^
                           |
        ----------------------------------------------------------------------
        |                                                                    |
-------------------------------------------------   -------------------------------------------------
|              ServerAccesors                   |   |              ClientAccesors                   |
-------------------------------------------------   -------------------------------------------------
| - _SITE_CD: string                               |   | - _SITE_CD: string                               |
| - _connectionString: string                   |   | - _connectionString: string                   |
| - _configurationRoot: IConfigurationRoot      |   | - _configurationRoot: IConfigurationRoot      |
| - _oracleConnection: OracleConnection         |   | - _oracleConnection: OracleConnection         |
| - _loggerAPI: LoggerAPI                       |   | - _loggerAPI: LoggerAPI                       |
|                                               |   | - _loggerClient: LoggerClient                 |
|                                               |   | - _lockObject: object                         |
-------------------------------------------------   -------------------------------------------------

※生成される独自クラス（IConfigurationRootなど.Net標準クラスは割愛しています）
--------------------------------      -----------------------------------
|    LoggerAPI                 |      |          LoggerClient           |
--------------------------------      -----------------------------------
| + LoggerAPI(string siteCode) |      | + LoggerClient(string siteCode) |
--------------------------------      -----------------------------------
```
## 実装例
Abstract Factory パターンは関連するオブジェクトファミリーを生成するために使用され、Factory Method パターンは個々のオブジェクトを生成する方法を制御するために使用されています。


### IAccesorsFactory インターフェース：抽象ファクトリクラス 
まずは、抽象ファクトリクラスです。ここでインターフェースを定義し、続くxxxAccesorsFactoryにより実装されます。
```
public interface IAccessorsFactory
{
    // 抽象ファクトリ（インターフェイス）なので側のみ
    IAccessors CreateAccessors(string siteCode);
}
```
### xxxAccesorsFactory インターフェース: 具象ファクトリクラス
次に、抽象ファクトリを実装するためにServerAccesorsFactory(APIサーバーを利用する上で共通のアクセサー) と ClientAccesorsFactory（クライアント側ECサイトの毎に異なるアクセサー）を定義していきます。本クラスの CreateAccessors メソッドは、Factory Methodパターンでそれぞれのクラスが異なる種類(共通かサイト毎か)の IAccesors インスタンスを生成するために使用しています。これにより、具体的なオブジェクトのインスタンスを生成する責任がサブクラスに移譲され、呼出し元を具体的なオブジェクトの詳細部から分離する事ができます。
#### ServerAccessorsFactory：APIサーバー利用時共通
```
public class ServerAccessorsFactory : IAccessorsFactory
{
    public IAccessors CreateAccessors(string siteCode)
    {
        // 具象ファクトリなのでここでインスタンス化
        return new ServerAccessors(siteCode);
    }
}
```
#### ClientAccessorsFactory：クライアントECサイト別
```
public class ClientAccessorsFactory : IAccessorsFactory
{
    public IAccessors CreateAccessors(string siteCode)
    {
        // 具象ファクトリなのでここでインスタンス化
        return new ClientAccessors(siteCode);
    }
}
```

### IAccesors インターフェース：抽象プロダクトクラス
いよいよプロダクトの生成ですが、ここでもまずプロダクトの抽象クラスを呼び出し、具体的なオブジェクト群の生成をこちらから分離していきます。
```
public interface IAccesors
{
    //抽象プロダクトなので最終的に具象化するメソッドを指示する
    public string GetCode();
    public string GetConnectionString();
    public IConfigurationRoot GetAppsettings();
    public OracleConnection GetConnection();
    public LoggerAPI GetLoggerAPI();
    public LoggerClient GetLoggerClient();
    public object GetLockObject();
}
```

### xxxAccesors ：具象プロダクトクラス
最後に具象クラスを抽象クラスの制約に違反しない範囲で自由に実装していく事ができます。今回の例ではAPIサーバーのアクセサを使用して購入処理を行う事はないので購入時に使用するLockobjectは生成されませんが、SiteAccesor側ではロックオブジェクトを生成している等違いを持たせる事ができています。
また、カプセル化されたGettterのみ定義し値はコンストラクタつまりファクトリからインスタンス化された時にしか設定できない設計となります。
#### ServerAccesors
```
public class ServerAccesors : IAccesors
{
#region Init(Field, Property, Constructor)
private string _SITE_CD;
private string _connectionString;
private IConfigurationRoot _configurationRoot;
private OracleConnection _oracleConnection;
private LoggerAPI _loggerAPI;

// コンストラクタでアクセサに必要な情報を格納に初期化して以後更新できないようにする
public ServerAccesors(string siteCode)
{
    // ECサイトの識別コード
    _SITE_CD = siteCode;
    // コンフィグ取得のためのオリジナルクラス
    var fnc = new ConfigUtils();
    // コンフィグ(.netcoreの場合Appsettings.json)を取得するためのオブジェクト
    _configurationRoot = fnc.BuildConfigurationRoot();
    // DB接続情報
    _connectionString = fnc.GetConnectionString("");
    // DBコネクション
    _oracleConnection = new DBUtils().CreateOracleConnection(_connectionString);
    // API用ロガー
    _loggerAPI = new LoggerAPI("");
}
#endregion Init(Field, Property, Constructor)

#region Getterメソッド群
// インターフェイスに定義されたメソッドを定義していくカプセル化しGetterのみ定義
public string getCode() { return _SITE_CD; }
public string getConnectionString() { return _connectionString; }
public IConfigurationRoot getAppsettings() { return _configurationRoot; }
public OracleConnection getConnection() { return _oracleConnection; }
public LoggerAPI getLoggerAPI() { return _loggerAPI; }
public LoggerClient getLoggerClient() { return null; }
public object getLockObject() { return null; }

#endregion Getterメソッド群
}
```
#### ClientAccesors
```
public class ClientAccesors : IAccesors
{
#region Init(Field, Property, Constructor)
private string _SITE_CD { get; } = "";
private string _connectionStrings { get; } = "";
private IConfigurationRoot _configurationRoot { get; } = null!;
private OracleConnection _oracleConnection { get; } = null!;
private LoggerAPI _loggerAPI { get; } = null!;
private LoggerClient _loggerClient { get; } = null!;
private static object _myLockObject { get; set; } = new object();
private static object _LockObjectFUGA { get; } = new object();
private static object _LockObjectPIYO { get; } = new object();
private static object _LockObjectHOGE { get; } = new object();

// コンストラクタでアクセサに必要な情報を格納に初期化して以後更新できないようにする
public ClientAccesors(string iSITE_CD)
{
    // コンフィグ取得のためのオリジナルクラス
    var fnc = new ConfigUtils();
    // サイトのコード（API共通は空）
    _SITE_CD = iSITE_CD;
    // コンフィグ(.netcoreの場合Appsettings.json)を取得するためのオブジェクト
    _configurationRoot = fnc.BuildConfigurationRoot();
    // DB接続文字列
    _connectionStrings = fnc.GetConnectionString(iSITE_CD)!;
    // DBコネクション
    _oracleConnection = new DBUtils().CreateOracleConnection(_connectionStrings);
    // APIDBへのロガー
    _loggerAPI = new LoggerAPI("");
    // クライアントサイトへのロガー
    _loggerClient = new LoggerClient(iSITE_CD);
}
#endregion Init(field, property, constructor)

#region Getterメソッド群
// インターフェイスに定義されたメソッドを定義していく（カプセル化）
public string getCode() { return _SITE_CD; }
public string getConnectionString() { return _connectionStrings; }
public IConfigurationRoot getAppsettings() { return _configurationRoot; }
public OracleConnection getConnection() { return _oracleConnection; }
public LoggerAPI getLoggerAPI() { return _loggerAPI; }
public LoggerClient getLoggerClient() { return _loggerClient; }
#endregion Getterメソッド群

#region その他のメソッド群
// サイト毎にロックオブジェクトを取得 ※内容はシングルトンパターンで別途い解説あり
public object getLockObject()
{
    object wLockObject;

    try
    {
        var target = this.getCode() switch
        {
            "1001" => wLockObject = LockObjectsSingleton.GetInstance().GetLockObject(this.getCode()),
            "1002" => wLockObject = LockObjectsSingleton.GetInstance().GetLockObject(this.getCode()),
            "1003" => wLockObject = LockObjectsSingleton.GetInstance().GetLockObject(this.getCode()),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    catch (Exception ex)
    {
        this.getLoggerClient().WriteLog("createSelectedPurchase", $"コード{this.getCode()}の購入クラス生成に失敗しました");
        throw;
    }
    return wLockObject;
}
#endregion その他のメソッド群
}
```

## 参考文献
- Java言語で学ぶデザインパターン入門第3版 Kindle版 結城 浩
- https://qiita.com/shoheiyokoyama/items/d752834a6a2e208b90ca#factory_pattern
