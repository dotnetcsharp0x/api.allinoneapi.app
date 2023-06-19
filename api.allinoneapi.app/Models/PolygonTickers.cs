namespace api.allinoneapi.app.Models
{
    public class PolygonTickers
    {
        #region polygon.io
        public List<Result> results { get; set; }
        public string status { get; set; }
        public string request_id { get; set; }
        public int count { get; set; }
        public string next_url { get; set; }

        #endregion
    }
}
