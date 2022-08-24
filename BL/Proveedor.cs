using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Proveedor
    {
        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.MasterContext context = new DL.MasterContext())
                {
                    var query = context.Proveedors.FromSqlRaw($"ProveedorGetAll").ToList();
                    result.Objects = new List<object>();
                    if (query != null)
                    {
                        foreach (var obj in query)
                        {
                            ML.Proveedor proveedor = new ML.Proveedor();
                            proveedor.IdProveedor = obj.IdProveedor;
                            proveedor.Nombre = obj.Nombre;
                            proveedor.Telefono = obj.Telefono;

                            result.Objects.Add(proveedor);
                        }
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se encontro el registro";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;

            }

            return result;
        }
        public static ML.Result GetId(int IdProveedor)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.MasterContext context = new DL.MasterContext())
                {
                    var query = context.Proveedors.FromSqlRaw($"ProveedorGetId {IdProveedor} ").AsEnumerable().FirstOrDefault();
                    result.Objects = new List<object>();
                    if (query != null)
                    {

                        ML.Proveedor proveedor = new ML.Proveedor();
                        proveedor.IdProveedor = query.IdProveedor;
                        proveedor.Nombre = query.Nombre;
                        proveedor.Telefono = query.Telefono;


                        result.Object = proveedor;
                        result.Correct = true;

                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se encontro el registro";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }
        public static ML.Result Add(ML.Proveedor proveedor)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.MasterContext context = new DL.MasterContext())
                {
                    var query = context.Database.ExecuteSqlRaw($"ProveedorAdd '{proveedor.Nombre}','{proveedor.Telefono}'") ;
                    
                    if (query > 0)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se inserto el registro";
                    }
                    result.Correct = true;
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }
        public static ML.Result Update(ML.Proveedor proveedor)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.MasterContext context = new DL.MasterContext())
                {
                    var query = context.Database.ExecuteSqlRaw($"ProveedorUpdate {proveedor.IdProveedor},'{proveedor.Nombre}','{proveedor.Telefono}'");

                    if (query > 0)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se inserto el registro";
                    }
                    result.Correct = true;
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }
        public static ML.Result Delete(ML.Proveedor proveedor)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.MasterContext context = new DL.MasterContext())
                {
                    var query = context.Database.ExecuteSqlRaw($"ProveedorDelete {proveedor.IdProveedor}");
                    if (query > 0)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se elimino";
                    }
                    result.Correct = true;
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }
    }

}
