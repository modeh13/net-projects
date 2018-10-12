namespace CursoBackend.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [Table("Producto")]    
    public partial class Producto
    {
        [XmlElement(ElementName = "Id")]
        [DataMember(Name = "Id")]
        public long Id { get; set; }

        [XmlElement(ElementName = "Nombre")]
        [StringLength(250)]
        public string Nombre { get; set; }

        [XmlElement(ElementName = "Descripcion")]
        [StringLength(500)]
        public string Descripcion { get; set; }

        [XmlElement(ElementName = "Precio")]
        [Column(TypeName = "numeric")]
        public decimal? Precio { get; set; }

        [XmlElement(ElementName = "Imagen")]
        [StringLength(250)]        
        public string Imagen { get; set; }
    }
}