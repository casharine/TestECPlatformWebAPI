# ［GOF］Factoryパターン - AbstractFactoryパターン&FactoryMethodパターン -

## はじめに

### AbstractFactoryパターン について
GOFの中でも特に初めて設計と実装に挑戦した時は難しいと感じました。少し長めになりますが、非常に便利です。実際あらゆる有名なフレームワークを構築する上で、複雑な生成部のロジックをクライアント（呼出し元）側から分離するためによく利用されています。

このパターンは、関連するオブジェクト群を一貫性を持って生成する事ができます。今回で言うとServerAccesors(APIサーバーを利用する上で共通の各種データやオブジェクトへのアクセサー群) と ClientAccesors（クライアント側ECサイトの毎に異なる各種データやオブジェクトへのアクセサー群）です。

既存のクライアントコード(呼出元)に影響を与えずに拡張可能できたりテストが容易だったりと良い事ずくめですが、必要以上に複雑になるので小規模プロジェクトには不向きだったり、抽象クラスの制限上具象クラスの柔軟なカスタマイズに支障がでる場合があるので注意が必要です。

#### Abstract Factoryパターンの要素
以下の要素が挙げられます。必ずしもAbstractクラスを使用しなければならないわけではなくInterfaceであっても問題ないので今回はC#のため抽象側はInterfaceで統一しています。

- 抽象ファクトリ (Abstract Factory): 関連する一連のオブジェクトを生成するためのインターフェースを定義します。
- 具体ファクトリ (Concrete Factory): 抽象ファクトリインターフェースを実装し、具体的なオブジェクトを生成します。
- 抽象プロダクト (Abstract Product): 各製品のためのインターフェースを宣言します。
- 具体プロダクト (Concrete Product): 抽象プロダクトのインターフェースを実装し、具体的な製品を生成します。

### FactoryMethodパターンについて
FactoryメソッドはFactoryパターンの一部を指し、特徴として特にメソッドレベルでオブジェクトの生成を抽象化します。具体的には、オブジェクトの生成方法をサブクラスで定義し、基底クラスでは抽象化することで、生成方法の多様性(ポリモーフィズム)を持たせる事ができます。今回で言うとcreateIAccesorsメソッドが本パターンを指します。


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
| - _site: string                               |   | - _site: string                               |
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
抽象ファクトリクラスです。続くxxxAccesorsFactoryにより実装されます。
```
public interface IAccessorsFactory
{
    IAccessors CreateAccessors(string siteCode);
}
```
### xxxAccesorsFactory インターフェース: 具象ファクトリクラス
次に、抽象ファクトリを実装するためにServerAccesorsFactory(APIサーバーを利用する上で共通のアクセサー) と ClientAccesorsFactory（クライアント側ECサイトの毎に異なるアクセサー）を定義していきます。本クラスの CreateAccessors メソッドは、Factory Methodパターンでそれぞれのクラスが異なる種類の IAccesors インスタンスを生成するために使用しています。これにより、具体的なオブジェクトのインスタンスを生成する責任がサブクラスに移譲され、呼出し元を具体的なオブジェクトの詳細から分離す事ができます。
#### ServerAccessorsFactory
```
public class ServerAccessorsFactory : IAccessorsFactory
{
    public IAccessors CreateAccessors(string siteCode)
    {
        return new ServerAccessors(siteCode);
    }
}
```
#### ClientAccessorsFactory
```
public class ClientAccessorsFactory : IAccessorsFactory
{
    public IAccessors CreateAccessors(string siteCode)
    {
        return new ClientAccessors(siteCode);
    }
}
```

### IAccesors インターフェース：抽象プロダクトクラス
いよいよプロダクトの生成ですが、ここでもまずプロダクトの抽象クラスを呼び出し、具体的なオブジェクト群の生成を分離します。
```
public interface IAccesors
{
    public string GetCode();
    public string GetConnectionString();
    public IConfigurationRoot GetAppsettings();
    public OracleConnection GetConnection();
    public LoggerAPI GetLoggerAPI();
    public LoggerClient GetLoggerClient();
    public object GetLockObject();
}
```

### xxxAccesors ：具象製品クラス
最後に具象クラスを抽象クラスの制約に違反しない範囲で自由に実装していく事ができます。今回の例ではAPIサーバーのアクセサを使用して購入処理を行う事はないので購入時に使用するLockobjectは生成されませんが、SiteAccesor側ではロックオブジェクトを生成している等違いを持たせる事ができています。
#### ServerAccesors
```
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
```
#### ClientAccesors
```
public class ClientAccesors : IAccesors
{
#region Init(Field, Property, Constructor)
private string _SITE { get; } = "";
private string _connectionStrings { get; } = "";
private IConfigurationRoot _configurationRoot { get; } = null!;
private OracleConnection _oracleConnection { get; } = null!;
private LoggerAPI _loggerAPI { get; } = null!;
private LoggerClient _loggerClient { get; } = null!;
private static object _myLockObject { get; set; } = new object();
private static object _LockObjectFUGA { get; } = new object();
private static object _LockObjectPIYO { get; } = new object();
private static object _LockObjectHOGE { get; } = new object();

public ClientAccesors(string iSITE)
{
    var fnc = new ConfigUtils();
    _SITE = iSITE;
    _configurationRoot = fnc.BuildConfigurationRoot();
    _connectionStrings = fnc.GetConnectionString(iSITE)!;
    _oracleConnection = new DBUtils().CreateOracleConnection(_connectionStrings);
    _loggerAPI = new LoggerAPI("");
    _loggerClient = new LoggerClient(iSITE);
}
#endregion Init(field, property, constructor)

#region Getterメソッド群
public string getCode() { return _SITE; }
public string getConnectionString() { return _connectionStrings; }
public IConfigurationRoot getAppsettings() { return _configurationRoot; }
public OracleConnection getConnection() { return _oracleConnection; }
public LoggerAPI getLoggerAPI() { return _loggerAPI; }
public LoggerClient getLoggerClient() { return _loggerClient; }
#endregion Getterメソッド群

#region その他のメソッド群
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
