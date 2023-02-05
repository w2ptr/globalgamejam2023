using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTarget : MonoBehaviour
{
    public enum PlayerJoystick { Player1Stick1,Player1Stick2,Player1Central,Player2Stick1,Player2Stick2,Player2Central };
    public  PlayerJoystick playerJoystick;

    private float translation1;
    private float translation2;

    private GameObject stick1;
    private GameObject stick2;
    public Vector3 tempVec;

    // Start is called before the first frame update
    void Start()
    {
        if(playerJoystick == PlayerJoystick.Player1Central){
            stick1 = GameObject.FindWithTag("Player1Target1");
            stick2 = GameObject.FindWithTag("Player1Target2");
        }
        if(playerJoystick == PlayerJoystick.Player2Central){
            stick1 = GameObject.FindWithTag("Player2Target1");
            stick2 = GameObject.FindWithTag("Player2Target2");
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (playerJoystick)
        {
            case PlayerJoystick.Player1Stick1:
                translation1 = Input.GetAxis("VerticalLeft") * Time.deltaTime * 20;
                transform.Translate(0, 0, translation1);
                translation2 = Input.GetAxis("HorizontalLeft") * Time.deltaTime * 20;
                transform.Translate(translation2, 0, 0);                        
            break;
            case PlayerJoystick.Player1Central:
                tempVec = (stick1.transform.position + stick2.transform.position) / 2.0f;
                transform.position = tempVec;
            break;
            case PlayerJoystick.Player1Stick2:
                translation1 = Input.GetAxis("VerticalRight") * Time.deltaTime * 20;
                transform.Translate(0, 0, translation1);
                translation2 = Input.GetAxis("HorizontalRight") * Time.deltaTime * 20;
                transform.Translate(translation2, 0, 0);                         
            break;
            case PlayerJoystick.Player2Stick1:
                translation1 = Input.GetAxis("VerticalLeft_P2") * Time.deltaTime * 20;
                transform.Translate(0, 0, translation1);
                translation2 = Input.GetAxis("HorizontalLeft_P2") * Time.deltaTime * 20;
                transform.Translate(translation2, 0, 0);                        
            break;
                case PlayerJoystick.Player2Central:
                tempVec = (stick1.transform.position + stick2.transform.position) / 2.0f;
                transform.position = tempVec;
            break;
            case PlayerJoystick.Player2Stick2:
                translation1 = Input.GetAxis("VerticalRight_P2") * Time.deltaTime * 20;
                transform.Translate(0, 0, translation1);
                translation2 = Input.GetAxis("HorizontalRight_P2") * Time.deltaTime * 20;
                transform.Translate(translation2, 0, 0);                         
            break;
            default:
            break;
        }


       // Debug.Log($"Input HorizontalRight: {Input.GetAxis("HorizontalRight")}");
    }
}
