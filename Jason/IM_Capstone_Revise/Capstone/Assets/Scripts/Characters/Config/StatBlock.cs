using System;

[Serializable]
public class StatBlock
{
    public const int HEALTHBASE = 100;
    public const int ARMORBASE = 50;
    public const int MRBASE = 50;
    public const int ATTACKBASE = 50;
    public const int APBASE = 50;
    public const int ASBASE = 100;
    public const int MANABASE = 100;
    public const int TENACITYBASE = 0;
    public const int STAMINABASE = 100;
    public const int LUCKBASE = 100;

    public const int HEALTHLV = 10;
    public const int ARMORLV = 5;
    public const int MRLV = 5;
    public const int ATTACKLV = 5;
    public const int APLV = 5;
    public const int ASLV = 5;
    public const int MANALV = 10;
    public const int TENACITYLV = 3;
    public const int STAMINALV = 10;
    public const int LUCKLV = 10;

    public Stat Health = new Stat(HEALTHBASE, HEALTHBASE, HEALTHLV);
    public Stat Armor = new Stat(ARMORBASE, ARMORBASE, ARMORLV);
    public Stat MagicResist = new Stat(MRBASE, MRBASE, MRLV);
    public Stat Attack = new Stat(ATTACKBASE, ATTACKBASE, ATTACKLV);
    public Stat AbilityPower = new Stat(APBASE, APBASE, APLV);
    public Stat AttackSpeed = new Stat(ASBASE, ASBASE, ASLV);
    public Stat Mana = new Stat(MANABASE, MANABASE, MANALV);
    public Stat Tenacity = new Stat(TENACITYBASE, TENACITYBASE, TENACITYLV);
    public Stat Stamina = new Stat(STAMINABASE, STAMINABASE, STAMINALV);
    public Stat Luck = new Stat(LUCKBASE, LUCKBASE, LUCKLV);

    public static int[] ToArray(StatBlock stats)
    {
        return new int[]
        {
        stats.Health.Num,
        stats.Armor.Num,
        stats.MagicResist.Num,
        stats.Attack.Num,
        stats.AbilityPower.Num,
        stats.AttackSpeed.Num,
        stats.Mana.Num,
        stats.Tenacity.Num,
        stats.Stamina.Num,
        stats.Luck.Num
        };
    }

    public static StatBlock FromArray(int[] arr)
    {
        if (arr.Length != 10) throw new ArgumentException("Array must be length 10");

        return new StatBlock
        {
            Health = new Stat(arr[0], HEALTHBASE, HEALTHLV),
            Armor = new Stat(arr[1], ARMORBASE, ARMORLV),
            MagicResist = new Stat(arr[2], MRBASE, MRLV),
            Attack = new Stat(arr[3], ATTACKBASE, ATTACKLV),
            AbilityPower = new Stat(arr[4], APBASE, APLV),
            AttackSpeed = new Stat(arr[5], ASBASE, ASLV),
            Mana = new Stat(arr[6], MANABASE, MANALV),
            Tenacity = new Stat(arr[7], TENACITYBASE, TENACITYLV),
            Stamina = new Stat(arr[8], STAMINABASE, STAMINALV),
            Luck = new Stat(arr[9], LUCKBASE, LUCKLV)
        };
    }

}


public class Stat
{
    private int num;
    private int baseNum;
    private int lvInc;

    public Stat(int num, int baseNum, int lvInc)
    {
        this.num = num;
        this.baseNum = baseNum;
        this.lvInc = lvInc;
    }

    public int Num { get => num; set => num = value; }
    public int BaseNum { get => baseNum; set => baseNum = value; }
    public int LvInc { get => lvInc; set => lvInc = value; }
}