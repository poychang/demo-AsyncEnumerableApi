using Microsoft.AspNetCore.Mvc;

namespace AsyncEnumerableApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateSlimBuilder(args);
            builder.Services.AddControllers();

            var app = builder.Build();
            app.UseAuthorization();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.MapControllers();
            app.Run();
        }
    }

    [ApiController]
    [Route("[controller]")]
    public class StreamController : ControllerBase
    {
        private int MAX = 4;

        [HttpGet]
        [Route("int")]
        public async IAsyncEnumerable<int> GetInt()
        {
            for (int i = 0; i < MAX; i++)
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
                yield return i;
            }
        }

        [HttpGet]
        [Route("string")]
        public async IAsyncEnumerable<string> GetString()
        {
            for (int i = 0; i < MAX; i++)
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
                yield return Guid.NewGuid().ToString();
            }
        }

        [HttpGet]
        [Route("object")]
        public async IAsyncEnumerable<Demo> GetObject()
        {
            for (int i = 0; i < MAX; i++)
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
                yield return new Demo { Id = i, Name = Guid.NewGuid().ToString() };
            }
        }

        [HttpGet]
        [Route("just-string")]
        public async Task<IActionResult> GetJustString()
        {
            // 設定回應的 Content-Type
            Response.Headers.Append("Content-Type", "text/plain");
            // 逐筆將資料加到回應內容中
            await foreach (var item in Streaming())
            {
                if (item is null) continue;
                await Response.WriteAsync(item);
            }
            // 完成回應
            await Response.CompleteAsync();
            // 最終返回一個空結果
            return new EmptyResult();

            async IAsyncEnumerable<string> Streaming()
            {
                for (int i = 0; i < MAX; i++)
                {
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    yield return Guid.NewGuid().ToString();
                }
            }
        }
    }

    public class Demo
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
