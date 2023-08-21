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
            app.UseHttpsRedirection();
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
    }

    public class Demo
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
