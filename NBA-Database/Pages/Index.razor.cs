using System.ComponentModel.Design;
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


        public string Buscando()
        {
            return "";
        }


        /*DEMO BUSCADOR
    const updatePokemonList = () => {
    const searchTerm = searchInput.value.toLowerCase();
    container.innerHTML = '';
    pokemon.filter(p => p.name.toLowerCase().includes(searchTerm))
   .forEach(p => {
     const card = document.createElement("div");
     card.classList.add("pokemon_card");
     card.innerHTML = `
       <span class="name">${p.name.charAt(0).toUpperCase() + p.name.slice(1)}</span>
       <span class="number">#${p.url.split("/")[6].padStart(3, "0")}</span>
       <img src="">
       <div class="types"></div>`;

     card.addEventListener('click', () => { window.location.href = `single.html?id=${p.url.split("/")[6]}`; });
           
     fetch(p.url)
       .then(response => response.json())
       .then(data => {
         card.querySelector('img').src = data.sprites.other.home.front_default;
         const typesContainer = card.querySelector('.types');
         data.types.forEach(typeData => {
           const typeName = typeData.type.name;
           const typeElement = document.createElement('div');
           typeElement.textContent = typeTranslations[typeName];
           typeElement.style.cssText = `background-color:${typeColors[typeName]}; color:white; padding:5px 10px; border-radius:10px; margin-right:5px; text-shadow:-1px -1px 0 #000, 1px -1px 0 #000, -1px 1px 0 #000, 1px 1px 0 #000; border:solid black 2px`;
           typesContainer.appendChild(typeElement);
          });
       })
      container.appendChild(card);
   });
};
searchInput.addEventListener("input", updatePokemonList);
updatePokemonList();*/


        //PAGINACIÓN
        /*public class Paginacion
        {
            public PlayersName Next { get; set; }
            public PlayersName Back { get; set; }
        }*/
    }
    //Contabilizar
    private int contador = 0;
    private const int CANTMOSTRAR = 2;
    private const int LIMITECONTADOR = 4;
    public void Retroceder()
    {
        if (contador >= 1)
            contador -= CANTMOSTRAR;
        AgregarEliminar(contador);
        //Console.WriteLine(contador);
    }

    public void Avanzar()
    {
        if (contador <= players.Length && contador <= LIMITECONTADOR) //Para que no cuente más de lo que debería contar al repartir
                                                                      //el listado entre los que hay que mostrar
            contador += CANTMOSTRAR;
        AgregarEliminar(contador);
        //Console.WriteLine(contador);
    }
    //Meter datos en nuevo array
    public void AgregarEliminar(int contador)
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

            // Mostrar los elementos
            int mostrarLimite = Math.Min(newIndex, playersNew.Length); // Determinar el límite para mostrar elementos
            //for (int i = 0; i < playersNew.Length; i++) //Recorremos el nuevo array
            for (int i = 0; i < mostrarLimite; i++) //Recorremos el nuevo array
            {
                if (playersNew[i] != null) //Si no es nulo lo muestra
                {
                    Console.WriteLine(playersNew[i].Player_name);
                }

            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public void MostrarPorPagina()
    {
        for (int i = 1; i < contador; i++)
        {
            //si el contador muestre 2 jugadores
        }
    }

}