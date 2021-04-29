using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[CreateAssetMenu(menuName = "Simple Magic Cube/Cube Data")]
public class CubeData : ScriptableObject
{
    private int size;
    public float SizeFloat
    {
        get => size;
        set => size = Mathf.RoundToInt(value);
    }

    public bool IsDataLoaded { get; set; }  //TODO: Tie to actual load status

    public event System.EventHandler OnDataUpdated;


}
