using System.Net.Http.Json;

namespace NBA_Database.Pages;

//Para leer el archivo json 
public partial class Index
{
    //Metemos en una raíz porque el tamaño no cambiará de PlayersName
    private PlayersName[]? players;

    //private Paginacion paginacion;

    //Leemos y pasamos a players el cotnenido de json
    protected override async Task OnInitializedAsync()
    {
        players = await Http.GetFromJsonAsync<PlayersName[]>("/assets/data/datas.json");
        //players = await Http.GetFromJsonAsync<PlayersName[]>("/assets/data/dataPrueba.json");
        LIMITECONTADOR = players.Length / 10; //El total de json lo dividimos entre 10
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
            string[] imagenExiste = { "CHI", "LAC", "TOR", "USA" };
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
    //Contabilizar
    private int contador = 0;
    private const int CANTMOSTRAR = 10;
    int LIMITECONTADOR;

    public int ContarPag()
    {
        if (contador == 0)
        {
            contador = 10;
        }
        return contador / 10;
    }

    public void Retroceder()
    {
        if (contador >= 1)
        {
            contador -= CANTMOSTRAR;
            //Para que salga el número de páginas

        }
        AgregarEliminar(contador);
        //Console.WriteLine(contador);
    }
    public void Avanzar()
    {
        //Mientras el contador sea menor que el total del array -CANTMOSTRAR, ponemos CANTMOSTRAR porque vamos en la tabla en grupo
        // de lo que pongamos en CANTMOSTRAR
        if (contador < players.Length - CANTMOSTRAR) //Para que no cuente más de lo que debería contar al repartir
        {                                                             //el listado entre los que hay que mostrar
            contador += CANTMOSTRAR;
        }
        AgregarEliminar(contador);
        //Console.WriteLine(contador);
    }


/////////////////////////////////////////////////////////////////Meter datos en nuevo array + Paginación///////////////////////////////////////////
    public PlayersName[] AgregarEliminar(int contador)
    {
        //Se crea nuevo array para meter los nuevos datos con límite al que pongamos para no cargar toda la lista
        PlayersName[] playersNew = new PlayersName[CANTMOSTRAR];
        try
        {
            //LLENAMOS EL NUEVO ARRAY
            int newIndex = 0; //Creamos un contador para almacenar desde la 1º posición, ya que si ponemos contador o i que es el mismo
            //valor, no se rellena correctamente, porque contador empieza desde donde esté
            for (int i = contador; i < contador + CANTMOSTRAR; i++) //Empieza desde el contador que vaya (ej: contador += CANTMOSTRAR y
            {                                                       //CANTMOSTRAR es 10 u otro, entonces el valor que empieza 0 se pone 
                                                                    //en 10 cuando pulsamos botón hasta los próximos 10, los 20 siguientes
               if (i < players.Length && players[i] != null) //Si el número del for que comience es menor que el total del array json y 
                {                                             //cada elemento es diferente a nulo
                    playersNew[newIndex] = players[i];  //Metemos cada elemento a la posición nueva del array nuevo
                    newIndex++;     //Posición nueva, después del 0 se incrementa en 1 para la próxima insercción
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        return playersNew;
    }

    ///////////////////////////ORDENAR//////////////////////////////////////////////////////////////////////////
    bool pulsar = false;
    //PlayersName[] playersOrdenado;
    public PlayersName[] OrdenarNombre()
    {
        //Controlamos con pulsar bool que cada vez le demos nos ordene de una u otra forma
        if (pulsar)
        {
            //Lo metemos en players que es la variable principal para que afecte directamente a la lista que tenemos creada y la ordene
            //directamente en la tabla
            players = players.OrderByDescending(x => x.Player_name).ToArray(); //Ordenamos de Z a la A
            pulsar = false; //Cambiamos a true para que la próxima vez que le demos se ejecute el contrario, de la A a la Z
        }
        else
        {
            players = players.OrderBy(x => x.Player_name).ToArray(); //Ordenamos de A a la Z
            pulsar = true; //Lo contrario del anterior, para que se ejecute luego de la Z a la A
        }
        return players; 
    }









}