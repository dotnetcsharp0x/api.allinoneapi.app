using api.allinoneapi.app.Data;
using api.allinoneapi.app.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.allinoneapi.app.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MainController : ControllerBase, IDisposable
    {
        apiallinoneapiappContext _context;
        Crypto_Price _cp;
        public MainController(apiallinoneapiappContext context)
        {
            _context = context;
        }
        ~MainController()
        {

        }
        public void Dispose()
        {
            try { }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }
        #region GetCryptoPrices
        [HttpGet]
        [Route("GetCryptoPrices")]
        public async Task<List<Crypto_Price>> GetCryptoPrices()
        {
            await _context.Database.ExecuteSqlRawAsync("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;");
            return await (from i in  _context.Crypto_Price select i).AsNoTracking().ToListAsync();
        }
        #endregion
        #region GetCryptoPrice
        [HttpGet]
        [Route("GetCryptoPrice")]
        public async Task<Crypto_Price> GetCryptoPrice(string symbol)
        {
            await _context.Database.ExecuteSqlRawAsync("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;");
            return await (from i in _context.Crypto_Price where i.Symbol == symbol select i).AsNoTracking().FirstAsync();
        }
        #endregion
        #region GetCryptoSymbols
        [HttpGet]
        [Route("GetCryptoSymbols")]
        public async Task<List<Crypto_Symbols>> GetCryptoSymbols()
        {
            await _context.Database.ExecuteSqlRawAsync("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;");
            return await (from i in _context.Crypto_Symbols select i).AsNoTracking().ToListAsync();
        }
        #endregion
    }
    
}