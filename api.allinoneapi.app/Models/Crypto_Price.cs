﻿using System.ComponentModel.DataAnnotations;

namespace api.allinoneapi.app.Models
{
    public class Crypto_Price : IDisposable
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        public decimal Price { get; set; }
        public DateTime DateTime { get; set; }

        public void Dispose()
        {
        }
    }
}
