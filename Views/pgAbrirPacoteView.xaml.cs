using AppAlbumFigurinhas.Controllers;
using AppAlbumFigurinhas.Models;
using AppAlbumFigurinhas.Services;

namespace AppAlbumFigurinhas.Views;

public partial class pgAbrirPacoteView : ContentPage
{
    FigurinhaController _controller;

    // Guarda a figurinha sorteada e se ela já existe na lista
    Figurinha _figurinhaExistente = null;
    (string Nome, string Selecao, string Tipo) _sorteada;

    public pgAbrirPacoteView()
    {
        InitializeComponent();
        _controller = new FigurinhaController();
        MostrarEstadoPacoteFechado();
    }

    // =============================================
    // ESTADO 1: Pacote fechado
    // =============================================
    void MostrarEstadoPacoteFechado()
    {
        stkPacoteFechado.IsVisible = true;
        stkAnimacao.IsVisible = false;
        stkFigurinhaRevelada.IsVisible = false;
        imgPacote.Scale = 1;
        imgPacote.Opacity = 1;
        imgPacote.Rotation = 0;
    }

    // =============================================
    // Clique em "Abrir"
    // =============================================
    private async void btnAbrir_Clicked(object sender, EventArgs e)
    {
        // Sorteia o jogador
        _sorteada = JogadoresDataService.SortearJogador();

        // Vai para estado de animação
        stkPacoteFechado.IsVisible = false;
        stkAnimacao.IsVisible = true;
        stkFigurinhaRevelada.IsVisible = false;

        // === ANIMAÇÃO do pacote abrindo ===
        // 1) Tremida
        for (int i = 0; i < 5; i++)
        {
            await imgPacoteAbrindo.TranslateTo(-8, 0, 60);
            await imgPacoteAbrindo.TranslateTo(8, 0, 60);
        }
        await imgPacoteAbrindo.TranslateTo(0, 0, 60);

        // 2) Crescer
        await imgPacoteAbrindo.ScaleTo(1.2, 200);

        // 3) Girar e sumir
        await Task.WhenAll(
            imgPacoteAbrindo.RotateTo(15, 150),
            imgPacoteAbrindo.FadeTo(0, 150)
        );

        // Aguarda um breve momento
        await Task.Delay(200);

        // =============================================
        // ESTADO 3: Revelar a figurinha
        // =============================================
        stkAnimacao.IsVisible = false;
        await MostrarFigurinhaRevelada();
    }

    // =============================================
    // Revelar a figurinha sorteada
    // =============================================
    async Task MostrarFigurinhaRevelada()
    {
        // Verifica se já existe no banco pelo nome
        _figurinhaExistente = _controller.GetAll()
            .FirstOrDefault(f => f.NomeJogador.Equals(
                _sorteada.Nome, StringComparison.OrdinalIgnoreCase));

        bool ehNova = _figurinhaExistente == null;

        // Configura o badge
        if (ehNova)
        {
            lblBadge.Text = " FIGURINHA NOVA!";
            frmBadge.BackgroundColor = Color.FromArgb("#FFD700");
            lblBadge.TextColor = Color.FromArgb("#0D6E3F");
        }
        else
        {
            lblBadge.Text = " Figurinha repetida";
            frmBadge.BackgroundColor = Color.FromArgb("#424242");
            lblBadge.TextColor = Color.FromArgb("#BDBDBD");
        }

        // Preenche os dados da figurinha
        lblNomeFigurinha.Text = _sorteada.Nome;
        lblSelecaoFigurinha.Text = "" + _sorteada.Selecao;
        lblTipoFigurinha.Text = "Tipo: " + _sorteada.Tipo;

        // Carrega a imagem — tenta encontrar pelo nome do jogador
        // Carrega a imagem pelo caminho real no disco
        // Em MostrarFigurinhaRevelada()
        string dirImagem = JogadoresDataService.BuscarCaminhoImagem(
            _sorteada.Nome, _sorteada.Selecao, _sorteada.Tipo);

        // Se não encontrou pela busca, tenta pegar do registro existente no banco
        if (string.IsNullOrEmpty(dirImagem) && _figurinhaExistente != null
            && !string.IsNullOrEmpty(_figurinhaExistente.DirImagem))
        {
            dirImagem = _figurinhaExistente.DirImagem;
        }

        if (!string.IsNullOrEmpty(dirImagem))
            imgFigurinhaRevelada.Source = ImageSource.FromFile(dirImagem);
        else
            imgFigurinhaRevelada.Source = null;

        // Configura botões conforme status
        if (ehNova)
        {
            // Figurinha nova: pode adicionar à lista ou só marcar status
            btnAdicionarLista.IsVisible = true;
            btnMarcarAdquirida.IsVisible = true;
            btnMarcarDesejada.IsVisible = true;
        }
        else
        {
            // Já na lista: só altera status ou sai
            btnAdicionarLista.IsVisible = false;

            // Mostra o status atual no botão
            btnMarcarAdquirida.Text = _figurinhaExistente.Obtido
                ? " Já adquirida (clique para desmarcar)"
                : " Marcar como adquirida";

            btnMarcarDesejada.Text = _figurinhaExistente.Desejado
                ? " Já desejada (clique para desmarcar)"
                : " Marcar como desejada";

            btnMarcarAdquirida.IsVisible = true;
            btnMarcarDesejada.IsVisible = true;
        }

        // Mostra o card com animação de entrada
        stkFigurinhaRevelada.IsVisible = true;
        stkFigurinhaRevelada.Opacity = 0;
        stkFigurinhaRevelada.Scale = 0.7;

        await Task.WhenAll(
            stkFigurinhaRevelada.FadeTo(1, 400),
            stkFigurinhaRevelada.ScaleTo(1, 400, Easing.SpringOut)
        );
    }

    // =============================================
    // Botão: Adicionar à lista (figurinha nova)
    // =============================================
    private async void btnAdicionarLista_Clicked(object sender, EventArgs e)
    {
        // Busca o caminho real da imagem no disco
        string dirImagemReal = JogadoresDataService.BuscarCaminhoImagem(
            _sorteada.Nome, _sorteada.Selecao, _sorteada.Tipo);

        var nova = new Figurinha
        {
            NomeJogador = _sorteada.Nome,
            Selecao = _sorteada.Selecao,
            TipoFigurinha = _sorteada.Tipo,
            Obtido = false,
            Desejado = false,
            DirImagem = dirImagemReal  // caminho real do arquivo
        };

        if (_controller.Insert(nova))
        {
            _figurinhaExistente = nova;
            btnAdicionarLista.IsVisible = false;
            btnMarcarAdquirida.IsVisible = true;
            btnMarcarDesejada.IsVisible = true;
            await DisplayAlert("Adicionada!",
                $"{_sorteada.Nome} foi adicionada à sua lista.", "OK");
        }
        else
        {
            await DisplayAlert("Erro",
                "Não foi possível adicionar a figurinha.", "OK");
        }
    }

    // =============================================
    // Botão: Marcar como adquirida (toggle)
    // =============================================
    private async void btnMarcarAdquirida_Clicked(object sender, EventArgs e)
    {
        if (_figurinhaExistente == null)
        {
            await DisplayAlert("Atenção",
                "Adicione a figurinha à lista primeiro.", "OK");
            return;
        }

        _figurinhaExistente.Obtido = !_figurinhaExistente.Obtido;
        _controller.Update(_figurinhaExistente);

        btnMarcarAdquirida.Text = _figurinhaExistente.Obtido
            ? "Já adquirida (clique para desmarcar)"
            : "Marcar como adquirida";

        string msg = _figurinhaExistente.Obtido
            ? "Figurinha marcada como adquirida! "
            : "Figurinha desmarcada como adquirida.";

        await DisplayAlert("Status atualizado", msg, "OK");
    }

    // =============================================
    // Botão: Marcar como desejada (toggle)
    // =============================================
    private async void btnMarcarDesejada_Clicked(object sender, EventArgs e)
    {
        if (_figurinhaExistente == null)
        {
            await DisplayAlert("Atenção",
                "Adicione a figurinha à lista primeiro.", "OK");
            return;
        }

        _figurinhaExistente.Desejado = !_figurinhaExistente.Desejado;
        _controller.Update(_figurinhaExistente);

        btnMarcarDesejada.Text = _figurinhaExistente.Desejado
            ? "Já desejada (clique para desmarcar)"
            : "Marcar como desejada";

        string msg = _figurinhaExistente.Desejado
            ? "Figurinha adicionada à lista de desejos! "
            : "Figurinha removida da lista de desejos.";

        await DisplayAlert("Status atualizado", msg, "OK");
    }

    // =============================================
    // Botão: Abrir outro pacote (reseta tela)
    // =============================================
    private void btnAbrirOutro_Clicked(object sender, EventArgs e)
    {
        _figurinhaExistente = null;
        imgPacoteAbrindo.Opacity = 1;
        imgPacoteAbrindo.Scale = 1;
        imgPacoteAbrindo.Rotation = 0;
        imgPacoteAbrindo.TranslationX = 0;
        imgPacoteAbrindo.TranslationY = 0;
        MostrarEstadoPacoteFechado();
    }

    // =============================================
    // Botão: Voltar
    // =============================================
    private void btnVoltar_Clicked(object sender, EventArgs e)
    {
        Application.Current.MainPage.Navigation.PopAsync();
    }
}