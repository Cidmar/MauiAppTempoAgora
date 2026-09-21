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
                            string dados_previsao = "";

                            dados_previsao = $"\nLatitude: " + t.lat +
                                              $"\nLongitude: " + t.lon +
                                              $"\nDescrição: " + t.description +
                                              $"\nTemperatura mínima: " + t.temp_min + " °C" +
                                              $"\nTemperatura máxima: " + t.temp_max + " °C" +
                                              $"\nVelocidade do vento: " + t.speed + " m/s" +
                                              $"\nVisibilidade: " + t.visibility + " metros" +
                                              $"\nNascer do sol: " + t.sunrise +
                                              $"\nPôr do sol: " + t.sunset;

                            lbl_resultado.Text = dados_previsao;
                        }

                        else
                        {
                            lbl_resultado.Text = "Não foi possível obter a previsão do tempo.";
                        }
                    }
                    else
                    {
                        lbl_resultado.Text = "Preencha o campo cidade";
                    }

                }
                catch (Exception ex)
                {
                    await DisplayAlertAsync("Erro", ex.Message, "OK");
                }

            }
        }
    }
}
