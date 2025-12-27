using UnityEngine;

[System.Serializable]
public class BlockChance
{
    public BlockType blockType;
    [Range(0f, 100f)]
    public float chance;
}
