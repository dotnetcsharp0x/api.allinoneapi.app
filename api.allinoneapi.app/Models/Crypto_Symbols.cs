﻿namespace api.allinoneapi.app.Models
{
    public class Crypto_Symbols
    {
        public int Id { get; set; }
        public string? Symbol { get; set; }
        public string? BaseAsset { get; set; }
        public string? QuoteAsset { get; set; }
        public float? circulating_supply { get; set; }
        public float? total_supply { get; set; }

    }
}
