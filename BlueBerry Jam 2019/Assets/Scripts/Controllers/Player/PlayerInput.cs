using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

[RequireComponent (typeof (Player))]
public class PlayerInput : MonoBehaviour {

    #region References
    Player player;

    // Xinput slot. Slot set when instantiated. state and prevState used to check controls
    public PlayerIndex playerIndex;
    GamePadState state;
    GamePadState prevState;
    #endregion

    #region Values
    // Stores Sticks and Triggers States
    Vector2 DPad;
    Vector2 leftStick = new Vector2();
    Vector2 rightStick = new Vector2();
    Vector2 directionalInput;

    float leftTrigger;
    float rightTrigger;

    bool leftTriggerOnce;
    bool rightTriggerOnce;
    #endregion

    // Use this for initialization
    void Start()
    {
        player = GetComponent<Player>();
    }

    void Update()
    {
        #region Gamepad Setup
        // Used to check the Gamepad state and previous State
        prevState = state;
        state = GamePad.GetState(playerIndex);

        // Stores Sticks/Triggers/DPad
        DPad.x = state.DPad.Left == ButtonState.Pressed ? -1 :
                 state.DPad.Right == ButtonState.Pressed ? 1 : 0;
        DPad.y = state.DPad.Down == ButtonState.Pressed ? -1 :
                 state.DPad.Up == ButtonState.Pressed ? 1 : 0;

        leftStick = new Vector2(state.ThumbSticks.Left.X, state.ThumbSticks.Left.Y);
        rightStick = new Vector2(state.ThumbSticks.Right.X, state.ThumbSticks.Right.Y);

        leftTrigger = state.Triggers.Left;
        rightTrigger = state.Triggers.Right;

        // Actions
        directionalInput = DPad != Vector2.zero ? DPad : leftStick;
        #endregion

        // Call functions inside the if statement to set what should be executed when pressed
        #region Button A
        if (prevState.Buttons.A == ButtonState.Released && state.Buttons.A == ButtonState.Pressed)
        {
            player.OnJumpInputDown();
        }
        else if (prevState.Buttons.A == ButtonState.Pressed && state.Buttons.A == ButtonState.Released)
        {
            player.OnJumpInputUp();
        }
        #endregion
        #region Button X
        if (prevState.Buttons.X == ButtonState.Released && state.Buttons.X == ButtonState.Pressed)
        {
            Debug.Log("Xbox Controller : Button X Pressed");
        }
        else if (prevState.Buttons.X == ButtonState.Pressed && state.Buttons.X == ButtonState.Released)
        {
            Debug.Log("Xbox Controller : Button X Released");
        }
        #endregion
        #region Button Y
        if (prevState.Buttons.Y == ButtonState.Released && state.Buttons.Y == ButtonState.Pressed)
        {
            Debug.Log("Xbox Controller : Button Y Pressed");
        }
        else if (prevState.Buttons.Y == ButtonState.Pressed && state.Buttons.Y == ButtonState.Released)
        {
            Debug.Log("Xbox Controller : Button Y Released");
        }
        #endregion
        #region Button B
        if (prevState.Buttons.B == ButtonState.Released && state.Buttons.B == ButtonState.Pressed)
        {
            Debug.Log("Xbox Controller : Button B Pressed");
        }
        else if (prevState.Buttons.B == ButtonState.Pressed && state.Buttons.B == ButtonState.Released)
        {
            Debug.Log("Xbox Controller : Button B Released");
        }
        #endregion
        #region Button LB
        if (prevState.Buttons.LeftShoulder == ButtonState.Released && state.Buttons.LeftShoulder == ButtonState.Pressed)
        {
            Debug.Log("Xbox Controller : Button LB Pressed");
        }
        else if (prevState.Buttons.LeftShoulder == ButtonState.Pressed && state.Buttons.LeftShoulder == ButtonState.Released)
        {
            Debug.Log("Xbox Controller : Button LB Released");
        }
        #endregion
        #region Button RB
        if (prevState.Buttons.RightShoulder == ButtonState.Released && state.Buttons.RightShoulder == ButtonState.Pressed)
        {
            Debug.Log("Xbox Controller : Button RB Pressed");
        }
        else if (prevState.Buttons.RightShoulder == ButtonState.Pressed && state.Buttons.RightShoulder == ButtonState.Released)
        {
            Debug.Log("Xbox Controller : Button RB Released");
        }
        #endregion
        #region Triggers [To Be Reworked]
        if (leftTrigger == 1)
        {
            if (leftTriggerOnce == false)
            {
                Debug.Log("Xbox Controller : Trigger LT Pressed");

                leftTriggerOnce = true;
            }
        }
        else if (leftTriggerOnce == true)
        {
            Debug.Log("Xbox Controller : Trigger LT Released");
            leftTriggerOnce = false;
        }

        if (rightTrigger == 1)
        {
            if (rightTriggerOnce == false)
            {
                Debug.Log("Xbox Controller : Trigger RT Pressed");

                rightTriggerOnce = true;
            }
        }
        else if (rightTriggerOnce == true)
        {
            Debug.Log("Xbox Controller : Trigger RT Released");
            rightTriggerOnce = false;
        }
        #endregion

        #region Start Button
        if (prevState.Buttons.Start == ButtonState.Released && state.Buttons.Start == ButtonState.Pressed)
        {
            Debug.Log("Xbox Controller : Button Start Pressed");
        }
        #endregion

        #region Keyboard Inputs [To Be Implemented]
        /*
        if (Input.GetKey(KeyCode.Q)) moveInput.x = -1;
        if (Input.GetKey(KeyCode.D)) moveInput.x = 1;
        if (Input.GetKey(KeyCode.Z)) moveInput.y = 1;
        if (Input.GetKey(KeyCode.S)) moveInput.y = -1;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            player.aimDirection = new Vector2(-1, 0);
        }*/
        #endregion
    }

    private void FixedUpdate()
    {
        player.SetDirectionalInput(directionalInput);
    }
}
