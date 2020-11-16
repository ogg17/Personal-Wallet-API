using System;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using PersonalWallet.Models;

namespace PersonalWallet.Utilities {
    public static class WalletsTransfer {
        // Transfer from one wallet to another with receiving
        // the exchange rate from the European Central Bank API
        public static void Transfer(Wallet walletFrom, Wallet walletTo, double value) {
            if (walletFrom.Type == walletTo.Type) return;
            if (walletFrom.Value < value) throw new Exception("WalletFrom: The wallet cannot be less than zero!");
            
            XDocument xDoc = XDocument.Load("https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml");
            string xmlns = "http://www.ecb.int/vocabulary/2002-08-01/eurofxref";

            if (xDoc.Root == null) {
                throw new Exception("Error connecting to " +
                                    "'https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml'");
            }

            var cubes = xDoc.Root.Element(XName.Get("Cube", xmlns))?.Element(XName.Get("Cube", xmlns));
            IFormatProvider formatter = new NumberFormatInfo {NumberDecimalSeparator = "."};
            double eur;

            if (walletFrom.Type == WalletType.Eur) eur = value;
            else
                eur = value / Double.Parse(cubes.Elements().FirstOrDefault(element =>
                    String.Equals(element.Attribute("currency")?.Value, walletFrom.Type.ToString(),
                        StringComparison.CurrentCultureIgnoreCase)).Attribute("rate").Value, formatter);

            if (walletTo.Type == WalletType.Eur) walletTo.Change(eur);
            else
                walletTo.Change(eur * Double.Parse(cubes.Elements().FirstOrDefault(element =>
                    String.Equals(element.Attribute("currency")?.Value, walletTo.Type.ToString(),
                        StringComparison.CurrentCultureIgnoreCase)).Attribute("rate").Value, formatter));

            walletFrom.Change(-value);
        }
    }
}
