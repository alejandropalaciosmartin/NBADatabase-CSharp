using CsvHelper;
using CsvHelper.Configuration.Attributes;
using System.Globalization;


namespace NBA_Database.Pages
{
    public partial class Index
    {
        protected override void OnInitialized()
        {
            FileUpload("assets/data/data.csv");
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
        }

        private List<PlayersName> players;
        public class PlayersName
        {
            //player_name,team_abbreviation,age,player_height,player_weight,college,country,draft_year,
            //gp,pts,reb,ast,season
            [Name("player_name")]
            public string PlayerName { get; set; } //jugador
            [Name("team_abbreviation")]
            public string TeamAbbreviation { get; set; } //abreviatura de equipo
            [Name("age")]
            public int Age { get; set; } //edad jugador

            [Name("player_height")]
            public double PlayerHeight { get; set; } //altura
            [Name("player_weight")]
            public double PlayerWeight { get; set; } //peso
            [Name("college")]
            public string College { get; set; } //universidad en la que jugo

            [Name("country")]
            public string Country { get; set; } //pais de procedencia
            [Name("draft_year")]
            public int DraftYear { get; set; } //año que entro en la nba
            [Name("gp")]
            public int Gp { get; set; } //partidos jugados
            [Name("pts")]
            public int Pts { get; set; } //media de puntos esa temporada
            [Name("reb")]
            public int Reb { get; set; } //media de rebotes esa temporada
            [Name("ast")]
            public int Ast { get; set; } //media de asistencias esa temporada
            [Name("season")]
            public int Season { get; set; }  //temporada a la que hace referencia todas las estadísticas anteriores
        }

    }
}