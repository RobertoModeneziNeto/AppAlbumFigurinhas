using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppAlbumFigurinhas.Models;
using AppAlbumFigurinhas.Services;

namespace AppAlbumFigurinhas.Controllers
{
    public class FigurinhaController
    {
        DataBaseService _dataBaseService;
        SQLiteConnection _connection;

        public FigurinhaController()
        {
            _dataBaseService = new DataBaseService();
            _connection = _dataBaseService.GetConnection();
            _connection.CreateTable<Figurinha>();
        }

        public bool Insert(Figurinha value)
        {
            return _connection.Insert(value) > 0;
        }

        public bool Update(Figurinha value)
        {
            return _connection.Update(value) > 0;
        }

        public bool Delete(Figurinha value)
        {
            return _connection.Delete(value) > 0;
        }

        public List<Figurinha> GetAll()
        {
            return _connection.Table<Figurinha>().ToList();
        }

        // Filtro combinado: nome + obtido + desejado
        public List<Figurinha> GetFiltrado(string nome, bool? obtido, bool? desejado)
        {
            var query = _connection.Table<Figurinha>();
            var lista = query.ToList();

            if (!string.IsNullOrEmpty(nome))
                lista = lista.Where(f =>
                    f.NomeJogador.Contains(nome,
                    StringComparison.OrdinalIgnoreCase)).ToList();

            if (obtido.HasValue)
                lista = lista.Where(f => f.Obtido == obtido.Value).ToList();

            if (desejado.HasValue)
                lista = lista.Where(f => f.Desejado == desejado.Value).ToList();

            return lista;
        }

        public Figurinha GetById(int id)
        {
            return _connection.Find<Figurinha>(id);
        }
    }
}
