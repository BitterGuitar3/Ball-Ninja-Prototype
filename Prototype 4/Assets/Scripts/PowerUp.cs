using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Used for determining what kind of powerup this script is attactehd to and passes that info to player for the player controller script to use
public enum PowerUpType { None, Pushback, Rockets }//Public for player to set what powerup they currently have to none

public class PowerUp : MonoBehaviour
{
    public PowerUpType powerUptype;
}
