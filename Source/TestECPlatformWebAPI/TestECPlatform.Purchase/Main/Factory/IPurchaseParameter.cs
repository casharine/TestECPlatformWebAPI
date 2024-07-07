using TestECPlatform.Purchase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPIBasis.Accesors;

namespace TestECPlatform.Purchase.Main.Factory
{
    interface IPurchaseParameter
    {
        public PurchaseParameter getParameter(PurchaseParameter purchaseParameter, IAccesors accesors);
    }
}
