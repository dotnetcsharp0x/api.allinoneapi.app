using api.allinoneapi.app.Data;
using api.allinoneapi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Binance.Net.Clients;
using System.Globalization;

namespace api.allinoneapi.app.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CryptoController : ControllerBase, IDisposable
    {
        Crypto crypto = new Crypto();
        public CryptoController() {
        }
        #region Dispose
        ~CryptoController()
        {
        }
        public void Dispose()
        {
            try {
                crypto.Dispose();
            }
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
        [Route("GetPrices")]
        public async Task<Crypto_Price[]> GetCryptoPrices()
        {
            var CryptoPriceList = new Crypto_Price[1];
            await using (apiallinoneapiappContext _context = new apiallinoneapiappContext())
            {
                await _context.Database.ExecuteSqlRawAsync("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;");
                CryptoPriceList = await (from i in _context.Crypto_Price select i).AsNoTracking().ToArrayAsync();
            }
            return CryptoPriceList;
        }
        #endregion

        #region GetPrice
        [HttpGet]
        [Route("GetPrice")]
        public async Task<Crypto_Price> GetCryptoPrice(string symbol)
        {
            var CryptoPice = new Crypto_Price();
            await using (apiallinoneapiappContext _context = new apiallinoneapiappContext())
            {
                await _context.Database.ExecuteSqlRawAsync("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;");
                CryptoPice = await (from i in _context.Crypto_Price where i.Symbol == symbol select i).AsNoTracking().FirstAsync();
            }
            return CryptoPice;
        }
        #endregion

        #region GetKandles
        [HttpGet]
        [Route("GetKandles")]
        public HashSet<Binance_CryptoKandles> GetKandles(string? symbol = "BTCUSDT", int minutes = 60, int lines = 60, string interval = "5M")
        {
            try
            {
                int to_minus = -182;
                to_minus -= minutes;
                var resp = crypto.Binance_GetKandles(symbol, to_minus, lines, interval);
                if (resp != null)
                {
                    return resp;
                }
                else
                {
                    return new HashSet<Binance_CryptoKandles>() { };
                }
            }
            catch (Exception ex)
            {
                return new HashSet<Binance_CryptoKandles>() { };
            }
        }
        #endregion

        #region GetInstruments
        [HttpGet]
        [Route("GetInstruments")]
        public async Task<List<Crypto_Symbols>> GetInstruments()
        {
            var CryptoSymbols = new List<Crypto_Symbols>();
            await using (apiallinoneapiappContext _context = new apiallinoneapiappContext())
            {
                await _context.Database.ExecuteSqlRawAsync("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;");
                CryptoSymbols = await(from i in _context.Crypto_Symbols select i).AsNoTracking().ToListAsync();
            }
            return CryptoSymbols;
        }
        #endregion

        #region GetInstruments/Detail
        [HttpGet]
        [Route("GetInstruments/Detail")]
        public async Task<List<Crypto_Symbols>> GetInstrumentsDetail(string symbol)
        {
            var CryptoSymbols = new List<Crypto_Symbols>();
            await using (apiallinoneapiappContext _context = new apiallinoneapiappContext())
            {
                await _context.Database.ExecuteSqlRawAsync("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;");
                CryptoSymbols = await(from i in _context.Crypto_Symbols where i.Symbol == symbol select i).AsNoTracking().ToListAsync();
            }
            return CryptoSymbols;
        }
        #endregion
    }

}