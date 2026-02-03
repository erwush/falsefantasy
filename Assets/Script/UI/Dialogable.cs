using UnityEngine;

[CreateAssetMenu(fileName = "Dialogable", menuName = "Scriptable Objects/Dialogable")]
public class Dialogable : ScriptableObject
{
    public int dialogCount;
    public int currentDial;
    public string[] nama;
    public string[] text;
    public Sprite[] avatar;
}
