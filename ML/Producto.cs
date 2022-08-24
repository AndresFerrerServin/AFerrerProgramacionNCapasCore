using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Producto
    {
        public int IdProducto { get; set; }

        [Required (ErrorMessage = "Coloque el producto")]
        public string Nombre { get; set; }

        [Range (1,1000)]
        public decimal PrecioUnitario { get; set; }

        [Range(1,20)]
        public int Stock { get; set; }

        [Required]
        public string Descripcion { get; set; }
        public string? Imagen { get; set; }

        [Required]
        public ML.Proveedor Proveedor { get; set; }

        [Required]
        public ML.Departamento Departamento { get; set; }
        public List<object> Productos { get; set; }

        public int Cantidad { get; set; }
        public decimal Subtotal { get; set; }
    }
}
