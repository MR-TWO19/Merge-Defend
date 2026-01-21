using System;
using System.Collections.Generic;
using UnityEngine;

namespace TwoCore
{
    public class GameDefinitionAsset : SingletonScriptObject<GameDefinitionAsset>
    {
        public List<CurrencyDefinition> currencies = new List<CurrencyDefinition>();
        public List<CurrencyExchange> currencyExchanges = new List<CurrencyExchange>();
        public List<string> categories = new List<string>();

        /// <summary>
        /// Invoke when asset loaded
        /// </summary>
        void Init()
        {
            if (currencies.Count == 0)
            {
                currencies.Add(new CurrencyDefinition("Cash") { type = CurrencyType.Hard, numericType = NumericType.Number });
                currencies.Add(new CurrencyDefinition("Coin") { type = CurrencyType.Soft, numericType = NumericType.Number });
            }

        }

        public double GetExchangeValue(int srcCurrency, int destCurrency, double valueGet)
        {
            var exchangeConfig = currencyExchanges.Find(exchange => exchange.srcCurrency == srcCurrency && exchange.destCurrency == destCurrency);
            if (exchangeConfig != null) return Mathf.CeilToInt(((float)(exchangeConfig.srcValue * valueGet)) / exchangeConfig.destValue);
            return valueGet;
        }

        public CurrencyDefinition FindCurrency(int id) => currencies.Find(_ => _.id == id);
        public CurrencyDefinition FindCurrencyByName(string name) => currencies.Find(_ => _.name == name);
        public List<CurrencyDefinition> FindCurrenciesByType(CurrencyType type) => currencies.FindAll(_ => _.type == type);
    }
}