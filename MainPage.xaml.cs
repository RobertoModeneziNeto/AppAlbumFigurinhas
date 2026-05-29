using AppAlbumFigurinhas.Views;

namespace AppAlbumFigurinhas
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void btnCadastrar_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage.Navigation.PushAsync(new pgCadFigurinhaView());
        }

        private void btnLista_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage.Navigation.PushAsync(new pgListaFigurinhasView());
        }

        private void btnAbrirPacote_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage.Navigation.PushAsync(new pgAbrirPacoteView());

        }
    }
}