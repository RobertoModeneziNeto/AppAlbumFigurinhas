using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppAlbumFigurinhas.Services
{
    public static class ImageService
    {
        public static async Task<string> SelecionarImagem()
        {
            string diretorio = "";
            var imgSelecionada = await MediaPicker.Default.PickPhotoAsync();
            if (imgSelecionada != null)
                diretorio = imgSelecionada.FullPath;
            return diretorio;
        }

        public static string CopiarImagem(string dirOriginal)
        {
            string dirDestino = "";
            if (!string.IsNullOrEmpty(dirOriginal))
            {
                var dirNovo = Path.Combine(AppContext.BaseDirectory, "Imagens");
                if (!Directory.Exists(dirNovo))
                    Directory.CreateDirectory(dirNovo);

                string nomeOriginal = Path.GetFileName(dirOriginal);
                dirDestino = Path.Combine(dirNovo, nomeOriginal);
                File.Copy(dirOriginal, dirDestino, overwrite: true);
            }
            return dirDestino;
        }
    }
}
