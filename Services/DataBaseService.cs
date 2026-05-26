using PCLExt.FileStorage.Folders;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppAlbumFigurinhas.Services
{
    public class DataBaseService
    {
        public SQLiteConnection GetConnection()
        {
            var pasta = new LocalRootFolder();
            var arquivo = pasta.CreateFile("figurinhas_db",
                PCLExt.FileStorage.CreationCollisionOption.OpenIfExists);
            return new SQLiteConnection(arquivo.Path);
        }
    }
}
