using System.Net.Http.Json;
using static NBA_Database.Pages.Compare;

namespace NBA_Database.Pages;

//Para leer el archivo json 
public partial class Index
{
    //Metemos en una raíz porque el tamaño no cambiará de PlayersName
    private List<PlayersName> players;
    private List<PlayersName> players2;

    //Leemos y pasamos a players el contenido de json
    protected override async Task OnInitializedAsync()
    {
        players = await Http.GetFromJsonAsync<List<PlayersName>>("/assets/data/datas.json");
        //CREAMOS NUEVA VARIABLE para reiniciar los datos antes de la búsqueda
        players2 = players;
        //LIMITECONTADOR = players.Count / 10; //El total de json lo dividimos entre 10
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
    private int pageSize = 7; // Número de elementos por página
    private int currentPage = 1; // Página actual

    public List<PlayersName> GetPlayersForCurrentPage()
    {
        int startIndex = (currentPage - 1) * pageSize;
        return players.Skip(startIndex).Take(pageSize).ToList();
    }

    public void Avanzar()
    {
        if (currentPage < GetTotalPages())
        {
            currentPage++;
        }
    }

    public void Retroceder()
    {
        if (currentPage > 1)
        {
            currentPage--;
        }
    }

    public int GetTotalPages()
    {
        return (int)Math.Ceiling((double)players.Count / pageSize);
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
}