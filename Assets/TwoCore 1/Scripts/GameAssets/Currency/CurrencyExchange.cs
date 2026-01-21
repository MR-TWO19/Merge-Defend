using System;

namespace TwoCore
{
    [Serializable]
    public class CurrencyExchange
    {
        public int srcCurrency;
        public int destCurrency;
        public int srcValue = 1;
        public int destValue = 1;
    }
}