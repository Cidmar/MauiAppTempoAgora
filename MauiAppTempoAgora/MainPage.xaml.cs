using MauiAppTempoAgora.Models;

namespace MauiAppTempoAgora
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private async void btn_buscar_Clicked(object sender, EventArgs e)
        {
            {
                try
                {
                    if (!string.IsNullOrEmpty(txt_cidade.Text))
                    {
                        Tempo? t = await Services.DataService.GetPrevisao(txt_cidade.Text);
                        
                        if (t != null)
                        {
                            // Parte 1: Exibindo descrição, vento e visibilidade
                            string dados_previsao = $"\nLatitude: {t.lat}" +
                                                    $"\nLongitude: {t.lon}" +
                                                    $"\nDescrição: {t.description}" +
                                                    $"\nTemperatura mínima: {t.temp_min} °C" +
                                                    $"\nTemperatura máxima: {t.temp_max} °C" +
                                                    $"\nVelocidade do vento: {t.speed} m/s" +
                                                    $"\nVisibilidade: {t.visibility} metros" +
                                                    $"\nNascer do sol: {t.sunrise}" +
                                                    $"\nPôr do sol: {t.sunset}";

                            lbl_resultado.Text = dados_previsao;
                        }
                    }
                    else
                    {
                        lbl_resultado.Text = "Preencha o campo cidade";
                    }
                }
                catch (Exception ex)
                {
                    // Parte 2: Captura os erros e exibe as mensagens amigáveis
                    if (ex.Message == "CidadeNaoEncontrada")
                    {
                        await DisplayAlertAsync("Cidade não encontrada", "A cidade informada não foi localizada. Verifique e tente novamente.", "OK");
                        lbl_resultado.Text = "Cidade não encontrada.";
                    }
                    else if (ex.Message == "SemConexao")
                    {
                        await DisplayAlertAsync("Sem Conexão", "Você está sem acesso à internet. Verifique sua conexão e tente novamente.", "OK");
                        lbl_resultado.Text = "Sem conexão com a internet.";
                    }
                    else
                    {
                        await DisplayAlertAsync("Erro", $"Ocorreu um erro: {ex.Message}", "OK");
                    }
                }
            }
        }
    }
}