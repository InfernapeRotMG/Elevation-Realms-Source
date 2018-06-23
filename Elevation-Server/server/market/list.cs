using System.IO;

namespace server.market
{
    class list : RequestHandler
    {
        protected override void HandleRequest()
        {
            using (StreamWriter wtr = new StreamWriter(Context.Response.OutputStream))
                wtr.Write("{ }"); //TODO
        }
    }
}
