# ［GOF］Factoryパターン - Factoryメソッド -

## はじめに
本プロジェクトでは、少し複雑になりますがFactoryパターンを入れ子構造に展開し、クライアントサイトが自社サイトの購入処理を行うか他社サイトの購入処理を行うかによって適切なオブジェクト群を一括生成できるようにプログラムしました。

- Factoryパターン
オブジェクト生成を抽象化する方法全般を指します。オブジェクトの生成をカプセル化するデザインパターンの一種で、具体的な生成方法をクライアントコードから分離する事ができます。
(internalはJavaでいうpackage-privateと同義です)

- FactoryMethodパターン
FactoryメソッドはFactoryパターンの一部を指し、特徴として特にメソッドレベルでオブジェクトの生成を抽象化します。具体的には、オブジェクトの生成方法をサブクラスで定義し、基底クラスでは抽象化することで、生成方法の多様性を持たせます。いわゆるポリモーフィズムです。

- その他のFactoryパターン
本ソリューションでは、Factoryメソッド以外にもSimpleFactoryパターンも利用していますが、他にもFactoryパターンにはオブジェクトの複製に特化したPrototypeパターンや一意性を担保することでグローバルアクセスポイントとするSingletonパターン等があります。
Prototypeについては実装中ParchaseResultを適用したいと思います。

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
### Builderパターン呼び出し部

    public class PurchaseLinkController : ControllerBase{
 public IActionResult PostPurchaseLinkData(PostPurchaseLinkRequestBase body){
    中略
                IPurchase Purchase = new SelfPurchaseFactory().createPurchase(param);
                PurchaseResult resPurchaseLink = Purchase.purchase();
    中略}
    }

## AbstractFactory
### IAccesorsFactory インターフェース
IAccesorsFactory インターフェースが Create メソッドを定義しています。これは具象ファクトリークラスによって実装され、具体的な IAccesors のインスタンスを生成します。
ServerAccesorsFactory と ClientAccesorsFactory クラスが IAccesorsFactory インターフェースを実装し、それぞれの Create メソッドが対応する具体的な IAccesors インスタンスを生成します。
IAccesors インターフェースが、ServerAccesors と ClientAccesors クラスで実装されており、各クラスが異なる方法でアクセスオブジェクトを提供します。
LoggerAPI と LoggerClient クラスが、それぞれのアクセサクラスで使用され、サイトコードに基づいてインスタンス化されます。


IAccesorsFactory インターフェース:

IAccesorsFactory インターフェースの Create メソッドが Factory Method パターンの実装です。このメソッドは、具体的な製品のインスタンス化の詳細をサブクラスに委譲します。
具象ファクトリークラス (ServerAccesorsFactory と ClientAccesorsFactory):

ServerAccesorsFactory と ClientAccesorsFactory クラスの Create メソッドは、それぞれのクラスが異なる種類の IAccesors インスタンスを生成するために使用されています。これにより、具体的な製品のインスタンスを生成する責任がサブクラスに移譲され、クライアントコードは具体的な製品の詳細から分離されます。
説明のポイント：
Abstract Factory パターンは、関連する一連の製品を生成するためのインターフェースを提供し、具体的なファクトリーがその実装を担当します。
Factory Method パターンは、具体的なファクトリーのサブクラスが生成する具体的な製品のインスタンス化を処理する方法を定義します。
このように、Abstract Factory パターンは関連する製品ファミリーを生成するために使用され、Factory Method パターンは個々の製品を生成する方法を制御するために使用されています。

```
public interface IAccesorsFactory
{
    IAccesors Create(string siteCode);
}

// Concrete Factory for Server
public class ServerAccesorsFactory : IAccesorsFactory
{
    public IAccesors Create(string siteCode)
    {
        return new ServerAccesors(siteCode);
    }
}

// Concrete Factory for Client
public class ClientAccesorsFactory : IAccesorsFactory
{
    public IAccesors Create(string siteCode)
    {
        return new ClientAccesors(siteCode);
    }
}
```

### IAccesors インターフェース
```
public interface IAccesors
{
    string GetCode();
    string GetConnectionString();
    IConfigurationRoot GetAppsettings();
    OracleConnection GetConnection();
    LoggerAPI GetLoggerAPI();
    LoggerClient GetLoggerClient();
    object GetLockObject();
}
```

### Concrete Products (ServerAccesors と ClientAccesors)

#### ServerAccesors
```
public class ServerAccesors : IAccesors
{
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

    public string GetCode() { return _site; }
    public string GetConnectionString() { return _connectionString; }
    public IConfigurationRoot GetAppsettings() { return _configurationRoot; }
    public OracleConnection GetConnection() { return _oracleConnection; }
    public LoggerAPI GetLoggerAPI() { return _loggerAPI; }
    public LoggerClient GetLoggerClient() { return null; } 
    public object GetLockObject() { return null; } 
}
```
#### ClientAccesors
```
public class ClientAccesors : IAccesors
{
    private string _site;
    private string _connectionString;
    private IConfigurationRoot _configurationRoot;
    private OracleConnection _oracleConnection;
    private LoggerAPI _loggerAPI;
    private LoggerClient _loggerClient;
    private object _lockObject;

    public ClientAccesors(string siteCode)
    {
        _site = siteCode;
        var fnc = new ConfigUtils();
        _configurationRoot = fnc.BuildConfigurationRoot();
        _connectionString = fnc.GetConnectionString(siteCode);
        _oracleConnection = new DBUtils().CreateOracleConnection(_connectionString);
        _loggerAPI = new LoggerAPI("");
        _loggerClient = new LoggerClient(siteCode);
        _lockObject = DetermineLockObject();
    }

    private object DetermineLockObject()
    {
        switch (_site)
        {
            case "2101": return new object(); // _LockObjectHOGE
            case "3501": return new object(); // _LockObjectFUGA
            case "3502": return new object(); // _LockObjectPIYO
            default: throw new ArgumentOutOfRangeException();
        }
    }

    public string GetCode() { return _site; }
    public string GetConnectionString() { return _connectionString; }
    public IConfigurationRoot GetAppsettings() { return _configurationRoot; }
    public OracleConnection GetConnection() { return _oracleConnection; }
    public LoggerAPI GetLoggerAPI() { return _loggerAPI; }
    public LoggerClient GetLoggerClient() { return _loggerClient; }
    public object GetLockObject() { return _lockObject; }
}
```

## 参考文献
- Java言語で学ぶデザインパターン入門第3版 Kindle版 結城 浩
- https://qiita.com/shoheiyokoyama/items/d752834a6a2e208b90ca#factory_pattern
