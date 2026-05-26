using AppAlbumFigurinhas.Controllers;
using AppAlbumFigurinhas.Models;
using AppAlbumFigurinhas.Services;

namespace AppAlbumFigurinhas.Views;

public partial class pgCadFigurinhaView : ContentPage
{
    FigurinhaController _controller;
    string _imgSelecionada = "";

    public pgCadFigurinhaView()
    {
        InitializeComponent();
        _controller = new FigurinhaController();
    }

    private async void btnAdicionarImagem_Clicked(object sender, EventArgs e)
    {
        _imgSelecionada = await ImageService.SelecionarImagem();
        imgFigurinha.Source = _imgSelecionada;
        btnRemoverImagem.IsVisible = true;
    }

    void RemoverImagem()
    {
        imgFigurinha.Source = "";
        _imgSelecionada = "";
        btnRemoverImagem.IsVisible = false;
    }

    private void btnRemoverImagem_Clicked(object sender, EventArgs e)
    {
        RemoverImagem();
    }

    private void btnSalvar_Clicked(object sender, EventArgs e)
    {
        string nome = txtNomeJogador.Text;
        string selecao = txtSelecao.Text;
        string tipo = pkrTipo.SelectedItem?.ToString();

        // Validação dos campos obrigatórios
        if (string.IsNullOrEmpty(nome) ||
            string.IsNullOrEmpty(selecao) ||
            string.IsNullOrEmpty(tipo) ||
            string.IsNullOrEmpty(_imgSelecionada))
        {
            DisplayAlert("Atenção", "Preencha todos os campos obrigatórios, incluindo a foto.", "OK");
            return;
        }

        Figurinha figurinha = new Figurinha();
        figurinha.NomeJogador = nome;
        figurinha.Selecao = selecao;
        figurinha.TipoFigurinha = tipo;
        figurinha.Obtido = chkObtido.IsChecked;
        figurinha.Desejado = chkDesejado.IsChecked;
        figurinha.DirImagem = ImageService.CopiarImagem(_imgSelecionada);

        if (_controller.Insert(figurinha))
        {
            DisplayAlert("Sucesso", "Figurinha cadastrada com sucesso! ", "OK");
            // Limpar campos
            txtNomeJogador.Text = "";
            txtSelecao.Text = "";
            pkrTipo.SelectedIndex = -1;
            chkObtido.IsChecked = false;
            chkDesejado.IsChecked = false;
            RemoverImagem();
        }
        else
        {
            DisplayAlert("Erro", "Ocorreu um erro ao cadastrar a figurinha.", "OK");
        }
    }

    private void btnVoltar_Clicked(object sender, EventArgs e)
    {
        Application.Current.MainPage.Navigation.PopAsync();
    }
}