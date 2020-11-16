using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PersonalWallet.Models {
    public class User {
        public int UserId { get; set; }
        public string Name { get; set; }
        public List<Wallet> Wallets { get; set; } = new List<Wallet>();
    }

    public class UserDto {
        public int UserId { get; set; }
        public string Name { get; set; }
        public List<WalletDto> Wallets { get; set; } = new List<WalletDto>();
    }
}