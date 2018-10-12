using System.Data.Entity;

namespace WebSanaAssessment.Models
{
   public class ModelDbContext : DbContext
   {
      public DbSet<ProductModel> Product { get; set; }
   }
}