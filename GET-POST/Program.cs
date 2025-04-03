using System.Net.Http.Json;
using System.Text.Json;

namespace WAFTemplate
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var url = "http://localhost:8080/Lluitadors";
            Uri uri = new(url);
            List<string> lluitadors = new List<string>();
            using (var client = new HttpClient())
            {
                var c = await client.GetAsync(uri);
                var json = await c.Content.ReadAsStringAsync();
                lluitadors = JsonSerializer.Deserialize<List<string>>(json);
            };
            
            Random rand = new Random();
            int empats = 0;
            
            while (true)
            {
                Console.WriteLine("------------------Lista de luchadores:------------------------------");
                lluitadors.ToList().ForEach(lluitador => Console.WriteLine(lluitador));
                string lluitador1 = lluitadors[rand.Next(0, lluitadors.Count)];
                string lluitador2 = lluitadors[rand.Next(0, lluitadors.Count)];
                
                if (lluitador1 == lluitador2) break;

                Resultat resultat = new();
                using (var client = new HttpClient())
                {
                    var lluita = new Lluita { lluitador1 = lluitador1, lluitador2 = lluitador2 };
                    var response = await client.PostAsJsonAsync("http://localhost:8080/Lluitadors/Combat", lluita);
                    resultat = await response.Content.ReadFromJsonAsync<Resultat>();
                }
                
                Console.WriteLine("GUANYA --->" + resultat.guanya);
                Console.WriteLine("PERD   --->" + resultat.perd);
                
                lluitadors.Remove(resultat.perd);
                if (lluitadors.Count == 1)
                {
                    lluitadors.ForEach(x => Console.WriteLine(x));
                    return;
                }

                if (lluitadors.Count == 2)
                {
                    if (resultat.guanya == resultat.perd)
                    {
                        if (empats == 2) return;
                        empats++;
                    }
                }
                else if (lluitadors.Count > 2)
                {
                    // Al menos un empate                    
                }
            }
        }

        // Los ultimos cambios son sobre el empate, dejo un comentario en cada uno, la siguiente version tendra la modificacion.
        
        private async Task Program2()
        {
            List<Lluitador> lluitadors = await GetList();
            while (true)
            {
                Resultat res = await SimulateBattle(lluitadors);
                ProcessResult(lluitadors, res);
                if (DosJugadorsIEmpats(lluitadors))
                {
                    // Ganan los dos
                }
                
                if (MesDeDosITotsEmpat(lluitadors))
                {
                    // Todos pierden
                }

            }
        }

        private async Task<List<Lluitador>> GetList()
        {
            const string url = "http://localhost:8080/Lluitadors";
            Uri uri = new(url);
            
            List<string> lluitadorsNom = new();
            using (var client = new HttpClient())
            {
                var c = await client.GetAsync(uri);
                var json = await c.Content.ReadAsStringAsync();
                lluitadorsNom = JsonSerializer.Deserialize<List<string>>(json);
            };
            
            List<Lluitador> lluitadors = new();
            lluitadorsNom.ForEach(x => lluitadors.Add(new Lluitador(x)));
            return lluitadors;
        }

        private async Task<Resultat> SimulateBattle(List<Lluitador> lluitadors)
        {
            Random rand = new();
            string lluitador1 = lluitadors[rand.Next(0, lluitadors.Count)].GetNom();
            string lluitador2 = lluitadors[rand.Next(0, lluitadors.Count)].GetNom();
            
            Resultat resultat = new();
            using (var client = new HttpClient())
            {
                var lluita = new Lluita { lluitador1 = lluitador1, lluitador2 = lluitador2 };
                var response = await client.PostAsJsonAsync("http://localhost:8080/Lluitadors/Combat", lluita);
                resultat = await response.Content.ReadFromJsonAsync<Resultat>();
            }

            return resultat;
        }

        private async Task ProcessResult(List<Lluitador> lluitadors, Resultat resultat)
        {
            if (resultat.guanya == resultat.perd) return;
            lluitadors.RemoveAt(lluitadors.FindIndex(x => x.GetNom() == resultat.perd));
        }

        private bool DosJugadorsIEmpats(List<Lluitador> lluitadors)
        {
            return false;
        }

        private bool MesDeDosITotsEmpat(List<Lluitador> lluitadors)
        {
            return false;
        }
    }}