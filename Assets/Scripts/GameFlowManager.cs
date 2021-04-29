using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[CreateAssetMenu(menuName = "Simple Magic Cube/Game Flow Manager")]
public class GameFlowManager : ScriptableObject
{
    public void InitiateGame(bool newGame)
    {
        //Transition out of menu
        //Based on param, game script will determine whether or not to shuffle
        if (newGame)
            Debug.Log("New game started!");
        else
            Debug.Log("Previous game continued!");
    }
}
