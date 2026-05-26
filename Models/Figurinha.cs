using SQLite;

namespace AppAlbumFigurinhas.Models
{
    public class Figurinha
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string NomeJogador { get; set; }
        public string Selecao { get; set; }
        public string TipoFigurinha { get; set; }
        public bool Obtido { get; set; }
        public bool Desejado { get; set; }
        public string DirImagem { get; set; }

        // Usadas apenas para exibição na tela (não salvas no banco)
        [Ignore]
        public string IconeCheck { get; set; }
        [Ignore]
        public string IconeCoracao { get; set; }
    }
}