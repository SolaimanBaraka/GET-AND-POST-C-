﻿namespace WAFTemplate;

public class Resultat
{
    public string guanya { get; set; }
    public string perd { get; set; }
    public Resultat()
    {
        guanya = "";
        perd = "";
    }
    public void guardarResultado(string tGuanya, string tPerd)
    {
        guanya = tGuanya;
        perd = tPerd;
    }
}