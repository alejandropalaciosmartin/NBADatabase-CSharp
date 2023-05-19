using NBA_Database.Shared;
using System.Net.Http.Json;

namespace NBA_Database.Pages
{
    public partial class Index
    {
        private PlayersName[]? players;
    protected override async Task OnInitializedAsync()
        {
            players = await Http.GetFromJsonAsync<PlayersName[]>("/assets/data/dataPrueba.json");
        }

        public class PlayersName
        {
            //college,country,draft_year,
            //gp,pts,reb,ast,season

            //player_name
            public string Player_name { get; set; }
            //age
            public int Age { get; set; }
            //player_height
            public double Player_height { get; set; }
            //player_weight
            public double Player_weight { get; set; }
            //team_abbreviation
            public string Team_abbreviation { get; set; }


            //Propiedad aplicando una propiedad anterio en un método para sacar solo dos decimales y en metros
            public string Player_heightM => CalcularDecimal(Player_height);
            public string Player_weightG => $"{Player_weight:0.00}";

            //Saca en metros con 2 decimales
            public string CalcularDecimal(double num)
            {
                num /= 100;
                return $"{num:0.00}";
            }
        }


        /*  public string College { get; set; } //universidad en la que jugo

            [JsonPropertyName("country")]
            public string Country { get; set; } //pais de procedencia
            [JsonPropertyName("draft_year")]
            public int DraftYear { get; set; } //año que entro en la nba
            [JsonPropertyName("gp")]
            public int Gp { get; set; } //partidos jugados
            [JsonPropertyName("pts")]
            public int Pts { get; set; } //media de puntos esa temporada
            [JsonPropertyName("reb")]
            public int Reb { get; set; } //media de rebotes esa temporada
            [JsonPropertyName("ast")]
            public int Ast { get; set; } //media de asistencias esa temporada
            [JsonPropertyName("season")]
            public int Season { get; set; }  //temporada a la que hace referencia todas las estadísticas anteriores
        */

    }
}