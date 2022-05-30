using UnityEngine;

public static class Utils
{
    public static string FormatNumber(this decimal money)
    {
        return string.Format("{0:n0}", money);
    }
    
    public static int ProcessXP(int kills, int difficulty, int bags, int hostagesKilled)
    {
        float killBonus = kills;
        float difficultyBonus = DifficultyBonus(difficulty);
        float bagsBonus = bags * 100;
        float hostageMalus = hostagesKilled * 10;
        return Mathf.Max(0, Mathf.RoundToInt(difficultyBonus * (killBonus + bagsBonus - hostageMalus)));
    }
    
    public static int DifficultyBonus(int difficulty)
    {
        return Mathf.RoundToInt(Mathf.Pow((difficulty + 1), 2f));
    }
    
    public static int Money_HostagePenalty(int difficulty, int hostagesKilled)
    {
        return DifficultyBonus(difficulty) * hostagesKilled * 1000;
    }
    
    public static int ProcessMoney(int difficulty, int moneyInEscapeVan, int hostagesKilled)
    {
        float difficultyBonus = DifficultyBonus(difficulty);
        float moneyBonus = moneyInEscapeVan;
        float hostageMalus = Money_HostagePenalty(difficulty, hostagesKilled);
        return Mathf.Max(0, Mathf.RoundToInt(moneyBonus - difficultyBonus * hostageMalus));
    }
}