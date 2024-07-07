using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPIBasis.ApiBuilder
{
    /// <summary>
    /// オプショナル引数を定義するためのインターフェイス
    /// </summary>
    public interface IOpt { }

    public abstract class None : IOpt { }

    public abstract class Required : IOpt { }
}
