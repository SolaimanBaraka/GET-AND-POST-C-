namespace WAFTemplate;

public class Lluitador
{
    string nom { get; set; }
    private int empats {get; set;}

    public Lluitador(string tNom, int tEmpats)
    {
        this.nom = tNom;
        this.empats = tEmpats;
    }
    public Lluitador(string nom)
    {
        this.nom = nom;
        this.empats = 0;
    }
    public string GetNom()
    {
        return nom;
    }
}