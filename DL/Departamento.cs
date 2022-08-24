using System;
using System.Collections.Generic;

namespace DL
{
    public partial class Departamento
    {
        public Departamento()
        {
            Productos = new HashSet<Producto>();
        }

        public int IdDepartamento { get; set; }
        public string Nombre { get; set; } = null!;
        public int IdArea { get; set; }
        public string AreaNombre { get; set; }

        public virtual Area IdAreaNavigation { get; set; } = null!;
        public virtual ICollection<Producto> Productos { get; set; }
    }
}
