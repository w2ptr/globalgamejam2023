using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTarget : MonoBehaviour
{
    public enum PlayerJoystick
    {
        Player1Stick1,
        Player1Stick2,

        Player1Central,
        Player2Central,

        Player2Stick1,
        Player2Stick2
    };

    public PlayerJoystick playerJoystick;

    private GameObject stick1;
    private GameObject stick2;

    // Start is called before the first frame update
    void Start()
    {
        if (playerJoystick == PlayerJoystick.Player1Central)
        {
            stick1 = GameObject.FindWithTag("Player1Target1");
            stick2 = GameObject.FindWithTag("Player1Target2");
        }
        if (playerJoystick == PlayerJoystick.Player2Central)
        {
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
                Move(Input.GetAxis("VerticalLeft"), Input.GetAxis("HorizontalLeft"));
                break;
            case PlayerJoystick.Player1Central:
                SetToAverage(stick1.transform, stick2.transform);
                break;
            case PlayerJoystick.Player1Stick2:
                Move(Input.GetAxis("VerticalRight"), Input.GetAxis("HorizontalRight"));
                break;
            case PlayerJoystick.Player2Stick1:
                Move(Input.GetAxis("VerticalLeft_P2"), Input.GetAxis("HorizontalLeft_P2"));
                break;
            case PlayerJoystick.Player2Central:
                SetToAverage(stick1.transform, stick2.transform);
                break;
            case PlayerJoystick.Player2Stick2:
                Move(Input.GetAxis("VerticalRight_P2"), Input.GetAxis("HorizontalRight_P2"));
                break;
            default:
                break;
        }
    }

    public void Move(float vertical, float horizontal)
    {
        //var translation1 = vertical * Time.deltaTime * 20;
        //transform.Translate(0, 0, translation1);

        transform.Translate(new Vector3(horizontal, 0, vertical) * (Time.deltaTime * 20));

        //var translation2 = horizontal * Time.deltaTime * 20;
        //transform.Translate(translation2, 0, 0);
    }

    private void SetToAverage(Transform transform1, Transform transform2)
    {
        transform.position = (transform1.position + transform2.position) / 2.0f;
    }
}
