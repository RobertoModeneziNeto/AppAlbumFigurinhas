using AppAlbumFigurinhas.Controllers;
using AppAlbumFigurinhas.Models;
using AppAlbumFigurinhas.Services;

namespace AppAlbumFigurinhas.Views;

public partial class pgEditarFigurinhaView : ContentPage
{
    FigurinhaController _controller;
    Figurinha _figurinha;
    string _imgSelecionada = "";

    // Recebe a figurinha a ser editada
    public pgEditarFigurinhaView(Figurinha figurinha)
    {
        InitializeComponent();
        _controller = new FigurinhaController();
        _figurinha = figurinha;
        PreencherCampos();
    }

    // Preenche os campos com os dados atuais da figurinha
    void PreencherCampos()
    {
        txtNomeJogador.Text = _figurinha.NomeJogador;
        txtSelecao.Text = _figurinha.Selecao;

        pkrTipo.SelectedIndex = _figurinha.TipoFigurinha == "Especial" ? 1 : 0;

        chkObtido.IsChecked = _figurinha.Obtido;
        chkDesejado.IsChecked = _figurinha.Desejado;

        if (!string.IsNullOrEmpty(_figurinha.DirImagem))
        {
            _imgSelecionada = _figurinha.DirImagem;
            imgFigurinha.Source = ImageSource.FromFile(_figurinha.DirImagem);
            btnRemoverImagem.IsVisible = true;
        }
    }

    private async void btnAlterarImagem_Clicked(object sender, EventArgs e)
    {
        _imgSelecionada = await ImageService.SelecionarImagem();
        if (!string.IsNullOrEmpty(_imgSelecionada))
        {
            imgFigurinha.Source = ImageSource.FromFile(_imgSelecionada);
            btnRemoverImagem.IsVisible = true;
        }
    }

    private void btnRemoverImagem_Clicked(object sender, EventArgs e)
    {
        imgFigurinha.Source = null;
        _imgSelecionada = "";
        btnRemoverImagem.IsVisible = false;
    }

    private void btnSalvar_Clicked(object sender, EventArgs e)
    {
        string nome = txtNomeJogador.Text;
        string selecao = txtSelecao.Text;
        string tipo = pkrTipo.SelectedItem?.ToString();

        if (string.IsNullOrEmpty(nome) ||
            string.IsNullOrEmpty(selecao) ||
            string.IsNullOrEmpty(tipo))
        {
            DisplayAlert("Atenção", "Preencha todos os campos obrigatórios.", "OK");
            return;
        }

        _figurinha.NomeJogador = nome;
        _figurinha.Selecao = selecao;
        _figurinha.TipoFigurinha = tipo;
        _figurinha.Obtido = chkObtido.IsChecked;
        _figurinha.Desejado = chkDesejado.IsChecked;

        // Só atualiza a imagem se o usuário alterou
        if (!string.IsNullOrEmpty(_imgSelecionada)
            && _imgSelecionada != _figurinha.DirImagem)
        {
            _figurinha.DirImagem = ImageService.CopiarImagem(_imgSelecionada);
        }
        else if (string.IsNullOrEmpty(_imgSelecionada))
        {
            _figurinha.DirImagem = "";
        }

        if (_controller.Update(_figurinha))
        {
            DisplayAlert("Sucesso", "Figurinha atualizada com sucesso! ", "OK");
            Application.Current.MainPage.Navigation.PopAsync();
        }
        else
        {
            DisplayAlert("Erro", "Não foi possível atualizar a figurinha.", "OK");
        }
    }

    private void btnVoltar_Clicked(object sender, EventArgs e)
    {
        Application.Current.MainPage.Navigation.PopAsync();
    }
}