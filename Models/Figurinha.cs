using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppAlbumFigurinhas.Models
{
    public class Figurinha
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string NomeJogador { get; set; }

        public string Selecao { get; set; }

        // "Comum" ou "Especial"
        public string TipoFigurinha { get; set; }

        // true = adquirida, false = não adquirida
        public bool Obtido { get; set; }

        // true = desejada, false = não desejada
        public bool Desejado { get; set; }

        // Diretório da foto da figurinha
        public string DirImagem { get; set; }
    }
}
