using System;

namespace PersonalWallet.Models {
    public enum WalletType { Rub = 1, Usd, Eur, Jpy, Bgn, Ron, Php, Zar }
    
    public class Wallet {
        public int WalletId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public WalletType Type { get; set; }
        public double Value { get; set; }

        public void Change(double value) {
            if (Value + value > 0) Value += value;
            else throw new Exception("The wallet cannot be less than zero!");
        }
    }
    
    public class WalletDto{
        public string Type { get; set; }
        public double Value { get; set; }
    }
}