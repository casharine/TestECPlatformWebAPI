using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPIBasis.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    public class LockObjectsSingleton
    {
        private static readonly object _lock = new object();
        private static LockObjectsSingleton _instance;

        private object _LockObjectHOGE;
        private object _LockObjectFUGA;
        private object _LockObjectPIYO;

        // シングルトンとして各ロックオブジェクトを初期化します
        private LockObjectsSingleton()
        {
            _LockObjectHOGE = new object();
            _LockObjectFUGA = new object();
            _LockObjectPIYO = new object();
        }

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

        public object GetLockObject(string code)
        {
            switch (code)
            {
                case "1001":
                    return _instance._LockObjectHOGE;
                case "1002":
                    return _instance._LockObjectFUGA;
                case "1003":
                    return _instance._LockObjectPIYO;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
