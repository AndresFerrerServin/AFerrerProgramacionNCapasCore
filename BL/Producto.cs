using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Producto
    {
        public static ML.Result GetAll(ML.Producto producto)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.MasterContext context = new DL.MasterContext())
                {
                    var query = context.Productos.FromSqlRaw($"ProductoGetAll '{producto.Nombre}',{producto.Departamento.IdDepartamento},{producto.Departamento.Area.IdArea}").ToList();
                    result.Objects = new List<object>();

                    if (query != null)
                    {
                        foreach (var obj in query)
                        {
                            producto = new ML.Producto();

                            producto.IdProducto = obj.IdProducto;
                            producto.Nombre = obj.Nombre;
                            producto.PrecioUnitario = obj.PrecioUnitario;
                            producto.Stock = obj.Stock;

                            producto.Proveedor = new ML.Proveedor();
                            producto.Proveedor.IdProveedor = obj.IdProveedor;
                            producto.Proveedor.Nombre = obj.ProveedorNombre;


                            producto.Departamento = new ML.Departamento();
                            producto.Departamento.IdDepartamento = obj.IdDepartamento;
                            producto.Departamento.Nombre = obj.DepartamentoNombre;

                            producto.Descripcion = obj.Descripcion;
                            producto.Imagen = obj.Imagen;

                            producto.Departamento.Area = new ML.Area();
                            producto.Departamento.Area.IdArea = obj.IdArea;
                            producto.Departamento.Area.Nombre = obj.AreaNombre;




                            result.Objects.Add(producto);
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

        public static ML.Result Add(ML.Producto producto)
        {

            ML.Result result = new ML.Result();
            try
            {
                using (DL.MasterContext context = new DL.MasterContext())
                {

                    var query = context.Database.ExecuteSqlRaw($"ProductoAdd '{producto.Nombre}',{producto.PrecioUnitario},{producto.Stock},'{producto.Descripcion}',{producto.Proveedor.IdProveedor},{producto.Departamento.IdDepartamento},'{producto.Imagen}'");
                    if (query >= 1)
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
        public static ML.Result Update(ML.Producto producto)
        {

            ML.Result result = new ML.Result();
            try
            {
                using (DL.MasterContext context = new DL.MasterContext())
                {
                    var query = context.Database.ExecuteSqlRaw($"ProductoUpdate {producto.IdProducto}, '{producto.Nombre}',{producto.PrecioUnitario},{producto.Stock},'{producto.Descripcion}',{producto.Proveedor.IdProveedor},{producto.Departamento.IdDepartamento},'{producto.Imagen}'");
                    if (query >= 1)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se actualizo";
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
        public static ML.Result Delete(ML.Producto producto)
        {

            ML.Result result = new ML.Result();
            try
            {
                using (DL.MasterContext context = new DL.MasterContext())
                {
                    var query = context.Database.ExecuteSqlRaw($"ProductoUpdate {producto.IdProducto}");
                    if (query >= 1)
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
        public static ML.Result GetId(int IdProducto)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.MasterContext context = new DL.MasterContext())
                {
                    var query = context.Productos.FromSqlRaw($"ProductoGetId {IdProducto}").AsEnumerable().FirstOrDefault();
                    result.Objects = new List<object>();

                    if (query != null)
                    {

                        ML.Producto producto = new ML.Producto();
                        producto.IdProducto = query.IdProducto;
                        producto.Nombre = query.Nombre;
                        producto.PrecioUnitario = query.PrecioUnitario;
                        producto.Stock = query.Stock;

                        producto.Proveedor = new ML.Proveedor();
                        producto.Proveedor.IdProveedor = query.IdProveedor;

                        producto.Departamento = new ML.Departamento();
                        producto.Departamento.IdDepartamento = query.IdDepartamento;
                        producto.Descripcion = query.Descripcion;
                        producto.Imagen = query.Imagen;

                        producto.Proveedor.Nombre = query.ProveedorNombre;
                        producto.Departamento.Nombre = query.DepartamentoNombre;

                        producto.Departamento.Area = new ML.Area();
                        producto.Departamento.Area.IdArea = query.IdArea;
                        producto.Departamento.Area.Nombre = query.AreaNombre;

                        result.Object = producto;
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
        public static ML.Result ConvertirExceltoDataTable(string connString)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (OleDbConnection context = new OleDbConnection(connString))
                {
                    string query = "SELECT * FROM [Hoja1$]";
                    using (OleDbCommand cmd = new OleDbCommand())
                    {
                        cmd.CommandText = query;
                        cmd.Connection = context;


                        OleDbDataAdapter da = new OleDbDataAdapter();
                        da.SelectCommand = cmd;

                        DataTable tableProducto = new DataTable();

                        da.Fill(tableProducto);

                        if (tableProducto.Rows.Count > 0)
                        {
                            result.Objects = new List<object>();

                            foreach (DataRow row in tableProducto.Rows)
                            {
                                ML.Producto producto = new ML.Producto();
                                producto.Nombre = row[0].ToString();

                                producto.PrecioUnitario = (row[1].ToString() == "")? 0 : decimal.Parse(row[1].ToString());

                                producto.Stock = (row[2].ToString() == "")? 0 : int.Parse(row[2].ToString());

                                producto.Proveedor = new ML.Proveedor();
                                producto.Proveedor.IdProveedor = (row[3].ToString() == "") ? 0 : int.Parse(row[3].ToString());

                                producto.Departamento = new ML.Departamento();
                                producto.Departamento.IdDepartamento = (row[4].ToString() == "") ? 0 : int.Parse(row[4].ToString());

                                producto.Descripcion = row[5].ToString();
                                producto.Imagen = null;

                                result.Objects.Add(producto);
                            }

                            result.Correct = true;

                        }

                        result.Object = tableProducto;

                        if (tableProducto.Rows.Count > 1)
                        {
                            result.Correct = true;
                        }
                        else
                        {
                            result.Correct = false;
                            result.ErrorMessage = "No existen registros en el excel";
                        }
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

        public static ML.Result ValidarExcel(List<object> Object)
        {
            ML.Result result = new ML.Result();

            try
            {
                result.Objects = new List<object>();
                //DataTable  //Rows //Columns
                int i = 1;
                foreach (ML.Producto producto in Object)
                {
                    ML.ErrorExcel error = new ML.ErrorExcel();
                    error.IdRegistro = i++;

                    if (producto.Nombre == "")
                    {
                        error.Mensaje += "Ingresa el  nombre :";
                    }
                    if (producto.PrecioUnitario == 0)
                    {
                        error.Mensaje += "Ingresa el  precio unitario :";
                    }
                    if (producto.Stock == 0)
                    {
                        error.Mensaje += "Ingresa un valor a stock :";
                    }
                    if (producto.Proveedor.IdProveedor == 0)
                    {
                        error.Mensaje += "Ingresa un proveedor :";
                    }
                    if (producto.Departamento.IdDepartamento == 0)
                    {
                        error.Mensaje += "Ingresa el  Departamento :";
                    }
                    if (producto.Descripcion == "")
                    {
                        error.Mensaje += "Ingresa la descripcion del producto :";
                    }

                    if (error.Mensaje != null)
                    {
                        result.Objects.Add(error);
                    }


                }
                result.Correct = true;
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;

            }

            return result;
        }


        public static ML.Result ProductoGetByIdDepartamento(int IdDepartamento)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.MasterContext context = new DL.MasterContext())
                {
                    var query = context.Productos.FromSqlRaw($@"ProductoGetByIdDepartamento {IdDepartamento}").ToList();
                    result.Objects = new List<object>();
                    if (query != null)
                    {
                        foreach (var obj in query)
                        {
                            ML.Producto producto = new ML.Producto();
                            producto.IdProducto = obj.IdProducto;
                            producto.Nombre = obj.Nombre;
                            producto.PrecioUnitario = obj.PrecioUnitario;
                            producto.Stock = obj.Stock;

                            producto.Proveedor = new ML.Proveedor();
                            producto.Proveedor.IdProveedor = obj.IdProveedor;
                            producto.Proveedor.Nombre = obj.ProveedorNombre;


                            producto.Departamento = new ML.Departamento();
                            producto.Departamento.IdDepartamento = obj.IdDepartamento;
                            producto.Departamento.Nombre = obj.DepartamentoNombre;

                            producto.Descripcion = obj.Descripcion;
                            producto.Imagen = obj.Imagen;

                            producto.Departamento.Area = new ML.Area();
                            producto.Departamento.Area.IdArea = obj.IdArea;
                            producto.Departamento.Area.Nombre = obj.AreaNombre;

                            result.Objects.Add(producto);


                        }
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No exixte registros";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }
            return result;
        }
    }
}
