using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using PersonalWallet.Models;
using System.Threading.Tasks;
using PersonalWallet.Utilities;

namespace PersonalWallet.Controllers {
    public enum OperationType { TopUp, Withdraw, Transfer }
    
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase {
        private readonly UsersContext dataBase;
        
        public UsersController(UsersContext context) {
            dataBase = context;
            if (!dataBase.Users.Any()) {
                User defaultUser = new User{Name = "Default"};

                var wallets = new List<Wallet> {
                    new Wallet {Type = WalletType.Rub, Value = 100},
                    new Wallet {Type = WalletType.Usd, Value = 10},
                    new Wallet {Type = WalletType.Eur, Value = 10}
                };

                defaultUser.Wallets = wallets;
                
                dataBase.Users.Add(defaultUser);
                dataBase.Wallets.AddRange(wallets);

                dataBase.SaveChanges();
            }
        }
        
        //
        // GET api/users
        //
        [HttpGet]
        public async Task<ActionResult<List<UserDto>>> Get() {
            List<UserDto> result = new List<UserDto>();
            foreach(var user in dataBase.Users) {
                result.Add(new UserDto{UserId = user.UserId, Name = user.Name, Wallets = 
                    await dataBase.Wallets.Where(wallet => wallet.UserId == user.UserId).Select(wallet => 
                        new WalletDto { Type = wallet.Type.ToString(), Value = wallet.Value}).ToListAsync()});
            }
            return Ok(result);
        }
        
        //
        // GET api/users/[id]
        //
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> Get(int id) {
            User user = dataBase.Users.FirstOrDefault(x => x.UserId == id);

            if (user == null)
                return NotFound("Error: This user does not exist!");
            
            return Ok(new UserDto{UserId = user.UserId, Name = user.Name, Wallets = await dataBase.Wallets.Where(
                wallet => wallet.UserId == id).Select(wallet =>
                new WalletDto {Type = wallet.Type.ToString(), Value = wallet.Value}).ToListAsync()});
        }
        
        //
        // POST api/users
        // body: UserDto user
        //
        [HttpPost]
        public async Task<ActionResult> Post(UserDto user) {
            if (user == null)
                return BadRequest("Error: User is null!");

            var wallets = GetWalletsFromUserDto(user);
            
            await dataBase.Users.AddAsync(new User{Name = user.Name, Wallets = wallets});
            await dataBase.Wallets.AddRangeAsync(wallets);
            
            await dataBase.SaveChangesAsync();
            return Ok();
        }
        
        //
        // PUT api/users/[id]
        // body: UserDto user
        //
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(UserDto user, int id) {
            if (user == null)
                return BadRequest("Error: User is null!");
            
            if (!dataBase.Users.Any(u => u.UserId == id))
                return NotFound("Error: This user does not exist!");

            try {
                var wallets = GetWalletsFromUserDto(user);
                var userTmp = await dataBase.Users.FirstOrDefaultAsync(u => u.UserId == id);
                var walletsTmp = dataBase.Wallets.Where(w => w.UserId == id);
                
                userTmp.Name = user.Name;
                dataBase.Wallets.AttachRange(walletsTmp);
                dataBase.Wallets.RemoveRange(walletsTmp);
                
                userTmp.Wallets = wallets;
                dataBase.Users.Update(userTmp);
                await dataBase.SaveChangesAsync();
            
                return Ok();
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
        
        //
        // PUT api/users/[id]/[operation]?walletFrom=[wallet from]&walletTo=[wallet to]&value=[value]
        //
        [HttpPut("{id}/{operation}")]
        public async Task<ActionResult<UserDto>> Put(int id, string operation,
            string walletFrom, double value, string walletTo = null){
            
            User user = await dataBase.Users.FirstOrDefaultAsync(u => u.UserId == id);
            var wallets = dataBase.Wallets.Where(w => w.UserId == id);
            
            if (user == null)
                return NotFound("Error: This user does not exist!");

            if (!Enum.TryParse(walletFrom, true, out WalletType walletFromType)) 
                return BadRequest("WalletFrom: This wallet type does not exist!");
            if (!Enum.TryParse(operation, true, out OperationType operationType))
                return BadRequest("Error: This operation does not exist!");
            
            var walletFromTmp = await wallets.FirstOrDefaultAsync(w => w.Type == walletFromType);
            if(walletFromTmp == null) return BadRequest("WalletFrom: This wallet does not exist for the user!");
            
            switch (operationType) {
                case OperationType.TopUp:
                    walletFromTmp.Change(value);
                    break;
                
                case OperationType.Withdraw:
                    try { walletFromTmp.Change(-value); }
                    catch (Exception ex) { return BadRequest(ex.Message); }
                    break;
                
                case OperationType.Transfer:
                    if (!Enum.TryParse(walletTo, true, out WalletType walletToType)) 
                        return BadRequest("WalletTo: This wallet type does not exist!");
                    
                    try {
                        var walletToTmp = await wallets.FirstOrDefaultAsync(w => w.Type == walletToType);
                        if(walletToTmp == null) 
                            return BadRequest("WalletTo: This wallet does not exist for the user!");
                        
                        WalletsTransfer.Transfer(walletFromTmp, walletToTmp , value);
                    }
                    catch (Exception ex) { return BadRequest(ex.Message); }
                    break;
                
                default:
                    return BadRequest("Error: This operation does not exist!");
            }
            
            dataBase.Wallets.UpdateRange(wallets);
            await dataBase.SaveChangesAsync();

            return Ok(new UserDto{UserId = user.UserId, Name = user.Name, Wallets = 
                await dataBase.Wallets.Where(wallet => wallet.UserId == id).Select(wallet => 
                    new WalletDto {Type = wallet.Type.ToString(), Value = wallet.Value}).ToListAsync()});
        }
        
        //
        // DELETE api/users/[id]
        //
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id) {
            User user = dataBase.Users.FirstOrDefault(x => x.UserId == id);
            
            if (user == null)
                return NotFound("Error: This user does not exist!");
            
            dataBase.Users.Attach(user);
            dataBase.Users.Remove(user);
            
            await dataBase.SaveChangesAsync();
            
            return Ok();
        }

        private List<Wallet> GetWalletsFromUserDto(UserDto user) {
            var wallets = new List<Wallet>();
            foreach (var wallet in user.Wallets) {
                if (!Enum.TryParse(wallet.Type, true, out WalletType walletType)) 
                    throw new Exception("Error: This wallet type does not exist!");
                wallets.Add(new Wallet{Type = walletType, Value = wallet.Value});
            }
            return wallets;
        }
    }
}