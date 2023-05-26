using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using static NBA_Database.Pages.Compare;
using static NBA_Database.Pages.Index;

namespace NBA_Database.Pages;

//Para leer el archivo json 
public partial class Index
{
    //Metemos en una raíz porque el tamaño no cambiará de PlayersName
    private List<PlayersName> players;
    private List<PlayersName> players2;
    private List<PlayerLink> playersLinks;

    //Leemos y pasamos a players el contenido de json
    protected override async Task OnInitializedAsync()
    {
        players = await Http.GetFromJsonAsync<List<PlayersName>>("/assets/data/datas.json");
        playersLinks = await Http.GetFromJsonAsync<List<PlayerLink>>("/assets/data/players.json");
        //CREAMOS NUEVA VARIABLE para reiniciar los datos antes de la búsqueda
        players2 = players;
        //LIMITECONTADOR = players.Count / 10; //El total de json lo dividimos entre 10
    }
    public class PlayerLink
    {
        public string Name { get; set; }
        public string Link { get; set; }
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
        public string Draft_year { get; set; }
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

        //public string DaImagenPais() => $"/assets/image/equipos/{Country}.png";
        public string DaImagenGeneral(string buscaBandera)
        {
            //Imagen de salida que la obtenemos en el foreach del Index.razor al llamar al método
            string imagen = "";
            //Creamos array con las banderas que tenemos la imagen
            string[] imagenExiste = { "CHI", "BOS", "BKN", "NYK", "PHI", "TOR", "CLE", "DET", "IND", "MIL", "DEN", "MIN", "OKC", "POR", "UTA", "GSW", "LAC", "LAL", "PHX", "SAC", "ATL", "CHA", "DAL", "MIA", "MEM", "WAS", "SAS", "NOP", "HOU", "ORL",
                "USA","Bahamas","Canada","Croatia","Democratic Republic of the Congo","France","Germany","Greece","Lithuania","Mali","Puerto Rico","Senegal","Serbia","Slovenia","Turkey","Brazil", "Latvia","Australia","Italy","Sweden","Ukraine","Austria",
                "Egypt", "Spain","Dominican Republic","Cameroon","New Zealand","South Sudan", "Czech Republic","Russia","Poland","Argentina","United Kingdom","Haiti" ,"China","Bosnia and Herzegovina","Georgia", "Switzerland"};
            try
            {
                //Recorremos el array de las fotos existentes
                for (int i = 0; i < imagenExiste.Length; i++)
                {
                    //Miramos si el valor del json que leemos lo contiene el array, si lo contiene lo muestra y sino muestra una predeterminada
                    if (imagenExiste.Contains(buscaBandera))
                    {
                        imagen = $"/assets/image/equiposNBA/{buscaBandera}.png";
                    }
                    else
                    {
                        imagen = $"/assets/image/mundo.png";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return imagen;
        }

///////////////////////////PAGINACIÓN//////////////////////////////////////////////////////////////////////////
    }
    private int elementosPagina = 7; // Número de elementos por página
    private int paginaActual = 1; // Página actual
    //Función para mostrar y paginar
    public List<PlayersName> ConseguirJugadoresPaginacion()
    {
        int primerElemento = (paginaActual - 1) * elementosPagina; //A la página actual le quito 1 para que al multiplicar
                                                       //por 1 se ponga 0 que al multiplicar te da 0, 1º posición
                                                       //Para obtener el siguiente grupo sería 2 que se suma abajo
                                                       //2 menos 1 da 1 que es la 2º pagina por el tamaño que son 7
                                                       //por lo que cogemos la siguiente posicion que empezamos que es
                                                       //el 7, ya que antes eran del 0 al 6 (7 posiciones)
                                                       //el siguiente del 7 al 13 (7 posiciones)
        return players.Skip(primerElemento).Take(elementosPagina).ToList();
        //Cogemos la lista, saltamos desde el inicio (0 al principio, luego desde la posicion 7)
        //Take -> Cogemos la cantidad de página que queremos mostrar y pasamos a la lista para mostrar
    }

    public void Avanzar()
    {
        if (paginaActual < TotalPaginas()) //Si es menor a la catidad total de página avanza 1
        {
            paginaActual++;
        }
    }

    public void Retroceder()
    {
        if (paginaActual > 1)
        {
            paginaActual--;
        }
    }
    //Cogemos la cantidad de páginas que se va a mostrar
    public int TotalPaginas()
    {
        return (int)Math.Ceiling((double)players.Count / elementosPagina); //En este caso 63
    }


    ///////////////////////////ORDENAR//////////////////////////////////////////////////////////////////////////
    bool pulsar = false;
    public List<PlayersName> OrdenarNombre()
    {
        //Controlamos con pulsar bool que cada vez le demos nos ordene de una u otra forma
        if (pulsar)
        {
            //Lo metemos en players que es la variable principal para que afecte directamente a la lista que tenemos creada y la ordene
            //directamente en la tabla
            players = players.OrderByDescending(x => x.Player_name).ToList(); //Ordenamos de Z a la A
            pulsar = false; //Cambiamos a true para que la próxima vez que le demos se ejecute el contrario, de la A a la Z
        }
        else
        {
            players = players.OrderBy(x => x.Player_name).ToList(); //Ordenamos de A a la Z
            pulsar = true; //Lo contrario del anterior, para que se ejecute luego de la Z a la A
        }
        return players; 
    }

    public List<PlayersName> OrdenarEdad()
    {
        if (pulsar)
        {
            players = players.OrderByDescending(x => x.Age).ToList(); 
            pulsar = false; 
        }
        else
        {
            players = players.OrderBy(x => x.Age).ToList(); 
            pulsar = true; 
        }
        return players;
    }

    public List<PlayersName> OrdenarAltura()
    {
        if (pulsar)
        {
            players = players.OrderByDescending(x => x.Player_heightM).ToList();
            pulsar = false;
        }
        else
        {
            players = players.OrderBy(x => x.Player_heightM).ToList();
            pulsar = true;
        }
        return players;
    }

    public List<PlayersName> OrdenarEquipo()
    {
        if (pulsar)
        {
            players = players.OrderByDescending(x => x.Team_abbreviation).ToList();
            pulsar = false;
        }
        else
        {
            players = players.OrderBy(x => x.Team_abbreviation).ToList();
            pulsar = true;
        }
        return players;
    }

    public List<PlayersName> OrdenarPais()
    {
        if (pulsar)
        {
            players = players.OrderByDescending(x => x.Country).ToList();
            pulsar = false;
        }
        else
        {
            players = players.OrderBy(x => x.Country).ToList();
            pulsar = true;
        }
        return players;
    }

    ///////////////////////////////////////////BUSCADOR///////////////////////////////////////////////////////////////////
    private string searchTerm = "";

    public void Buscar()
    {
        players = players2; //CREAMOS NUEVA VARIABLE para reiniciar los datos antes de la búsqueda
        players = players.Where(p => p.Player_name.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
        //private List<PlayersName> filteredPlayers => players.Where(p => p.Player_name.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
        //IndexOf -> se comporta como contains, sino encuentra nada searchTerm devuelve -1 y por eso es >= 0 para que coja los resultados
        //y tiene StringComparison.OrdinalIgnoreCase para ignorarno distiga mayuscula/minuscula 
    }

    //////////////////////SELECCIÓN 
    /////PAÍSES
    public void CogePais(ChangeEventArgs e)
    {
        var seleccionar = e.Value.ToString();
        string pais = " ";

        switch (seleccionar)
        {
            case "Germany": pais = "Germany"; break;
            case "Argentina": pais = "Argentina"; break;
            case "Australia": pais = "Australia"; break;
            case "Bahamas": pais = "Bahamas"; break;
            case "Bosnia and Herzegovina": pais = "Bosnia and Herzegovina"; break;
            case "Brazil": pais = "Brazil"; break;
            case "Cameroon": pais = "Cameroon"; break;
            case "Canada": pais = "Canada"; break;
            case "China": pais = "China"; break;
            case "Croatia": pais = "Croatia"; break;
            case "Egypt": pais = "Egypt"; break;
            case "Slovenia": pais = "Slovenia"; break;
            case "Spain": pais = "Spain"; break;
            case "France": pais = "France"; break;
            case "Georgia": pais = "Georgia"; break;
            case "Greece": pais = "Greece"; break;
            case "Haiti": pais = "Haiti"; break;
            case "Italy": pais = "Italy"; break;
            case "Latvia": pais = "Latvia"; break;
            case "Lithuania": pais = "Lithuania"; break;
            case "Mali": pais = "Mali"; break;
            case "New Zealand": pais = "New Zealand"; break;
            case "Poland": pais = "Poland"; break;
            case "Puerto Rico": pais = "Puerto Rico"; break;
            case "United Kingdom": pais = "United Kingdom"; break;
            case "Czech Republic": pais = "Czech Republic"; break;
            case "Democratic Republic of the Congo": pais = "Democratic Republic of the Congo"; break;
            case "Dominican Republic": pais = "Dominican Republic"; break;
            case "Russia": pais = "Russia"; break;
            case "Senegal": pais = "Senegal"; break;
            case "Serbia": pais = "Serbia"; break;
            case "South Sudan": pais = "South Sudan"; break;
            case "Sweden": pais = "Sweden"; break;
            case "Switzerland": pais = "Switzerland"; break;
            case "Turkey": pais = "Turkey"; break;
            case "Ukraine": pais = "Ukraine"; break;
            case "USA": pais = "USA"; break;
            default: players = players2; break; // Restablece la lista original si no hay coincidencia
        }
        if(seleccionar == "todos") players = players2.Select(x => x).ToList();
        else players = players2.Where(x => x.Country == pais).ToList();
    }

    /////EQUIPO
    public void CogeEquipo(ChangeEventArgs e)
    {
        var seleccionar = e.Value.ToString();
        string equipo = " ";

        switch (seleccionar)
        {
            case "ATL": equipo = "ATL"; break;
            case "BOS": equipo = "BOS"; break;
            case "BKN": equipo = "BKN"; break;
            case "CHA": equipo = "CHA"; break;
            case "CHI": equipo = "CHI"; break;
            case "CLE": equipo = "CLE"; break;
            case "DAL": equipo = "DAL"; break;
            case "DEN": equipo = "DEN"; break;
            case "DET": equipo = "DET"; break;
            case "GSW": equipo = "GSW"; break;
            case "HOU": equipo = "HOU"; break;
            case "IND": equipo = "IND"; break;
            case "LAC": equipo = "LAC"; break;
            case "LAL": equipo = "LAL"; break;
            case "MEM": equipo = "MEM"; break;
            case "MIA": equipo = "MIA"; break;
            case "MIL": equipo = "MIL"; break;
            case "MIN": equipo = "MIN"; break;
            case "NOP": equipo = "NOP"; break;
            case "NYK": equipo = "NYK"; break;
            case "OKC": equipo = "OKC"; break;
            case "ORL": equipo = "ORL"; break;
            case "PHI": equipo = "PHI"; break;
            case "PHX": equipo = "PHX"; break;
            case "POR": equipo = "POR"; break;
            case "SAC": equipo = "SAC"; break;
            case "SAS": equipo = "SAS"; break;
            case "TOR": equipo = "TOR"; break;
            case "UTA": equipo = "UTA"; break;
            case "WAS": equipo = "WAS"; break;
            default: players = players2; break; // Restablece la lista original si no hay coincidencia
        }
        if (seleccionar == "todos") players = players2.Select(x => x).ToList();
        else players = players2.Where(x => x.Team_abbreviation == equipo).ToList();
    }
}