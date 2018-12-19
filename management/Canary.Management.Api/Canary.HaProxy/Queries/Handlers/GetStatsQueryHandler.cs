using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canary.HaProxy.Queries.Handlers
{
    public interface IGetStatsQueryHandler
    {
        Task<IEnumerable<Dictionary<string, string>>> GetStats();
    }
    public class GetStatsQueryHandler : BaseHandler, IGetStatsQueryHandler
    {
        public GetStatsQueryHandler(string host, int port) : base(host, port)
        {
        }

        public async Task<IEnumerable<Dictionary<string, string>>> GetStats()
        {
            string text = Encoding.ASCII.GetString(await this.SendTextAndReadResponse("show stat"));
            var lines = text.Split('\n').Select(s => s.Trim(' ', '#', ',')).ToArray();
            var cols = lines.First().Split(',').ToArray();
            return lines
                .Skip(1)
                .Select(l =>
                {
                    var vals = l.Split(',');
                    return Enumerable.Range(0, cols.Length)
                        .ToDictionary(i => cols[i], i => i < vals.Length ? vals[i] : null);
                });
        }
    }
}
