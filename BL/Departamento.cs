using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Departamento
    {
        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.MasterContext context = new DL.MasterContext())
                {
                    var query = context.Departamentos.FromSqlRaw($"DepartamentoGetAll").ToList();
                    result.Objects = new List<object>();
                    if (query != null)
                    {
                        foreach (var obj in query)
                        {
                            ML.Departamento departamento = new ML.Departamento();
                            departamento.IdDepartamento = obj.IdDepartamento;
                            departamento.Nombre = obj.Nombre;

                            departamento.Area = new ML.Area();
                            departamento.Area.IdArea = obj.IdArea;
                            departamento.Area.Nombre = obj.AreaNombre;

                            result.Objects.Add(departamento);
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

        public static ML.Result Add (ML.Departamento departamento)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.MasterContext context = new DL.MasterContext())
                {
                    int query = context.Database.ExecuteSqlRaw($"DepartamentoAdd '{departamento.Nombre}',{departamento.Area.IdArea}");
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

        public static ML.Result Update (ML.Departamento departamento)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.MasterContext context = new DL.MasterContext())
                {
                    var query = context.Database.ExecuteSqlRaw($"DepartamentoUpdate {departamento.IdDepartamento},'{departamento.Nombre}',{departamento.Area.IdArea}");
                   
                    if (query > 0)
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

        public static ML.Result GetId(int IdDepartamento)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.MasterContext context = new DL.MasterContext())
                {
                    var query = context.Departamentos.FromSqlRaw($"DepartamentoGetId {IdDepartamento}").AsEnumerable().FirstOrDefault();
                    result.Objects = new List<object>();
                    if (query != null)
                    {
                       
                            ML.Departamento departamento = new ML.Departamento();
                            departamento.IdDepartamento = query.IdDepartamento;
                            departamento.Nombre = query.Nombre;

                            departamento.Area = new ML.Area();
                            departamento.Area.IdArea = query.IdArea;
                            departamento.Area.Nombre = query.AreaNombre;

                        result.Object = departamento;
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

        public static ML.Result Delete(ML.Departamento departamento)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.MasterContext context = new DL.MasterContext())
                {
                    var query = context.Database.ExecuteSqlRaw($"DepartamentoDelete {departamento.IdDepartamento}");
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


        public static ML.Result GetByIdArea(int IdArea)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.MasterContext context = new DL.MasterContext())
                {
                    var query = context.Departamentos.FromSqlRaw($"DepartamentoGetByIdArea {IdArea}").ToList();
                    result.Objects = new List<object>();

                    if (query != null)
                    {
                        foreach (var obj in query)
                        {
                            ML.Departamento departamento = new ML.Departamento();
                            departamento.IdDepartamento = obj.IdDepartamento;
                            departamento.Nombre = obj.Nombre;

                            departamento.Area = new ML.Area();
                            departamento.Area.IdArea = obj.IdArea;
                            departamento.Area.Nombre = obj.AreaNombre;
                            

                            result.Objects.Add(departamento);
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

    }
}

