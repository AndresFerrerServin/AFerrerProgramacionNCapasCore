using Microsoft.EntityFrameworkCore;

namespace BL
{
    public class Usuario
    {
        public static ML.Result Add(ML.Usuario usuario)
        {
             ML.Result result = new ML.Result();
            try
            {
                using (DL.MasterContext context = new DL.MasterContext())
                {
                    var query = context.Database.ExecuteSqlRaw($"UsuarioAdd '{usuario.UserName}','{usuario.Nombre}'" +
                        $"                                                      ,'{usuario.ApellidoPaterno}','{usuario.ApellidoMaterno}'" +
                        $"                                                      ,'{usuario.Email}','{usuario.Password}','{usuario.FechaNacimiento}'" +
                        $"                                                      ,'{usuario.Sexo}','{usuario.Telefono}','{usuario.Celular}','{usuario.CURP}'" +
                        $"                                                      ,'{usuario.Imagen}',{usuario.Rol.IdRol},{usuario.Estatus} " +
                        $"                                                      ,'{usuario.Direccion.Calle}','{usuario.Direccion.NumeroInterior}'" +
                        $"                                                      ,'{usuario.Direccion.NumeroExterior}',{usuario.Direccion.Colonia.IdColonia}");
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

        public static ML.Result Update(ML.Usuario usuario)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.MasterContext context = new DL.MasterContext())
                {
                    var query = context.Database.ExecuteSqlRaw($"UsuarioUpdate {usuario.IdUsuario},'{usuario.UserName}','{usuario.Nombre}'" +
                        $"                                                      ,'{usuario.ApellidoPaterno}','{usuario.ApellidoMaterno}'" +
                        $"                                                      ,'{usuario.Email}','{usuario.Password}','{usuario.FechaNacimiento}'" +
                        $"                                                      ,'{usuario.Sexo}','{usuario.Telefono}','{usuario.Celular}','{usuario.CURP}'" +
                        $"                                                      ,'{usuario.Imagen}',{usuario.Rol.IdRol},{usuario.Estatus} ,{usuario.Direccion.IdDireccion}" +
                        $"                                                      ,'{usuario.Direccion.Calle}','{usuario.Direccion.NumeroInterior}'" +
                        $"                                                      ,'{usuario.Direccion.NumeroExterior}',{usuario.Direccion.Colonia.IdColonia}");
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

        public static ML.Result Delete(ML.Usuario usuario)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.MasterContext context = new DL.MasterContext())
                {
                    var query = context.Database.ExecuteSqlRaw($"UsuarioDelete {usuario.IdUsuario}");
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

        public static ML.Result GetAll(ML.Usuario usuario)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.MasterContext context = new DL.MasterContext())
                {
                    usuario.Nombre = (usuario.Nombre == null) ? "" : usuario.Nombre;
                    usuario.ApellidoPaterno = (usuario.ApellidoPaterno ==null)? "":usuario.ApellidoPaterno;
                    usuario.ApellidoMaterno = (usuario.ApellidoMaterno == null) ? "" : usuario.ApellidoMaterno;

                    var query = context.Usuarios.FromSqlRaw($"UsuarioGetAll '{usuario.Nombre}','{usuario.ApellidoPaterno}','{usuario.ApellidoMaterno}'").ToList();
                    result.Objects = new List<object>();
                    if (query != null)
                    {
                        foreach (var obj in query)
                        {
                            usuario = new ML.Usuario();
                            usuario.IdUsuario = obj.IdUsuario;
                            usuario.UserName = obj.UserName;
                            usuario.Nombre = obj.Nombre;
                            usuario.ApellidoPaterno = obj.ApellidoPaterno;
                            usuario.ApellidoMaterno = obj.ApellidoMaterno;
                            usuario.Email = obj.Email;
                            usuario.Password = obj.Password;
                            usuario.FechaNacimiento = obj.FechaNacimiento.ToString("dd-MM-yyyy");
                            usuario.Sexo = obj.Sexo;
                            usuario.Telefono = obj.Telefono;
                            usuario.Celular = obj.Celular;
                            usuario.CURP = obj.Curp;
                            usuario.Imagen = obj.Imagen;

                            usuario.Rol = new ML.Rol();
                            usuario.Rol.IdRol = obj.IdRol;
                            usuario.Rol.Nombre = obj.RolNombre;
                            usuario.Estatus = obj.Estatus.Value;

                            usuario.Direccion = new ML.Direccion();
                            usuario.Direccion.IdDireccion = int.Parse(obj.IdDireccion.ToString());
                            usuario.Direccion.Calle = obj.Calle;
                            usuario.Direccion.NumeroInterior = obj.NumeroInterior;
                            usuario.Direccion.NumeroExterior = obj.NumeroExterior;

                            usuario.Direccion.Colonia = new ML.Colonia();
                            usuario.Direccion.Colonia.IdColonia = int.Parse(obj.IdColonia.ToString());
                            usuario.Direccion.Colonia.Nombre = obj.ColoniaNombre;

                            usuario.Direccion.Colonia.Municipio = new ML.Municipio();
                            usuario.Direccion.Colonia.Municipio.IdMunicipio = int.Parse(obj.IdMunicipio.ToString());
                            usuario.Direccion.Colonia.Municipio.Nombre = obj.MunicipioNombre;

                            usuario.Direccion.Colonia.Municipio.Estado = new ML.Estado();
                            usuario.Direccion.Colonia.Municipio.Estado.IdEstado = int.Parse(obj.IdEstado.ToString());
                            usuario.Direccion.Colonia.Municipio.Estado.Nombre = obj.EstadoNombre;

                            usuario.Direccion.Colonia.Municipio.Estado.Pais = new ML.Pais();
                            usuario.Direccion.Colonia.Municipio.Estado.Pais.IdPais = int.Parse(obj.IdPais.ToString());
                            usuario.Direccion.Colonia.Municipio.Estado.Pais.Nombre = obj.PaisNombre;


                            result.Objects.Add(usuario);                           
                            
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

        public static ML.Result GetId(int IdUsuario)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.MasterContext context = new DL.MasterContext())
                {
                    var query = context.Usuarios.FromSqlRaw($"UsuarioGetId {IdUsuario} ").AsEnumerable().FirstOrDefault();
                    result.Objects = new List<object>();
                    if (query != null)
                    {

                        ML.Usuario usuario = new ML.Usuario();
                        usuario.IdUsuario = query.IdUsuario;
                        usuario.UserName = query.UserName;
                        usuario.Nombre = query.Nombre;
                        usuario.ApellidoPaterno = query.ApellidoPaterno;
                        usuario.ApellidoMaterno = query.ApellidoMaterno;
                        usuario.Email = query.Email;
                        usuario.Password = query.Password;
                        usuario.FechaNacimiento = query.FechaNacimiento.ToString("dd-MM-yyyy");
                        usuario.Sexo = query.Sexo;
                        usuario.Telefono = query.Telefono;
                        usuario.Celular = query.Celular;
                        usuario.CURP = query.Curp;
                        usuario.Imagen = query.Imagen;

                        usuario.Rol = new ML.Rol();
                        usuario.Rol.IdRol = query.IdRol;
                        usuario.Estatus = query.Estatus.Value;

                        usuario.Direccion = new ML.Direccion();
                        usuario.Direccion.IdDireccion = query.IdDireccion;
                        usuario.Direccion.Calle = query.Calle;
                        usuario.Direccion.NumeroInterior = query.NumeroInterior;
                        usuario.Direccion.NumeroExterior = query.NumeroExterior;

                        usuario.Direccion.Colonia = new ML.Colonia();
                        usuario.Direccion.Colonia.IdColonia = query.IdColonia;
                        usuario.Direccion.Colonia.Nombre = query.ColoniaNombre;

                        usuario.Direccion.Colonia.Municipio = new ML.Municipio();
                        usuario.Direccion.Colonia.Municipio.IdMunicipio = query.IdMunicipio;
                        usuario.Direccion.Colonia.Municipio.Nombre = query.MunicipioNombre;

                        usuario.Direccion.Colonia.Municipio.Estado = new ML.Estado();
                        usuario.Direccion.Colonia.Municipio.Estado.IdEstado = query.IdEstado;
                        usuario.Direccion.Colonia.Municipio.Estado.Nombre = query.EstadoNombre;

                        usuario.Direccion.Colonia.Municipio.Estado.Pais = new ML.Pais();
                        usuario.Direccion.Colonia.Municipio.Estado.Pais.IdPais = query.IdPais;
                        usuario.Direccion.Colonia.Municipio.Estado.Pais.Nombre = query.PaisNombre;
                        result.Object = usuario;
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
    public static ML.Result GetByEmeil(string Email)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.MasterContext context = new DL.MasterContext())
                {
                    var query = context.Usuarios.FromSqlRaw($"UsuarioGetEmail '{Email}'").AsEnumerable().FirstOrDefault();
                    if (query != null)
                    {
                        ML.Usuario usuario = new ML.Usuario();
                        usuario.IdUsuario = query.IdUsuario;
                        usuario.UserName = query.UserName;
                        usuario.Nombre = query.Nombre;
                        usuario.ApellidoPaterno = query.ApellidoPaterno;
                        usuario.ApellidoMaterno = query.ApellidoMaterno;
                        usuario.Email = query.Email;
                        usuario.Password = query.Password;
                        usuario.FechaNacimiento = query.FechaNacimiento.ToString("dd-MM-yyyy");
                        usuario.Sexo = query.Sexo;
                        usuario.Telefono = query.Telefono;
                        usuario.Celular = query.Celular;
                        usuario.CURP = query.Curp;
                        usuario.Imagen = query.Imagen;

                        usuario.Rol = new ML.Rol();
                        usuario.Rol.IdRol = query.IdRol;
                        usuario.Estatus = query.Estatus.Value;

                        usuario.Direccion = new ML.Direccion();
                        usuario.Direccion.IdDireccion = query.IdDireccion;
                        usuario.Direccion.Calle = query.Calle;
                        usuario.Direccion.NumeroInterior = query.NumeroInterior;
                        usuario.Direccion.NumeroExterior = query.NumeroExterior;

                        usuario.Direccion.Colonia = new ML.Colonia();
                        usuario.Direccion.Colonia.IdColonia = query.IdColonia;
                        usuario.Direccion.Colonia.Nombre = query.ColoniaNombre;

                        usuario.Direccion.Colonia.Municipio = new ML.Municipio();
                        usuario.Direccion.Colonia.Municipio.IdMunicipio = query.IdMunicipio;
                        usuario.Direccion.Colonia.Municipio.Nombre = query.MunicipioNombre;

                        usuario.Direccion.Colonia.Municipio.Estado = new ML.Estado();
                        usuario.Direccion.Colonia.Municipio.Estado.IdEstado = query.IdEstado;
                        usuario.Direccion.Colonia.Municipio.Estado.Nombre = query.EstadoNombre;

                        usuario.Direccion.Colonia.Municipio.Estado.Pais = new ML.Pais();
                        usuario.Direccion.Colonia.Municipio.Estado.Pais.IdPais = query.IdPais;
                        usuario.Direccion.Colonia.Municipio.Estado.Pais.Nombre = query.PaisNombre;
                        result.Object = usuario;
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