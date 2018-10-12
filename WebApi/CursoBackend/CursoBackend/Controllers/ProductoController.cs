using CursoBackend.Datos;
using CursoBackend.Models;
using CursoBackend.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace CursoBackend.Controllers
{
    public class ProductoController : ApiController
    {
        // GET api/<controller>
        public IHttpActionResult Get()
        {
            return Ok(ProductoRepository.Instance.ObtenerProductos());
        }

        // GET api/<controller>/5 -- IHttpActionResult
        public Producto Get(int id)
        {
            return ProductoRepository.Instance.ObtenerProducto(id);
        }

        // POST api/<controller>/
        [HttpPost]        
        [ActionName("AgregarProducto")]
        public IHttpActionResult AgregarProducto(JObject pro)
        {
            Respuesta rpta = new Respuesta(false, "ERROR");
            try
            {
                var _producto = Util.Util.JsonToObject<Producto>(pro);

                if (_producto != null)
                {
                    ProductoRepository.Instance.CrearEntidad(_producto);
                    rpta = new Respuesta(true, "OK");
                }
            }
            catch (Exception ex)
            {
                rpta.Mensaje = ex.Message;
            }

            return Ok(rpta);
            //return rpta;            
        }        

        [HttpPost]
        [ActionName("GuardarProducto")]
        [HandleException(Type = typeof(ArgumentNullException), Status = HttpStatusCode.BadRequest)]
        [HandleException(Type = typeof(Exception), Status = HttpStatusCode.NotAcceptable)]
        public IHttpActionResult GuardarProducto(Producto pro)
        {            
            Respuesta rpta = new Respuesta(false, "ERROR");

            // pro = null;
            //try
            //{
                var httpReq = HttpContext.Current.Request;                
                var file = httpReq.Files.Count > 0 ? httpReq.Files[0] : null;

                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);

                    var path = Path.Combine(
                        HttpContext.Current.Server.MapPath("~/App_Data"),
                        fileName
                    );

                    file.SaveAs(path);
                    pro.Imagen = "/App_Data/" + fileName;
                    ProductoRepository.Instance.CrearEntidad(pro);
                    rpta = new Respuesta(true, "OK");
                }
                else {
                    rpta.Mensaje = "No se obtuvo el archivo.";
                }
            //}
            //catch (Exception ex)
            //{
            //    rpta.Mensaje = ex.Message;
            //}

            return Ok(rpta);
        }

        [HttpPut]
        public IHttpActionResult Put(JObject pro)
        {
            Respuesta rpta = new Respuesta(false, "ERROR"); ;
            try
            {
                var _producto = Util.Util.JsonToObject<Producto>(pro);

                if (_producto != null)
                {
                    ProductoRepository.Instance.ActualizarEntidad(_producto);
                    rpta = new Respuesta(true, "OK");
                }
            }
            catch (Exception ex)
            {
                rpta.Mensaje = ex.Message;
            }

            return Ok(rpta);
        }

        [HttpPut]
        [ActionName("ActualizarProducto")]
        public IHttpActionResult Actualizar(Producto pro)
        {
            Respuesta rpta = new Respuesta(false, "ERROR");
            var httpReq = HttpContext.Current.Request;
            var file = httpReq.Files.Count > 0 ? httpReq.Files[0] : null;

            if (file != null && file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);

                var path = Path.Combine(
                    HttpContext.Current.Server.MapPath("~/App_Data"),
                    fileName
                );

                file.SaveAs(path);
                pro.Imagen = "/App_Data/" + fileName;
                ProductoRepository.Instance.ActualizarEntidad(pro);
                rpta = new Respuesta(true, "OK");
            }
            else
            {
                rpta.Mensaje = "No se obtuvo el archivo.";
            }
            return Ok(rpta);
        }

        // DELETE api/<controller>/5
        [HttpDelete]
        [ActionName("EliminarProducto")]
        public IHttpActionResult Delete(long id)
        {
            Respuesta rpta = new Respuesta(false, "ERROR");

            try
            {
                ProductoRepository.Instance.EliminarEntidad(id);
                rpta = new Respuesta(true, "OK");
            }
            catch (Exception ex)
            {
                rpta.Mensaje = ex.Message;
            }

            return Ok(rpta);
        }

        [HttpGet]
        // [Authorize]
        [ActionName("Archivos")]
        public HttpResponseMessage GetFile([FromUri]string path)
        {
            HttpResponseMessage result = Request.CreateResponse(HttpStatusCode.Gone);

            if (!string.IsNullOrEmpty(path))
            {
                path = Util.Criptografia.Decrypt(path);
                var fullPath = HttpContext.Current.Server.MapPath("~" + path);

                if (File.Exists(fullPath))
                {
                    result = Request.CreateResponse(HttpStatusCode.Gone);

                    // Serve the file to the client
                    result = Request.CreateResponse(HttpStatusCode.OK);
                    result.Content = new StreamContent(new FileStream(fullPath, FileMode.Open, FileAccess.Read));
                    result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("inline"); //attachment
                    result.Content.Headers.ContentDisposition.FileName = Path.GetFileName(fullPath);
                }
            }
            return result;
        }
    }

    public class Respuesta
    {
        public bool Proceso { get; set; }
        public string Mensaje { get; set; }

        public Respuesta() { }

        public Respuesta(bool proceso, string mensaje) {
            Proceso = proceso;
            Mensaje = mensaje;
        }
    }
}