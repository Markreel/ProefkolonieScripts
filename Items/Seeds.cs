using UnityEngine;

public enum SeedType { PotatoSeed, Grain, VegetableSeed, }

public class Seeds : MonoBehaviour, IItem
{
    public SeedType SeedType;
}