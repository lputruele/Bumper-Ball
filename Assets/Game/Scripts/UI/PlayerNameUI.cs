using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class PlayerNameUI : MonoBehaviour
    {
        public GameObject player;
        public TMP_Text floatingText;

        void Awake()
        {
            floatingText = GetComponent<TMP_Text>();
        }

        void LateUpdate()
        {
            if (player != null) {
                floatingText.text = player.transform.parent.name;
                transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 1, player.transform.position.z);
                if (!player.activeInHierarchy)
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }
}