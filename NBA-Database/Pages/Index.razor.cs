using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace NBA_Database.Pages
{
    public partial class Index
    {
        /*private async Task FileUpload(string file)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var rutaArchivo = "assets/data/data.csv";
                    var response = await httpClient.GetAsync(rutaArchivo);
                    var csvR = await response.Content.ReadAsStringAsync();

                    using (var reader = new StringReader(csvR))
                    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                    {
                        players = csv.GetRecords<PlayersName>().ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }*/
        /*protected override void OnInitialized()
        {
            FileUpload(@"assets/data/data.csv");
        }

        private void FileUpload(string file)
        {
            try
            {
                using StreamReader sr = new StreamReader(file);
                using (CsvReader csvR = new CsvReader(sr, CultureInfo.InvariantCulture))
                {
                    players = csvR.GetRecords<PlayersName>().ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }*/

        //private List<PlayersName> players;
        private PlayersName[]? players;

        protected override async Task OnInitializedAsync()
        {
            players = await Http.GetFromJsonAsync<PlayersName[]>("assets/data/data.json");
        }
        public class PlayersName
        {
            //player_name,team_abbreviation,age,player_height,player_weight,college,country,draft_year,
            //gp,pts,reb,ast,season
            [JsonPropertyName("player_name")]
            public string PlayerName { get; set; } //jugador
            [JsonPropertyName("team_abbreviation")]
            public string TeamAbbreviation { get; set; } //abreviatura de equipo
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
        }

    }
}