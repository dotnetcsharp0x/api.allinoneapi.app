using api.allinoneapi.app.Data;
using api.allinoneapi.app.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.allinoneapi.app.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CryptoController : ControllerBase, IDisposable
    {
        public CryptoController() {
        }
        #region Dispose
        ~CryptoController()
        {
        }
        public void Dispose()
        {
            try { }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.SuppressFinalize(this);
            }
        }
        #endregion

        #region GetCryptoPrices
        [HttpGet]
        [Route("GetCryptoPrices")]
        public async Task<List<Crypto_Price>> GetCryptoPrices()
        {
            var CryptoPriceList = new List<Crypto_Price>();
            using (apiallinoneapiappContext _context = new apiallinoneapiappContext())
            {
                await _context.Database.ExecuteSqlRawAsync("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;");
                CryptoPriceList = await (from i in _context.Crypto_Price select i).AsNoTracking().ToListAsync();
            }
            return CryptoPriceList;
        }
        #endregion

        #region GetCryptoPrice
        [HttpGet]
        [Route("GetCryptoPrice")]
        public async Task<Crypto_Price> GetCryptoPrice(string symbol)
        {
            var CryptoPice = new Crypto_Price();
            using (apiallinoneapiappContext _context = new apiallinoneapiappContext())
            {
                await _context.Database.ExecuteSqlRawAsync("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;");
                CryptoPice = await (from i in _context.Crypto_Price where i.Symbol == symbol select i).AsNoTracking().FirstAsync();
            }
            return CryptoPice;
        }
        #endregion

        #region GetCryptoSymbols
        [HttpGet]
        [Route("GetCryptoSymbols")]
        public async Task<List<Crypto_Symbols>> GetCryptoSymbols()
        {
            var CryptoSymbols = new List<Crypto_Symbols>();
            using (apiallinoneapiappContext _context = new apiallinoneapiappContext())
            {
                await _context.Database.ExecuteSqlRawAsync("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;");
                CryptoSymbols = await(from i in _context.Crypto_Symbols select i).AsNoTracking().ToListAsync();
            }
            return CryptoSymbols;
        }
        #endregion
    }
    
}