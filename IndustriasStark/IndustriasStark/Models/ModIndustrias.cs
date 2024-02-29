using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace IndustriasStark.Models
{
    [Table("Funcionários")]
    public class ModIndustrias
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [NotNull]
        public String Nome { get; set; }
        [NotNull]
        public String CPF { get; set; }
        [NotNull]
        public String Telefone { get; set; }
        [NotNull]
        public String Endereco { get; set; }
        [NotNull]
        public String Bairro { get; set; }
        [NotNull]
        public String Complemento { get; set; }
        [NotNull]
        public String Email { get; set; }
        [NotNull]
        public String Setor { get; set; }
        [NotNull]
        public String Turno { get; set; }
        [NotNull]
        public String Dados { get; set; }
        [NotNull]
        public Boolean Favorito { get; set; }
        public byte[] Foto { get; set; }

        public ModIndustrias()
        {
            this.Id = 0;
            this.Nome = "";
            this.CPF = "";
            this.Telefone = "";
            this.Endereco = "";
            this.Bairro = "";
            this.Complemento = "";
            this.Email = "";
            this.Setor = "";
            this.Turno = "";
            this.Dados = "";
            this.Favorito = false;
            this.Foto = null;
        }
    }
}
