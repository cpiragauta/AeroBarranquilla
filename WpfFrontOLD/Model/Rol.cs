//------------------------------------------------------------------------------
// <auto-generated>
//    Este código se generó a partir de una plantilla.
//
//    Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//    Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WpfFront.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Rol
    {
        public Rol()
        {
            this.MenuRol = new HashSet<MenuRol>();
            this.Usuario = new HashSet<Usuario>();
        }
    
        public int RowID { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string UsuarioCreacion { get; set; }
        public Nullable<System.DateTime> FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
    
        public virtual ICollection<MenuRol> MenuRol { get; set; }
        public virtual ICollection<Usuario> Usuario { get; set; }
    }
}
