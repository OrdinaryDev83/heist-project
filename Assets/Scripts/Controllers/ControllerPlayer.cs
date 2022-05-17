using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;

public class ControllerPlayer : ControllerBase
{
    private Camera _cam;
    private PlayerUI _ui;
    private InventoryHandler _inv;
    protected override void Start()
    {
        base.Start();
        _cam = CameraController.I.cam;
        _ui = PlayerUI.I;
        _inv = GetComponent<InventoryHandler>();
    }

    public float interactionMaxDistance = 2f;

    private IInteractable _interacting;
    private float _cooldown = 0f;
    private float _maxCd = 0f;
    protected void Update() {

        if (Health.dead) return;

        UpdateItemUse();
        UpdateItemSwitch();
        if (_inv.bag)
        {
            UpdateThrowBag();
        }
        else
        {
            UpdateGadgetDeploy();
        }

        MoveEntity(GetAxisRaw());

        UpdateInteractionHover();
        UpdateInteractionButton();

        UpdateReloadWeapon();
    }

    #region Inputs
    Vector2 GetAxisRaw()
    {
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    bool AimButton()
    {
        return Input.GetMouseButton(1);
    }

    bool ThrowButton()
    {
        return Input.GetKeyDown(KeyCode.G);
    }
    
    bool ThrowButtonDown()
    {
        return Input.GetKeyDown(KeyCode.G);
    }
    
    bool ReloadButtonDown()
    {
        return Input.GetKeyDown(KeyCode.R);
    }

    bool UseButton()
    {
        return Input.GetKey(KeyCode.E);
    }
    
    bool UseButtonDown()
    {
        return Input.GetKeyDown(KeyCode.E);
    }
    
    bool UseButtonUp()
    {
        return Input.GetKeyUp(KeyCode.E);
    }
    
    bool FireButton()
    {
        return Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space);
    }
    
    bool FireButtonDown()
    {
        return Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space);
    }

    Vector2 MousePosition()
    {
        return _cam.ScreenToWorldPoint(Input.mousePosition);
    }

    bool ScrollWheel()
    {
        return Input.mouseScrollDelta.y != 0;
    }
    
    #endregion
    
    private bool Weapon_Auto()
    {
        return Inv.GetWearingWeaponType2() == FirearmBase.Type.Rifle || Inv.GetWearingWeaponType2() == FirearmBase.Type.Smg;
    }

    private void UpdateItemUse()
    {
        if (FireButton() && Weapon_Auto()
            || (FireButtonDown() && !Weapon_Auto())) {
            Inv.UseItem();
        }
    }

    private void UpdateItemSwitch()
    {
        if (!ScrollWheel()) return;
        Inv.selected = Inv.selected == 0 ? 1 : 0;
        Inv.UpdateShownWeapon();
    }

    private void UpdateThrowBag()
    {
        if (!ThrowButton()) return;
        var dir = MousePosition() - (Vector2)transform.position;
        Inv.ThrowBag(dir, Inv.throwForce);
    }

    private void UpdateReloadWeapon()
    {
        if (!ReloadButtonDown()) return;
            Inv.Reload();
    }

    private void UpdateGadgetDeploy()
    {
        if (!ThrowButtonDown()) return;
        Inv.DeployGadget();
    }

    private void UpdateInteractionHover()
    {
        if (AimButton() && Inv.firearms.Length != 0) {
            Anim.Aim(MousePosition());

            var cursor = MousePosition();
            var dir = cursor - (Vector2)transform.position;

            var ib = Interaction.GetInteraction(dir, interactionMaxDistance, Inv.bag != null);
            if(ib != null && ib.CanHover(Inv))
            {
                _ui.SetInteractionTextState = true;
                _ui.SetInteractionText(cursor, string.Concat("[", ib.GetData(Inv).label, "]"));
                var data = ib.GetData(Inv);
                
                if(data.hasProgression) {
                    _ui.FillerState = true;
                    _ui.SetInteractionFiller(data.progression);
                }
                
                if (UseButtonDown() && ib.CanInteract(Inv)) {
                    _interacting = ib;
                    float i = data.interactionTime;
                    if (i > 0f) {
                        _ui.FillerState = true;
                        _cooldown = i;
                        _maxCd = i;
                    } else {
                        _ui.FillerState = false;
                    }
                }
            }
            else
            {
                _ui.SetInteractionTextState = false;
                _interacting = null;
                _cooldown = 0f;
                _ui.FillerState = false;
            }
        } else {
            _ui.SetInteractionTextState = false;
            _interacting = null;
            _cooldown = 0f;
            _ui.FillerState = false;
            Anim.StopAiming();
        }
    }

    private void UpdateInteractionButton()
    {
        if (_interacting != null)
        {
            if(UseButton())
            {
                _cooldown -= Time.deltaTime;
                _ui.SetInteractionFiller(1f - (_cooldown / _maxCd));
                if (_cooldown <= 0f)
                {
                    _interacting.OnInteract(Inv, new string[] { transform.position.x.ToString(), transform.position.y.ToString() });
                    _interacting = null;
                    _ui.SetInteractionFiller(0f);
                }
            }
            else
            {
                _interacting = null;
            }
        }
        
        if(UseButtonUp())
        {
            _cooldown = 0f;
            _interacting = null;
            _ui.FillerState = false;
        }
    }
}
