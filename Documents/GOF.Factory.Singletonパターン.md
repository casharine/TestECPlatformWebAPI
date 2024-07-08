# ［GOF］Factoryパターン - Singletonパターン -

## はじめに
購入処理時のロックオブジェクトの生成・取得にはSingletonFactoryパターンを採用しています。本パターンもGOFのFactoryパターンの一種で、カプセル化を行いながらアプリケーション全体で唯一のインスタンスを担保していきます。これにより複数のスレッドから同時にアクセスされても、正しく動作するようにスレッドセーフに実装することが可能で、競合状態や同期の問題を回避できます。

また、最終的にはサイト毎にロックオブジェクトをシングルトンにしたいので動的にロックオブジェクトを生成・取得するようにアレンジしています。

## クラス図
```
--------------------------------------------------
|              LockObjectsSingleton              |
--------------------------------------------------
| - _lock: object                                |
| - _instance: LockObjectsSingleton              |
| - _LockObjectHOGE: object                      |
| - _LockObjectFUGA: object                      |
| - _LockObjectPIYO: object                      |
--------------------------------------------------
| + GetInstance(): LockObjectsSingleton          |
| + GetLockObject(string code): object           |
--------------------------------------------------
```

## 実装例
### フィールド及びコンストラクタ
API全体で一つのインスタンスを共有するようデザインしながら以下の通りコーディングしていきます。

- _lock：インスタンスの作成を排他的に制御するためのロックオブジェクトです。
- _instance：クラスの唯一のインスタンスを担保しこのインスタンスからロックオブジェクトを取得していきます。
- コンストラクタ：カプセル化し外部からのインスタンス化を防ぎながらサイト毎のロックオブジェクトを生成していきます。

```
private static readonly object _lock = new object();
private static LockObjectsSingleton _instance;
private object _LockObjectHOGE;
private object _LockObjectFUGA;
private object _LockObjectPIYO;

private LockObjectsSingleton()
{
    _LockObjectHOGE = new object();
    _LockObjectFUGA = new object();
    _LockObjectPIYO = new object();
}
```

### GetInstance() メソッド:クラスの唯一のインスタンスを生成
クラスの唯一のインスタンスを返します。初めて呼ばれたときにのみインスタンスが作成され、lock (_lock) により、複数のスレッドが同時に _instance を作成しようとしても、排他的に制御する事ができます。また、public staticにより、どこからでも簡単にインスタンスにアクセスできるようにしています。
```
public static LockObjectsSingleton GetInstance()
{
    if (_instance == null)
    {
        lock (_lock)
        {
            if (_instance == null)
            {
                _instance = new LockObjectsSingleton();
            }
        }
    }
    return _instance;
}
```
### GetLockObject(string code) メソッド:
指定されたサイトのコードに基づいて対応するロックオブジェクトを返します。
```
public object GetLockObject(string code)
{
    switch (code)
    {
        case "1001":
            return _LockObjectHOGE;
        case "1002":
            return _LockObjectFUGA;
        case "1003":
            return _LockObjectPIYO;
        default:
            throw new ArgumentOutOfRangeException();
    }
}

```