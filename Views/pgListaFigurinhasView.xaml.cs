using AppAlbumFigurinhas.Controllers;
using AppAlbumFigurinhas.Models;

namespace AppAlbumFigurinhas.Views;

public partial class pgListaFigurinhasView : ContentPage
{
    FigurinhaController _controller;

    public pgListaFigurinhasView()
    {
        InitializeComponent();
        _controller = new FigurinhaController();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        AtualizarLista();
    }

    void AtualizarLista()
    {
        string nome = txtFiltroNome.Text;
        bool? obtido = chkFiltroObtidas.IsChecked ? true : null;
        bool? desejado = chkFiltroDesejadas.IsChecked ? true : null;

        var lista = _controller.GetFiltrado(nome, obtido, desejado);

        // Adiciona os ícones corretos conforme status
        foreach (var f in lista)
        {
            
            f.IconeCheck = f.Obtido ? "check.png" : "check_off.png";
            f.IconeCoracao = f.Desejado ? "heart.png" : "heart_off.png";
        }

        lsvFigurinhas.ItemsSource = null;
        lsvFigurinhas.ItemsSource = lista;
    }

    private void Filtro_Changed(object sender, EventArgs e)
    {
        AtualizarLista();
    }

    private void tapCheck_Tapped(object sender, TappedEventArgs e)
    {
        if (e.Parameter is Figurinha item)
        {
            item.Obtido = !item.Obtido;
            _controller.Update(item);
            AtualizarLista();
        }
    }

    private void tapCoracao_Tapped(object sender, TappedEventArgs e)
    {
        if (e.Parameter is Figurinha item)
        {
            item.Desejado = !item.Desejado;
            _controller.Update(item);
            AtualizarLista();
        }
    }

    private async void tapDeletar_Tapped(object sender, TappedEventArgs e)
    {
        if (e.Parameter is Figurinha item)
        {
            bool confirmar = await DisplayAlert(
                "Confirmação",
                $"Deseja excluir a figurinha de {item.NomeJogador}?",
                "Sim", "Não");

            if (confirmar)
            {
                _controller.Delete(item);
                AtualizarLista();
            }
        }
    }

    private void btnVoltar_Clicked(object sender, EventArgs e)
    {
        Application.Current.MainPage.Navigation.PopAsync();
    }
}