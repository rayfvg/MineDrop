public class SlotResult
{
    public SymbolConfig symbol;
    public int amount;
    public Slot sourceSlot;   // 🔥 ВАЖНО

    public bool IsPickaxe => symbol != null && symbol.hasAmount;
    public bool IsBook => symbol != null && symbol.id == "Book";
    public bool IsDynamite => symbol != null && symbol.id == "Dynamite";
    public bool IsEye => symbol != null && symbol.id == "Eye";
}