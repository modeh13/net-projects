using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using WebSanaAssessment.Models;
using WebSanaAssessment.Util;

namespace WebSanaAssessment.Controllers
{
   //Pending   
   //1- Validation by Number of Product.
   //The base template used: Visual Studio by default.

   public class ProductController : Controller
   {
      #region Attributes - Properties
      //private static List Contacts = new List();
      private static List<ProductModel> productList = new List<ProductModel>();

      /// <summary>
      /// Path where the XML file is, this contains Product elements.
      /// </summary>
      private string XmlPath;

      /// <summary>
      /// Path where the CSV file is, this contains Product elements.
      /// </summary>
      private string CsvPath;

      /// <summary>
      /// Character used as separator in the CSV file.
      /// </summary>
      private char CsvSeparator = '|';

      /// <summary>
      /// DataSource type (1: In-Memory, 2: XML)
      /// </summary>
      public int DataSourceType
      {
         get { return (int)Session["dataSourceType"]; }
         set { Session["dataSourceType"] = value; }
      }

      /// <summary>
      /// Property to store Products in memory 
      /// </summary>
      public List<ProductModel> ProductList
      {
         get {
            if (Session["productList"] == null) {
               Session["productList"] = new List<ProductModel>();
            }

            return (List<ProductModel>)Session["productList"];
         }
         set { Session["productList"] = value; }
      }
      #endregion

      #region Constructors
      public ProductController()
      {         
         XmlPath = HostingEnvironment.MapPath(ConfigurationManager.AppSettings["xmlPath"]);
         CsvPath = HostingEnvironment.MapPath(ConfigurationManager.AppSettings["csvPath"]);         
      } 
      #endregion      

      #region "In-Memory"
      /// <summary>
      /// Add new Product to collection
      /// </summary>
      /// <param name="product">Product object</param>
      private void AddProductMemory(ProductModel product)
      {         
         ProductList.Add(product);
      }

      /// <summary>
      /// Delete a Product by Id
      /// </summary>
      /// <param name="productId">ProductId</param>
      private void DeleteProductMemory(int productId)
      {
         if (ProductList.Any(x => x.Id == productId))
         {
            ProductList.RemoveAll(x => x.Id == productId);
         }
      }

      #endregion

      #region "XML"
      /// <summary>
      /// Get Product elements by XML file
      /// </summary>
      /// <returns></returns>
      private List<ProductModel> GetProductModelsByXml()
      {
         return Utilities.Deserialize<ProductModel>(XmlPath);
      }

      /// <summary>
      /// Add new Product to XML file
      /// </summary>
      /// <param name="product">Product object</param>
      private void AddProductXml(ProductModel product)
      {
         XmlDocument xmlDoc = new XmlDocument();
         xmlDoc.Load(XmlPath);

         XmlElement xmlElement = Utilities.SerializeToXmlElement(product);
         XmlNode importNode = xmlDoc.ImportNode(xmlElement, true);
         xmlDoc.DocumentElement.InsertAfter(importNode, xmlDoc.DocumentElement.LastChild);
         xmlDoc.Save(XmlPath);
      }

      /// <summary>
      /// Delete a Product by Id
      /// </summary>
      /// <param name="productId">ProductId</param>
      private void DeleteProductXml(int productId)
      {         
         XDocument doc = XDocument.Load(XmlPath);

         var elements = from node in doc.Descendants("ProductModel")                 
                        where node.Descendants("Id").First().Value == productId.ToString()
                        select node;
         
         elements?.ToList().ForEach(x => x.Remove());
         doc.Save(XmlPath);
      }
      #endregion

      #region "CSV"
      /// <summary>
      /// Get Product elements by Csv file
      /// </summary>
      /// <returns></returns>
      private List<ProductModel> GetProductModelsByCsv()
      {
         //Id|Number|Title|Description|Price|Stock|Status|CreationDate
         return Utilities.GetListFromCsv<ProductModel>(CsvPath, CsvSeparator);        
      }

      /// <summary>
      /// Add new Product to XML file
      /// </summary>
      /// <param name="product">Product object</param>
      private void AddProductCsv(ProductModel product)
      {
         Utilities.WriteObjectToCsv(product, CsvPath, CsvSeparator);
      }

      /// <summary>
      /// Delete a Product by Id
      /// </summary>
      /// <param name="productId">ProductId</param>
      private void DeleteProductCsv(int productId)
      {
         var oldLines = System.IO.File.ReadAllLines(CsvPath);
         var newLines = oldLines.Where(line => !line.StartsWith($"{productId}{CsvSeparator}"));
         Utilities.WriteAllLinesBetter(CsvPath, newLines.ToArray());
      }
      #endregion

      #region "Database"
      /// <summary>
      /// Get Product elements from Database
      /// </summary>
      /// <returns>List of Products</returns>
      private List<ProductModel> GetProductByDataBase()
      {         
         using (ModelDbContext DbContext = new ModelDbContext())
         {
            return DbContext.Product.ToList();
         }
      }

      /// <summary>
      /// Add new Product to XML file
      /// </summary>
      /// <param name="product">Product object</param>
      private void AddProductDatabase(ProductModel product)
      {
         using (ModelDbContext DbContext = new ModelDbContext())
         {
            if(DbContext.Product.Any(x => x.Number == product.Number))
            {               
               throw new System.Exception($"A Product already exists with the same 'Number': {product.Number}");
            }

            DbContext.Product.Add(product);
            DbContext.SaveChanges();            
         }
      }

      /// <summary>
      /// Delete a Product by Id
      /// </summary>
      /// <param name="productId">ProductId</param>
      private void DeleteProductDatabase(int productId)
      {
         using (ModelDbContext DbContext = new ModelDbContext())
         {  
            ProductModel product = DbContext.Product.Find(productId);
            DbContext.Product.Remove(product);
            DbContext.SaveChanges();
         }
      }
      #endregion

      #region Methods
      /// <summary>
      /// Get the Datasource types to show in DropdownList
      /// </summary>
      /// <returns>SelectList</returns>
      private SelectList GetDataSourceTypes()
      {
         SelectList listItems = new SelectList(
            new List<SelectListItem>(){
               new SelectListItem
               {
                  Text = "In-Memory Storage",
                  Value = "1",
                  Selected = true
               },
               new SelectListItem
               {
                  Text = "XML storage",
                  Value = "2"
               },
               new SelectListItem
               {
                  Text = "CSV storage",
                  Value = "3"
               },
               new SelectListItem
               {
                  Text = "Database",
                  Value = "4"
               }
            }, "Value", "Text");

         return listItems;
      }

      /// <summary>
      /// Get ProductId for a new Product.
      /// </summary>
      /// <returns></returns>
      private long GetProductId()
      {
         long id = 1;
         var productsList = GetProductsList();

         if (productsList.Any())
         {
            id = productsList.Max(x => x.Id) + 1;
         }

         return id;
      }

      /// <summary>
      /// Get Products List based on DataSource type.
      /// </summary>
      /// <returns>Product List</returns>
      private List<ProductModel> GetProductsList()
      {
         switch (DataSourceType)
         {
            case 1: //In-memory
               return ProductList;
            case 2: //Xml               
               return GetProductModelsByXml();
            case 3: //Csv
               return GetProductModelsByCsv();
            case 4: //Database
               return GetProductByDataBase();
         }

         return new List<ProductModel>();
      }
      #endregion

      #region "Actions"
      /// <summary>
      /// Get IndexView
      /// </summary>
      /// <returns></returns>
      public ActionResult Index()
      {
         ViewData["dataSources"] = GetDataSourceTypes();

         //Set Datasource by default.
         if (Session["dataSourceType"] == null)
         {
            DataSourceType = 1;
         }

         return View(GetProductsList());
      }

      /// <summary>
      /// Change DataSource type.
      /// </summary>
      /// <returns>GridView</returns>
      public ActionResult ChangeDataSource(int dataSourceType)
      {
         if (dataSourceType > 0) DataSourceType = dataSourceType;
         return PartialView("Grid", GetProductsList());
      }

      /// <summary>
      /// Get the FormView for the Product by Id
      /// </summary>
      /// <param name="productId">ProductId</param>
      /// <returns></returns>
      public ActionResult Form(int productId)
      {  
         return PartialView(GetProductsList().FirstOrDefault(x => x.Id == productId));
      }

      /// <summary>
      /// Add a new Product based on DataSource type.
      /// </summary>
      /// <param name="product">Product object</param>
      /// <returns>GridView with Product List, FormView for a Product.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Form(ProductModel product)
      {
         if (ModelState.IsValid)
         {
            try
            {
               product.Id = GetProductId();
               switch (DataSourceType)
               {
                  case 1:
                     AddProductMemory(product);
                     break;
                  case 2:
                     AddProductXml(product);
                     break;
                  case 3:
                     AddProductCsv(product);
                     break;
                  case 4:
                     AddProductDatabase(product);
                     break;
               }
            }
            catch (System.Exception ex)
            {
               return new HttpStatusCodeResult(HttpStatusCode.BadRequest, ex.Message);
            }

            return PartialView("Grid", GetProductsList());
         }

         return PartialView("Form", product);
      }

      /// <summary>
      /// Delete a Product by ProductId
      /// </summary>
      /// <param name="productId">ProductId</param>
      /// <returns>GridView</returns>
      [HttpPost]      
      public ActionResult Delete(int productId)
      {
         switch (DataSourceType)
         {
            case 1:
               DeleteProductMemory(productId);
               break;
            case 2:
               DeleteProductXml(productId);
               break;
            case 3:
               DeleteProductCsv(productId);
               break;
            case 4:
               DeleteProductDatabase(productId);
               break;
         }

         return PartialView("Grid", GetProductsList());
      }
      #endregion
   }
}