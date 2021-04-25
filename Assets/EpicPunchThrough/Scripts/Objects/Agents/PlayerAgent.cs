using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerAgent : Agent
{
    public float mouseAimSmoothing = 15f;
  
    public enum Control {
        Horizontal,
        Vertical,
        Jump,
        Attack,
        Block
    }
    protected Dictionary<Control, float> controlState = new Dictionary<Control, float>();
    protected List<Control> controlQueue = new List<Control>();

    public override State state {
        get {
            return base.state;
        }
        set {
            base.state = value;
            RepeatControls();
        }
    }
    
    public override Technique activeTechnique {
        get {
            return base.activeTechnique;
        }
        set {
            base.activeTechnique = value;
            if( value == null ) {
                foreach( KeyValuePair<Control, float> control in controlState ) {
                    if( control.Value != 0 ) {
                        controlQueue.Add(control.Key);
                    }
                }
            }
        }
    }

    public override void Init()
    {
        base.Init();

        controlState.Add(Control.Horizontal, 0);
        controlState.Add(Control.Vertical, 0);
        controlState.Add(Control.Jump, 0);
        controlState.Add(Control.Attack, 0);
        controlState.Add(Control.Block, 0);

        directionIndicator.gameObject.SetActive(true);

        AgentManager.Instance.playerAgent = this;

        HandleSubscriptions(true);
    }

    protected void HandleSubscriptions(bool state)
    {
        if( state ) {
            InputManager.Instance.HorizontalInput += OnHorizontal;
            InputManager.Instance.VerticalInput += OnVertical;
            InputManager.Instance.JumpInput += OnJump;
            InputManager.Instance.AttackInput += OnAttack;
            InputManager.Instance.BlockInput += OnBlock;

            InputManager.Instance.AimHorizontal += OnAimHorizontal;
            InputManager.Instance.AimVertical += OnAimVertical;
        } else {
            InputManager.Instance.HorizontalInput -= OnHorizontal;
            InputManager.Instance.VerticalInput -= OnVertical;
            InputManager.Instance.JumpInput -= OnJump;
            InputManager.Instance.AttackInput -= OnAttack;
            InputManager.Instance.BlockInput -= OnBlock;

            InputManager.Instance.AimHorizontal -= OnAimHorizontal;
            InputManager.Instance.AimVertical -= OnAimVertical;
        }
    }

    #region Controls Methods

    protected bool OnAimHorizontal( float value )
    {
        aimDirection = new Vector2(aimDirection.x + (value / mouseAimSmoothing), aimDirection.y);

        if( controlState[Control.Attack] > 0 ) {
            SubmitChangedAttack();
        }

        return true;
    }
    protected bool OnAimVertical( float value )
    {
        aimDirection = new Vector2(aimDirection.x, aimDirection.y + (value / mouseAimSmoothing));

        if( controlState[Control.Attack] > 0 ) {
            SubmitChangedAttack();
        }

        return true;
    }

    protected bool OnHorizontal( float value )
    {
        UpdateControl(Control.Horizontal, value);

        return true;
    }
    protected bool OnVertical( float value )
    {
        UpdateControl(Control.Vertical, value);

        return true;
    }

    protected bool OnJump( float value )
    {
        UpdateControl(Control.Jump, value);

        return true;
    }

    protected bool OnAttack( float value )
    {
        UpdateControl(Control.Attack, value);

        return true;
    }

    protected bool OnBlock( float value )
    {
        UpdateControl(Control.Block, value);

        return true;
    }

    protected void UpdateControl( Control control, float value )
    {
        controlState[control] = value;

        controlQueue.Add(control);
    }

	#endregion

	#region Action Methods
	public override void DoUpdate( GameManager.UpdateData data )
    {
        if( controlQueue.Count > 0 ) {
            SubmitAction();
        }

        if( activeActionValue != 0 ) {
            Control control = ActionToControl(lastAction);
            if( controlState[control] == 0 ) {
                PerformAction(lastAction, 0);
            }
        }

        base.DoUpdate(data);
    }

    protected void SubmitAction()
    {
        switch( controlQueue[0] ) {
            case Control.Horizontal:
                SubmitHorizontalAction();
                break;
            case Control.Vertical:
                SubmitVerticalAction();
                break;
            case Control.Jump:
                SubmitAction(Action.Jump, controlState[Control.Jump]);
                break;
            case Control.Attack:
                SubmitAttack();
                break;
            case Control.Block:
                SubmitAction(Action.Block, controlState[Control.Block]);
                break;
        }

        controlQueue.RemoveAt(0);
    }
    protected void SubmitAction( Action action, float value )
    {
        if( lastAction != action || activeActionValue != value ) {
            PerformAction(action, value);
        }
    }
    protected void SubmitHorizontalAction()
    {
        if( controlState[Control.Horizontal] > 0 ) {
            if( isFacingRight ) {
                SubmitAction(Action.MoveForward, controlState[Control.Horizontal]);
            } else {
                SubmitAction(Action.MoveBack, controlState[Control.Horizontal]);
            }
        } else if( controlState[Control.Horizontal] < 0 ) {
            if( isFacingRight ) {
                SubmitAction(Action.MoveBack, -controlState[Control.Horizontal]);
            } else {
                SubmitAction(Action.MoveForward, -controlState[Control.Horizontal]);
            }
        } else {
            if( lastAction == Action.MoveBack ) {
                SubmitAction(Action.MoveBack, 0);
            } else {
                SubmitAction(Action.MoveForward, 0);
            }
        }
    }
    protected void SubmitVerticalAction()
    {
        if( controlState[Control.Vertical] > 0 ) {
            SubmitAction(Action.MoveUp, controlState[Control.Vertical]);
        } else {
            SubmitAction(Action.MoveDown, -controlState[Control.Vertical]);
        }
    }

    protected void SubmitAttack()
    {
        switch(aimSegment) {
            case AimSegment.Up:
                SubmitAction(Action.AttackUp, controlState[Control.Attack]);
                break;
            case AimSegment.Forward:
                SubmitAction(Action.AttackForward, controlState[Control.Attack]);
                break;
            case AimSegment.Down:
                SubmitAction(Action.AttackDown, controlState[Control.Attack]);
                break;
            case AimSegment.Back:
                SubmitAction(Action.AttackBack, controlState[Control.Attack]);
                break;
        }
    }
    protected void SubmitChangedAttack()
    {
        Action action = Action.None;
        switch(aimSegment) {
            case AimSegment.Up:
                action = Action.AttackUp;
                break;
            case AimSegment.Forward:
                action = Action.AttackForward;
                break;
            case AimSegment.Down:
                action = Action.AttackDown;
                break;
            case AimSegment.Back:
                action = Action.AttackBack;
                break;
        }
        if( lastAction == action ) { return; }

        actions.Clear();
        SubmitAttack();
    }
    #endregion

    public Control ActionToControl( Action action )
    {
        Control control = Control.Horizontal;
        switch( action ) {
            case Action.MoveForward:
                control = Control.Horizontal;
                break;
            case Action.MoveBack:
                control = Control.Horizontal;
                break;
            case Action.MoveUp:
                control = Control.Vertical;
                break;
            case Action.MoveDown:
                control = Control.Vertical;
                break;
            case Action.Jump:
                control = Control.Jump;
                break;
            case Action.AttackBack:
                control = Control.Attack;
                break;
            case Action.AttackDown:
                control = Control.Attack;
                break;
            case Action.AttackForward:
                control = Control.Attack;
                break;
            case Action.AttackUp:
                control = Control.Attack;
                break;
            case Action.Block:
                control = Control.Block;
                break;
        }
        return control;
    }

    protected void RepeatControls()
    {
        if( activeActionValue != 0 ) {
            ActionEvent handler = ActionToActionEvent(lastAction);
            if( handler != null ) {
                handler(new ActionEventArgs(activeActionValue));
            }
        } else {
            foreach( KeyValuePair<Control, float> control in controlState ) {
                if( control.Value != 0 ) {
                    controlQueue.Add(control.Key);
                }
            }
        }
    }
}
