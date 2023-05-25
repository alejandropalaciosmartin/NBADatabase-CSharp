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
    public void Retroceder()
    {
        if (contador > 1)
            contador -= CANTMOSTRAR;
        AgregarEliminar(contador);
        Console.WriteLine(contador);
    }

    public void Avanzar()
    {
        if (contador < players.Length- CANTMOSTRAR)
            contador += CANTMOSTRAR;
        AgregarEliminar(contador);
        Console.WriteLine(contador);
    }
    //Meter datos en nuevo array
    public void AgregarEliminar(int contador)
    {
        string[] playersNew = new string[players.Length];
        try
        {
            //Meter en el nuevo array
            for (int i = contador; i < contador + CANTMOSTRAR; i++)
            {
                if (contador + CANTMOSTRAR - 1 != players.Length)
                {
                    playersNew[i] = players[i].ToString();
                    Console.WriteLine(contador);
                }
            }
            //Mostrar
            for (int i = contador; i < contador + CANTMOSTRAR; i++)
            {
                if (contador + CANTMOSTRAR - 1 != players.Length)
                {
                    Console.WriteLine(players[i].Player_name);
                }
            }
        }
        catch (Exception e)
        {
            throw new Exception("Sales del margen");
        }
    }

    public void MostrarPorPagina()
    {
        for (int i = 1; i < contador; i++)
        {
            //si el contador muestre 2 jugadores
        }
    }
    // private bool ParImpar => players.Length % 2 == 0;
    /*        for (int i = contador; i < contador + CANTMOSTRAR; i++)
        {
            if (!ParImpar)
            {
                if (contador + CANTMOSTRAR - 1 != players.Length)
                {
                    playersNew[i] = players[i].ToString();
                    Console.WriteLine(contador);
                }
            }
            else
            {
                if (contador + CANTMOSTRAR != players.Length)
                {
                    playersNew[i] = players[i].ToString();
                    Console.WriteLine(contador);
                }
            }
        }
        //Mostrar
        for (int i = contador; i < contador + CANTMOSTRAR; i++)
        {
            if (!ParImpar)
            {
                if (contador + CANTMOSTRAR - 1 != players.Length)
                {
                    Console.WriteLine(players[i].Player_name);
                }
            }
            else
            {
                if (contador + CANTMOSTRAR != players.Length)
                {
                    Console.WriteLine(players[i].Player_name);
                }
            }
        }*/
    /*public void SacarParImpar()
    {
        players.Length % 2 == 0;
    }*/
}