using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canary.HaProxy.Queries.Handlers
{
    public interface IGetConditionalRoutesQueryHandler
    {
        Task<Dictionary<int, string>> GetAllAcls();
        Task<IEnumerable<string>> GetAclValues(int aclIndex);
    }

    public class GetConditionalRoutesQueryHandler: BaseHandler, IGetConditionalRoutesQueryHandler
    {
        public GetConditionalRoutesQueryHandler(string host, int port) : base(host, port)
        {
        }

        public async Task<Dictionary<int, string>> GetAllAcls()
        {
            string text = Encoding.ASCII.GetString(await this.SendTextAndReadResponse("show acl"));
            return getLines(text)
                .ToDictionary(
                    k => int.Parse(k.Split('(', ')').First()),
                    v => v);
        }

        public async Task<IEnumerable<string>> GetAclValues(int aclIndex)
        {
            //todo: this should read multiple acls at once which will be faster
            string text = Encoding.ASCII.GetString(await this.SendTextAndReadResponse($"show acl #{aclIndex}"));
            return getLines(text)
                .Select(l => l.StartsWith("0x")
                    ? l.Substring(l.IndexOf(" ")).Trim()
                    : l);
        }


        private static IEnumerable<string> getLines(string text)
        {
            return text
                .Trim()
                .Split("\n")
                .Select(l => l.Trim())
                .Where(l => !l.StartsWith("#"));
        }

    }
}
