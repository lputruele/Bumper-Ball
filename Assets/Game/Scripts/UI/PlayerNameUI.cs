using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BumperBallGame
{
    public class PlayerNameUI : MonoBehaviour
    {
        public GameObject Player;
        // Start is called before the first frame update
        void Awake()
        {
            transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y + 1, Player.transform.position.z);

        }

        // Update is called once per frame
        void LateUpdate()
        {
            transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y + 1, Player.transform.position.z);
        }
    }
}