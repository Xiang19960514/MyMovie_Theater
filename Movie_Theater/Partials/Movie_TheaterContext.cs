using Microsoft.EntityFrameworkCore;

namespace Movie_Theater.Models
{
    public partial class Movie_TheaterContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // 判斷是否設定過
            if (!optionsBuilder.IsConfigured)
            {
                IConfiguration Config = new ConfigurationBuilder()
                    // 設定專案跟目錄
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    // 加入專案目錄的設定檔
                    .AddJsonFile("appsettings.json")
                    // 生成設定
                    .Build();
                // 進行SQLSeerver連線(無填入導覽)
                optionsBuilder.UseSqlServer(Config.GetConnectionString("Movie"));
            }
        }
    }
}
