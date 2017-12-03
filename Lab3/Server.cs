using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Lab3
{
  public class Server
  {
    private HttpListener listener;
    private string serverUri;
    private HttpListenerContext context;
    string requestBody;

    public void Start()
    {
      this.listener.Start();
      while (this.listener.IsListening)
      {
        this.context = this.listener.GetContext();
        var methodName = this.context.Request.Url.LocalPath.Substring(1).ToLower();
        var methodInfo = typeof(Server).GetMethods(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
          .FirstOrDefault(a => string.Compare(a.Name, methodName, true) == 0);
        if (methodInfo != null)
          methodInfo.Invoke(this, new Type[] { });
        else
          SendResponse();
      }
    }

    public void Stop()
    {
      this.SendResponse();
      this.listener.Stop();
    }

    private void ping()
    {
      this.SendResponse();
    }

    private void PostInputData()
    {
      var stream = this.context.Request.InputStream;
      var encoding = this.context.Request.ContentEncoding;
      using (var reader = new StreamReader(stream, encoding))
      {
        this.requestBody = reader.ReadToEnd();
      }
      this.GetAnswer();
    }

    private void GetAnswer()
    {
      var input = this.requestBody.Deserialize<Input>();
      var output = new Output(input);
      var serializedOutput = output.Serialize();
      this.SendResponse(serializedOutput);
    }

    private void SendResponse(string body = "")
    {
      var response = context.Response;
      response.StatusCode = (int)HttpStatusCode.OK;
      response.ContentEncoding = Encoding.UTF8;
      response.ContentLength64 = Encoding.UTF8.GetByteCount(body);
      using (Stream stream = response.OutputStream)
      {
        stream.Write(Encoding.UTF8.GetBytes(body), 0, (int)response.ContentLength64);
      }
    }

    public Server(string uri)
    {
      this.serverUri = uri;
      this.listener = new HttpListener();
      this.listener.Prefixes.Add(uri);
      this.requestBody = string.Empty;
    }
  }
}