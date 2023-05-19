using System;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace NBA_Database.Pages
{
    public partial class Index
    {
        private PlayersName[]? forecasts;

        protected override async Task OnInitializedAsync()
        {
            forecasts = await Http.GetFromJsonAsync<PlayersName[]>("assets/data/data.json");
        }

        public class PlayersName
        {
            //player_weight,college,country,draft_year,
            //gp,pts,reb,ast,season
            //player_name
            public string Player_name { get; set; }
            //age
            public int Age { get; set; }
            //player_height
            public double Player_height { get; set; }
            //team_abbreviation
            public string Team_abbreviation { get; set; }
            //Propiedad aplicando una propiedad anterio en un método para sacar solo dos decimales y en metros
            public string Player_heightM => CalcularDecimal(Player_height);

            //Saca en metros con 2 decimales
            public string CalcularDecimal(double num)
            {
                num /= 100;
                return $"{num:0.00}";
            }
        }
        /*
            
            [JsonPropertyName("team_abbreviation")]
            public string TeamAbbreviation { get; set; } //
            [JsonPropertyName("age")]
            public int Age { get; set; } //edad jugador

            [JsonPropertyName("player_height")]
            public double PlayerHeight { get; set; } //altura
            [JsonPropertyName("player_weight")]
            public double PlayerWeight { get; set; } //peso
            [JsonPropertyName("college")]
            public string College { get; set; } //universidad en la que jugo

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