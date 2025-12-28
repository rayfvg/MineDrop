[System.Serializable]
public class PickaxeGroup
{
    public SymbolConfig pickaxe;
    public int count;

    public int Damage => pickaxe.damage;
}
