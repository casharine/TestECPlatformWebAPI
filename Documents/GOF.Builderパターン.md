# ［GOF］Builderパターン

## はじめに
APIの各種Accessor群をAPI情報を取得するためのGOFのビルダーパターンを採用しました。

Builderパターンは、複雑なオブジェクトの生成過程をカプセル化し、その生成過程をステップごとに制御できるようにするデザインパターンです。このパターンは特に、同じ生成過程で異なる表現を作りたい場合や、オブジェクトの生成が多段階にわたる場合に有効です。

本プロジェクトでは、メソッドチェーンのような記法で、利用者がサイトコードだけ指定すれば、そのサイトの処理を行う上で必要な様々なアクセサ（データオブジェクトや接続情報）の複雑な取得処理を意識せず簡単に呼び出せるようにしました。

```_Accesors = ApiBuilder.Create().Code(iCode).Build();```

## クラス図
```
        +---------------+
        |    呼び出し部   |
        +---------------+
                |
                |     +----------------------+                  
                |     | IOpt                 |                   
                |     +----------------------+                  
                |     |  + Required()        |                 
                |     |  + None()            |                  
                |     +----------------------+                  
                |                ^            
                v                |
        +---------------------------+
        | ApiBuilder <TCode>        |<-\---------+
        +---------------------------+            |
                ^                                |
                |                                |
     +----------------------+                    |
     |  ApiBuilder(Static)  |                    |
     +----------------------+                    |
     |  + Create()          |                    |
     |  + Code()            |                    |
     |  + Build()           |                    |
     +----------------------+                    |
                ^                                |
                |                                |
        +-----------------+                      |
        |    IAccesors    |<---------------------+
        +-----------------+
                ^
                |    
    ※以下割愛Factoryパターンにて解説いたします
```


## 実装例
### Builderパターン呼び出し部
本ソリューションではControllerが呼び出された際のStartUp関数の中でまずサーバーのアクセサ（各種接続/データオブジェクトなど）を呼び出し、PostされてきたリクエストパラメータをDBに保存しています。

```
public class APIUtility
{
    private IAccesors _Accesors { get; set; } = null!;

    public IAccesors StartUpAPI<T>(ILogger<T> iLogger, object iBody, string userID, string prgID, string iCode = "")
    {
        iLogger.LogInformation(iBody.ToString());
        try
        {
            _Accesors = ApiBuilder.Create().Code(iCode).Build();
            _Accesors.getLoggerAPI().WriteLogAPI(userID, prgID, "", iBody.ToString()!);
        } catch
        {
            iLogger.LogInformation("ApiBuilder内でコンフィグ情報の生成に失敗しました");
        }
        return _Accesors;
    }
}
```

### ApiBuilder<TCode> クラス
このクラスはジェネリッククラスで、以下でTCode という型パラメーターを取ります。

```
public class ApiBuilder<TCode> where TCode : IOpt
{
    internal readonly string Code = "";

    internal ApiBuilder(string code) => (Code) = (code);
}
```

### IOpt クラス
TCode は IOpt インターフェースを実装しています。これはオプショナル引数を定義するためで、APIビルダーパターン内での引数の指定を柔軟に行えるようになります。具体的には引数がある場合(client用アクセサ)、ない場合(APIサーバー用アクセサ)に対応できるようにしてます。

```
namespace WebAPIBasis.ApiBuilder
{
    public interface IOpt { }

    public abstract class None : IOpt { }

    public abstract class Required : IOpt { }
}
```

### ApiBuilder Static クラス
クラスは、上述のApiBuilder<TCode> クラスのインスタンスを作成するためのファクトリーメソッドを提供しています。

- public static ApiBuilder<None> Create(): ApiBuilder<None> のインスタンスを作成するファクトリーメソッド。つまりBuilderObject自体の生成部です。
- public static ApiBuilder<None> Code(this ApiBuilder<None> builder, string code = ""): ApiBuilder<None> のインスタンスにコードを設定するメソッド。ここでは主に引数部分の設定をしています。
- public static IAccesors Build<TCode>(this ApiBuilder<TCode> builder) where TCode : IOpt: ApiBuilder<TCode> のインスタンスから IAccesors オブジェクトを生成するメソッド。ServerAccesors/ClientAccesors オブジェクトを生成します。

```
public static class ApiBuilder
{
    public static ApiBuilder<None> Create() =>
        new ApiBuilder<None>(default(string)!);

    public static ApiBuilder<None> Code (this ApiBuilder<None> builder, string code = "") =>
        new ApiBuilder<None>(code);

    public static IAccesors Build<TCode>(this ApiBuilder<TCode> builder) where TCode : IOpt =>
        builder.Code == "" ? new ServerAccesors() : new ClientAccesors(builder.Code);
}
```

## 参考文献
Java言語で学ぶデザインパターン入門第3版 Kindle版 結城 浩

