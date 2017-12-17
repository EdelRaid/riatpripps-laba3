using System.IO;
using System.Net;
using System.Text;

namespace Lab3
{
  public class Server
  {
    private HttpListener listener;

    public void Start()
    {
      this.listener.Start();
      while (this.listener.IsListening)
      {
        var context = this.listener.GetContext();
        var methodName = context.Request.Url.LocalPath.Substring(1).ToUpper();
        if (methodName == nameof(this.Ping).ToUpper())
          this.Ping(context);
        else if (methodName == nameof(this.PostInputData).ToUpper())
          this.PostInputData(context);
        else if (methodName == nameof(this.GetAnswer).ToUpper())
          this.GetAnswer(context, string.Empty);
        else
          this.SendResponse(context, string.Empty);
      }
    }

    public void Stop()
    {
      this.listener.Stop();
    }

    private void Ping(HttpListenerContext context)
    {
      this.SendResponse(context, string.Empty);
    }

    private void PostInputData(HttpListenerContext context)
    {
      var stream = context.Request.InputStream;
      var encoding = context.Request.ContentEncoding;
      using (var reader = new StreamReader(stream, encoding))
      {
        var requestBody = reader.ReadToEnd();
        this.GetAnswer(context, requestBody);
      }
    }

    private void GetAnswer(HttpListenerContext context, string requestBody)
    {
      var input = requestBody.Deserialize<Input>();
      var output = new Output(input);
      var serializedOutput = output.Serialize();
      this.SendResponse(context, serializedOutput);
    }

    private void SendResponse(HttpListenerContext context, string requestBody)
    {
      var response = context.Response;
      response.StatusCode = (int)HttpStatusCode.OK;
      response.ContentEncoding = Encoding.UTF8;
      response.ContentLength64 = Encoding.UTF8.GetByteCount(requestBody);
      using (Stream stream = response.OutputStream)
      {
        stream.Write(Encoding.UTF8.GetBytes(requestBody), 0, (int)response.ContentLength64);
      }
    }

    public Server(string uri)
    {
      this.listener = new HttpListener();
      this.listener.Prefixes.Add(uri);
    }
  }
}