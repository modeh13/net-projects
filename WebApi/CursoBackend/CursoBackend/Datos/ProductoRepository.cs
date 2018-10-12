using CursoBackend.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace CursoBackend.Datos
{
    public class ProductoRepository
    {
        private static ProductoRepository instance;

        public static ProductoRepository Instance
        {
            get {
                if (instance == null) return instance = new ProductoRepository();
                else return instance;
            }
        }

        public IEnumerable<Producto> ObtenerProductos()
        {
            List<Producto> listado;

            using (Model ctx = new Model())
            {
                DbQuery<Producto> query = ctx.Set<Producto>();
                listado = query.ToList();
            }

            listado.ForEach(p => p.Imagen = (!string.IsNullOrEmpty(p.Imagen)) ? Util.Criptografia.Encrypt(p.Imagen) : string.Empty);

            return listado;
        }

        public Producto ObtenerProducto(int id)
        {
            Producto producto;

            using (Model ctx = new Model())
            {
                DbQuery<Producto> query = ctx.Set<Producto>();
                producto = query.FirstOrDefault(p => p.Id == id);
                producto.Imagen = (!string.IsNullOrEmpty(producto.Imagen)) ? Util.Criptografia.Encrypt(producto.Imagen) : string.Empty;
            }

            return producto;
        }

        public void CrearEntidad(Producto pro)
        {
            using (Model ctx = new Model())
            {
                using (var dbTran = ctx.Database.BeginTransaction())
                {
                    try
                    {
                        ctx.Producto.Add(pro);
                        ctx.SaveChanges();
                        dbTran.Commit();
                    }
                    catch (Exception ex)
                    {
                        dbTran.Rollback();
                        throw ex;
                    }                    
                }
            }
        }

        public void ActualizarEntidad(Producto pro)
        {
            using (Model ctx = new Model())
            {
                using (var dbTran = ctx.Database.BeginTransaction())
                {
                    try
                    {                        
                        ctx.Entry(pro).State = EntityState.Modified;
                        ctx.SaveChanges();
                        dbTran.Commit();
                    }
                    catch (Exception ex)
                    {
                        dbTran.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public void EliminarEntidad(long id)
        {
            using (Model ctx = new Model())
            {
                using (var dbTran = ctx.Database.BeginTransaction())
                {
                    try
                    {
                        var entidad = new Producto() { Id = id };
                        ctx.Entry(entidad).State = EntityState.Deleted;
                        ctx.SaveChanges();                       
                        dbTran.Commit();
                    }
                    catch (Exception ex)
                    {
                        dbTran.Rollback();
                        throw ex;
                    }
                }
            }
        }
    }
}