using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    public class Turno
    {
        public int IdTurno { get; set; }
        public DateTime Fecha { get; set; }
        public int CapacidadMaxima { get; set; }
        public int Ocupados { get; set; }   // Cantidad socios que asisten, acumulable
        public List<Socio> Socios { get; set; } = new List<Socio>(); // Listado de socios en el turno


        // Método para agregar socio respetando la capacidad máxima
        public bool AgregarSocio(Socio socio)
        {
            if (Socios.Count < CapacidadMaxima)
            {
                Socios.Add(socio);
                Ocupados = Socios.Count; // actualizar Ocupados
                return true;
            }
            return false; // No se pudo agregar
        }
    }
}
