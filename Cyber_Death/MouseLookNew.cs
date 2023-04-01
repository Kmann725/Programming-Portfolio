using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

namespace GoofyGhosts
{
    public class MouseLookNew : MonoBehaviour
    {
        private Vector2 mousePos;
        private Quaternion newRot;
        Vector2 screenCenter;

        private PlayerControls newPC;
        public static bool scriptEnabled;

        // Start is called before the first frame update
        void Start()
        {
            newPC = new PlayerControls();
            scriptEnabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            
            Cursor.lockState = CursorLockMode.Confined;
            
        }

        // Update is called once per frame
        void Update()
        {
            if (!SceneManager.GetSceneByBuildIndex(3).isLoaded)
            {
                screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
                mousePos = Mouse.current.position.ReadValue();
                Vector2 direction = (mousePos - screenCenter).normalized;
                float angle = (Mathf.Atan2(-direction.y, direction.x)) * Mathf.Rad2Deg;
                //print(direction);
                transform.rotation = Quaternion.Euler(0, angle + 90, 0);
            }
        }

        public static void UseThis(TextMeshProUGUI text)
        {
            scriptEnabled = !scriptEnabled;
            if(scriptEnabled)
            {
                text.color = Color.green;
            }
            else
            {
                text.color = Color.red;
            }
        }
    }
}
