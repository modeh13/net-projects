using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebSanaAssessment.Models
{
   [Table("Products")]
   public class ProductModel
   {
      #region Properties
      [Key]
      public long Id { get; set; }

      [Required(ErrorMessage = "Required Number")]
      public int Number { get; set; }

      [Required(ErrorMessage = "Required Title")]
      public string Title { get; set; }
      public string Description { get; set; }

      [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
      [Required(ErrorMessage = "Required Price")]
      public decimal Price { get; set; }

      [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = false)]
      [Required(ErrorMessage = "Required Stock")]
      public int Stock { get; set; }

      public bool Status { get; set; }
      public DateTime? CreationDate { get; set; }
      #endregion

      #region Constructors
      public ProductModel()
      {
         Id = 1;
         Status = true;
         CreationDate = DateTime.Now;
      }
      #endregion

      #region Methods
      /// <summary>
      /// Get Default Product
      /// </summary>
      /// <returns>ProductModel</returns>
      public static ProductModel GetDefaultProduct()
      {
         Random random = new Random();

         return new ProductModel()
         {
            Id = random.Next(0, 1000),
            Number = random.Next(0, 9999999),
            Title = "Product",
            Description = "This is a default product",
            Price = random.Next(0, 99999999),
            Stock = random.Next(0, 120),
            Status = true,
            CreationDate = DateTime.Now
         };
      } 
      #endregion
   }
}