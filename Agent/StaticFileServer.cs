using System.Net;
using System.Reflection;

namespace Agent
{
    public class StaticFileServer()
    {
        public async Task ServeStaticFileAsync(HttpListenerContext context)
        {
            if (context.Request.HttpMethod != "GET")
            {
                context.Response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
                byte[] errorMessage = System.Text.Encoding.UTF8.GetBytes("Only GET requests are allowed");
                await context.Response.OutputStream.WriteAsync(errorMessage, 0, errorMessage.Length);
                context.Response.Close();
                return;
            }

            string? resourcePath = context.Request.Url?.AbsolutePath.TrimStart('/');
            if (string.IsNullOrEmpty(resourcePath))
            {
                resourcePath = "index.html"; // Serve index.html by default
            }

            resourcePath = resourcePath.Replace('/', '.');

            // Replace 'Agent' with your actual namespace
            string resourceName = $"Agent.static.{resourcePath}";

            using (Stream? resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            {
                if (resourceStream == null)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    byte[] errorMessage = System.Text.Encoding.UTF8.GetBytes("File not found");
                    await context.Response.OutputStream.WriteAsync(errorMessage, 0, errorMessage.Length);
                }
                else
                {
                    context.Response.StatusCode = (int)HttpStatusCode.OK;
                    context.Response.ContentType = GetContentType(resourcePath);
                    await resourceStream.CopyToAsync(context.Response.OutputStream);
                }
            }

            context.Response.Close();
        }

        private static string GetContentType(string path)
        {
            string extension = Path.GetExtension(path).ToLowerInvariant();
            return extension switch
            {
                ".htm" => "text/html",
                ".html" => "text/html",
                ".css" => "text/css",
                ".js" => "text/javascript",
                ".json" => "application/json",
                ".png" => "image/png",
                ".jpg" => "image/jpeg",
                ".jpeg" => "image/jpeg",
                ".gif" => "image/gif",
                ".svg" => "image/svg+xml",
                ".woff" => "font/woff",
                ".woff2" => "font/woff2",
                _ => "application/octet-stream",
            };
        }
    }
}
