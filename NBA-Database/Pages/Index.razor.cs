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
            //college
            public string College { get; set; }
            //country
            public string Country { get; set; }
            //draft_year
            public int Draft_year { get; set; }
            //gp
            public int Gp { get; set; }
            //pts
            public double Pts { get; set; }
            //reb
            public double Reb { get; set; }
            //ast
            public double Ast { get; set; }
            //season
            public string Season { get; set; }


            //Propiedad nueva aplicando una propiedad anterior con un método para sacar solo dos decimales y en metros
            public string Player_heightM => CalcularDecimal(Player_height);
            //En este caso directamente meterlo en un string y ponerle dos decimales porque ya está transformado el valor
            public string Player_weightG => $"{Player_weight:0.00}";

            //Saca en metros con 2 decimales
            public string CalcularDecimal(double num)
            {
                num /= 100;
                return $"{num:0.00}";
            }

            //Para que aparezca la imagen del equipo 
            public string daImagen()
            {
                    string imagen = "";
                    string directorio = @"/assets/image/equipos";
                try
                {
                    string team = $"{Team_abbreviation}.png";

                    if (Path.Exists(directorio))
                    {
                        string[] archivos = Directory.GetFiles(directorio, "*.png");

                        foreach (var dir in archivos)
                        {
                            if (dir == team)
                            {
                                imagen = $"/assets/image/equipos/{Team_abbreviation}.png";
                            }
                            else
                            {
                                imagen = $"/assets/image/mundo.png";
                            }

                        }

                    }
                    else
                    {
                        Console.WriteLine("No existe");
                    }
                    //    string imagen = "";
                    //if ($"/assets/image/equipos/{Team_abbreviation}.png" == $"/assets/image/equipos/*.png")
                    //{
                    //    imagen = $"/assets/image/equipos/{Team_abbreviation}.png";
                    //}
                    //else
                    //{
                    //    imagen = $"/assets/image/equipos/mundo.png";
                    //}
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return imagen;
            }
            public string daImagenPais() => $"/assets/image/equipos/{Country}.png";
        }
    }
}