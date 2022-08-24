using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL_C
{
    public class Producto
    {
        public static ML.Result CargaMasiva()
        {
            ML.Result result = new ML.Result();
            ML.Result resultErrores = new ML.Result();
            resultErrores.Objects = new List<object>();

            string file = @"C:\Users\digis\Documents\Andres Ferrer ServinVS\AFerrerProgramacionNCapasCore\LayOutProducto.txt";

            StreamReader archivo = new StreamReader(file);
            string line;
            bool isFirstLine = true;
            while ((line = archivo.ReadLine()) != null)
            {
                if (isFirstLine)
                {
                    isFirstLine = false;
                    line = archivo.ReadLine();
                }

                Console.WriteLine(line);
                string[] datos = line.Split('|');
                ML.Producto producto = new ML.Producto();
                producto.Nombre = datos[0];
                producto.PrecioUnitario = decimal.Parse(datos[1]);
                producto.Stock = int.Parse(datos[2]);
                producto.Descripcion = datos[3];
                producto.Proveedor = new ML.Proveedor();
                producto.Proveedor.IdProveedor = int.Parse(datos[4]);
                producto.Departamento = new ML.Departamento();
                producto.Departamento.IdDepartamento = int.Parse(datos[5]);
                producto.Imagen = null;

                result = BL.Producto.Add(producto);

                if (!result.Correct)
                {
                    resultErrores.Objects.Add("No se inserto el Nombre" + producto.Nombre +
                                              "No se inserto el PrecioUnitario" + producto.PrecioUnitario +
                                              "No se inserto el Stock" + producto.Stock +
                                              "No se inserto la Descripcion" + producto.Descripcion +
                                              "No se inserto el Proveedor" + producto.Proveedor.IdProveedor +
                                              "No se inserto el Departamento" + producto.Departamento.IdDepartamento);

                }

            }
            if (resultErrores.Objects != null)
            {

            }

            TextWriter tw = new StreamWriter(@"C:\Users\digis\Documents\Andres Ferrer ServinVS\AFerrerProgramacionNCapasCore\Errores.txt");
            foreach (string error in resultErrores.Objects)
            {
                tw.WriteLine(error); //Se le concatenan todos los errores
            }
            tw.Close();


            return result;
        }
    }
}
