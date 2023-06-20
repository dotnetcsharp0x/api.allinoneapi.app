using api.allinoneapi.app.Data;
using api.allinoneapi.Models;
using api.allinoneapi.Models.Stocks.Polygon;
using api.allinoneapi.Models.Stocks.Polygon.Actions;
using Azure.Core.GeoJson;
using CryptoExchange.Net.CommonObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Nancy.Json;
using RestSharp;
using System.Linq.Expressions;
using System.Text;

namespace api.allinoneapi.app.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StockController : Controller, IDisposable
    {

        #region Dispose
        ~StockController()
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

        #region GetInstruments
        [HttpGet]
        [Route("GetInstruments")]
        public async Task<List<StockInstruments>> GetInstruments()
        {
            var CryptoSymbols = new List<StockInstruments>();
            await using (apiallinoneapiappContext _context = new apiallinoneapiappContext())
            {
                await _context.Database.ExecuteSqlRawAsync("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;");
                CryptoSymbols = await (from i in _context.StockInstruments where i.market == "stocks" select i).AsNoTracking().ToListAsync();
            }
            return CryptoSymbols;
        }
        #endregion

        #region GetInstrumentsDetail
        [HttpGet]
        [Route("GetInstruments/Detail")]
        public async Task<List<StockDescription>> GetInstrumentsDetail(string? ticker = "AAPL")
        {
            var CryptoSymbols = new List<StockDescription>();
            await using (apiallinoneapiappContext _context = new apiallinoneapiappContext())
            {
                await _context.Database.ExecuteSqlRawAsync("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;");
                CryptoSymbols = await (from i in _context.StockDescription
                                       where i.market == "stocks" && i.ticker == ticker
                                       select new StockDescription()
                                       {
                                           ticker = i.ticker
                    ,
                                           name = i.name
                    ,
                                           market = i.market
                    ,
                                           type = i.type
                    ,
                                           currency_name = i.currency_name
                    ,
                                           composite_figi = i.composite_figi
                    ,
                                           share_class_figi = i.share_class_figi
                    ,
                                           locale = i.locale
                    ,
                                           primary_exchange = i.primary_exchange
                    ,
                                           cik = i.cik
                    ,
                                           phone_number = i.phone_number
                    ,
                                           address1 = i.address1
                    ,
                                           city = i.city
                    ,
                                           state = i.state
                    ,
                                           postal_code = i.postal_code
                    ,
                                           description = i.description
                    ,
                                           sic_code = i.sic_code
                    ,
                                           sic_description = i.sic_description
                    ,
                                           homepage_url = i.homepage_url
                    ,
                                           total_employees = i.total_employees
                    ,
                                           list_date = i.list_date
                    ,
                                           share_class_shares_outstanding = i.share_class_shares_outstanding
                    ,
                                           weighted_shares_outstanding = i.weighted_shares_outstanding
                    ,
                                           round_lot = i.round_lot
                    ,
                                           update_date = i.update_date
                                       }).AsNoTracking().ToListAsync();
            }
            return CryptoSymbols;
        }
        #endregion

        #region GetEtfs
        [HttpGet]
        [Route("GetEtfs")]
        public async Task<List<StockInstruments>> GetEtfs()
        {
            var CryptoSymbols = new List<StockInstruments>();
            await using (apiallinoneapiappContext _context = new apiallinoneapiappContext())
            {
                await _context.Database.ExecuteSqlRawAsync("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;");
                CryptoSymbols = await (from i in _context.StockInstruments where i.market == "stocks" && i.type.ToUpper() == "ETF" select i).AsNoTracking().ToListAsync();
            }
            return CryptoSymbols;
        }
        #endregion

        #region GetBonds
        [HttpGet]
        [Route("GetBonds")]
        public async Task<List<StockInstruments>> GetBonds()
        {
            var CryptoSymbols = new List<StockInstruments>();
            await using (apiallinoneapiappContext _context = new apiallinoneapiappContext())
            {
                await _context.Database.ExecuteSqlRawAsync("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;");
                CryptoSymbols = await (from i in _context.StockInstruments where i.market == "stocks" && i.type.ToUpper() == "WARRANT" select i).AsNoTracking().ToListAsync();
            }
            return CryptoSymbols;
        }
        #endregion

        #region GetPrice
        [HttpGet]
        [Route("GetPrice")]
        public async Task<List<Binance_CryptoKandles>> GetPrice(string symbol = "SPY")
        {
            var CryptoSymbols = new List<Binance_CryptoKandles>();
            await using (apiallinoneapiappContext _context = new apiallinoneapiappContext())
            {
                await _context.Database.ExecuteSqlRawAsync("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;");
                CryptoSymbols = await (from i in _context.CryptoKandles where i.symbol == symbol select i).AsNoTracking().ToListAsync();
            }
            return CryptoSymbols;
        }
        #endregion

        #region GetPriceDetailed
        [HttpGet]
        [Route("GetPriceDetailed")]
        public async Task<List<api.allinoneapi.app.Models.TickersMainPage>> GetPriceDetailed(string? symbol, int limit = 15)
        {
            await using (apiallinoneapiappContext _context = new apiallinoneapiappContext())
            {
                if (symbol != null)
                {
                    var resp = await (from i in _context.StockDescription
                                      from d in _context.CryptoKandles
                                      where i.ticker == d.symbol && d.symbol == symbol
                                      orderby i.market_cap descending
                                      select new api.allinoneapi.app.Models.TickersMainPage()
                                      {
                                          ticker = i.ticker,
                                          name = i.name,
                                          closePrice = String.Format("{0:f3}", d.closePrice).Replace(".000", "").Replace(",000", ""),
                                          currency_name = i.currency_name

                                      }).Take(limit).AsNoTracking().ToListAsync();
                    return resp;
                }
                else
                {
                    var resp = await (from i in _context.StockDescription
                                      from d in _context.CryptoKandles
                                      where i.type == "CS" && i.ticker == d.symbol
                                      orderby i.market_cap descending
                                      select new api.allinoneapi.app.Models.TickersMainPage()
                                      {
                                          ticker = i.ticker,
                                          name = i.name,
                                          closePrice = String.Format("{0:f3}", d.closePrice).Replace(".000", "").Replace(",000", ""),
                                          currency_name = i.currency_name

                                      }).Take(limit).AsNoTracking().ToListAsync();
                    return resp;
                }
            }
        }
        #endregion

        #region GetKandles
        [HttpGet]
        [Route("GetKandles")]
        public List<Polygon_StockKandles> GetKandles(string symbol = "SPY", string interval = "1D", int minutes = 1, int lines = 1, string datestart = "", string dateend = "")
        {
            string intr = "minute";
            switch (interval)
            {
                case "1m":
                    {
                        intr = "minute";
                        break;
                    }
                case "1H":
                    {
                        intr = "hour";
                        break;
                    }
                case "1D":
                    {
                        intr = "day";
                        break;
                    }
                case "1W":
                    {
                        intr = "week";
                        break;
                    }
                case "1M":
                    {
                        intr = "month";
                        break;
                    }
                case "1Q":
                    {
                        intr = "quarter";
                        break;
                    }
                case "1Y":
                    {
                        intr = "year";
                        break;
                    }
            }
            string api = "1IDknqV7XjsFhZRNtwdNcJOtPp9IH0Ji";
            var kandles_resp = new List<Polygon_StockKandles>();
            //https://api.polygon.io/v2/aggs/ticker/SPY/range/1/hour/2023-06-01/2023-06-04?adjusted=true&sort=asc&limit=289&apiKey=1IDknqV7XjsFhZRNtwdNcJOtPp9IH0Ji
            //https://api.polygon.io/v2/aggs/ticker/SPY/range/1/hour/2023-06-01/2023-06-01?adjusted=true&sort=asc&limit=289&apiKey=1IDknqV7XjsFhZRNtwdNcJOtPp9IH0Ji


            string url_get = "https://api.polygon.io/v2/aggs/ticker/SPY/range/" + minutes + "/" + intr + "/" + datestart + "/" + dateend + "?adjusted=true&sort=asc&limit=" + lines + "&apiKey=";
            var url = url_get + api;
            var client = new RestClient(url);
            var request = new RestRequest(url, Method.Get);
            request.AddHeader("Content-Type", "application/json");
            var r = client.ExecuteAsync(request).Result.Content;

            var Content = new StringContent(r.ToString(), Encoding.UTF8, "application/json");
            JavaScriptSerializer? js = new JavaScriptSerializer();
            var ticker_resp = js.Deserialize<api.allinoneapi.Models.Root>(r);
            foreach (var a in ticker_resp.results)
            {
                var rsp = new Polygon_StockKandles();
                rsp.symbol = ticker_resp.ticker;
                rsp.source = "Polygon";
                rsp.openTime = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(a.t) / 1000).UtcDateTime;
                rsp.closeTime = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(a.t) / 1000).UtcDateTime;
                rsp.openPrice = Convert.ToDecimal(a.o);
                rsp.highPrice = Convert.ToDecimal(a.h);
                rsp.lowPrice = Convert.ToDecimal(a.l);
                rsp.closePrice = Convert.ToDecimal(a.c);
                rsp.volume = Convert.ToDecimal(a.v);
                rsp.quoteVolume = 0;
                rsp.tradeCount = 0;
                rsp.takerBuyBaseVolume = 0;
                rsp.takerBuyQuoteVolume = 0;
                kandles_resp.Add(rsp);
            }
            Content.Dispose();
            return kandles_resp;
            #endregion

        }
    }
}
