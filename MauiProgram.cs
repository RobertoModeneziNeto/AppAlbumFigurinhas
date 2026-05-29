using Microsoft.Extensions.Logging;

namespace AppAlbumFigurinhas
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            CopiarImagensJogadores();
#if DEBUG
            builder.Logging.AddDebug();
#endif
            return builder.Build();
        }

        static void CopiarImagensJogadores()
        {
            try
            {
                var destino = Path.Combine(AppContext.BaseDirectory, "Imagens");
                if (!Directory.Exists(destino))
                    Directory.CreateDirectory(destino);

                var origem = @"C:\Users\Roberto\Downloads\TODAS_LAS_FIGURITAS";

                if (!Directory.Exists(origem)) return;

                foreach (var arquivo in Directory.GetFiles(origem))
                {
                    var nomeArquivo = Path.GetFileName(arquivo);
                    var caminhoDestino = Path.Combine(destino, nomeArquivo);
                    if (!File.Exists(caminhoDestino))
                        File.Copy(arquivo, caminhoDestino);
                }
            }
            catch { }
        }
    }
}