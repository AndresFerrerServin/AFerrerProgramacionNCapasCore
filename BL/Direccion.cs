using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Direccion
    {
        public static ML.Result GetAll () 
        {
            ML.Result result = new ML.Result ();
            try
            {
                using (DL.MasterContext context = new DL.MasterContext())
                {
                    var query = context.Direccions.FromSqlRaw($"DireccionGetAll").ToList();
                    result.Objects = new List<object>();
                    if (query != null)
                    {
                        foreach (var obj in query)
                        {
                            ML.Direccion direccion = new ML.Direccion();
                            direccion.IdDireccion = obj.IdDireccion;
                            direccion.Calle = obj.Calle;
                            direccion.NumeroInterior = obj.NumeroInterior;
                            direccion.NumeroExterior = obj.NumeroExterior;

                            direccion.Colonia = new ML.Colonia();
                            direccion.Colonia.IdColonia = obj.IdColonia;
                            direccion.Colonia.Nombre = obj.ColoniaNombre; 

                            direccion.Usuario = new ML.Usuario();
                            direccion.Usuario.IdUsuario = obj.IdUsuario;
                            direccion.Usuario.Nombre = obj.UsuarioNombre;

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
    }
}
