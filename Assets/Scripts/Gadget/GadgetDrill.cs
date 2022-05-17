using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GadgetDrill : GadgetBase {

    InteractableSecuredDoor _door;
    [FormerlySerializedAs("_jamCurve")] [SerializeField]
    private AnimationCurve jamCurve;
    [FormerlySerializedAs("_spriteRenderer")] [SerializeField]
    SpriteRenderer spriteRenderer;

    public void SetRemainingTime(InteractableSecuredDoor door, int remainingTime) {
        this.remainingSecs = remainingTime;
        this.maxSecs = remainingTime;
        this._door = door;
        int r = Mathf.FloorToInt(jamCurve.Evaluate(Random.Range(0f, 1f)));
        _maxJam = Mathf.CeilToInt(r * ((remainingTime + 1) / 60f));
        _jammed = false;
        _jamCount = 0;
        _colorChangeTimer = 0.5f;

        StartCoroutine(EDrill());
    }

    private IEnumerator EDrill()
    {
        while (remainingSecs > 0 && _jamCount < _maxJam)
        {
            if (!_jammed)
            {
                var k = Random.Range(0, 100);
                if (k < 20)
                {
                    _jammed = true;
                    _jamCount++;
                }
            }
            yield return new WaitForSeconds(5f);
        }
    }

    float _cooldown = 0f;
    private int maxSecs = 0;
    public int remainingSecs = 0;
    private bool _jammed = false;
    private int _jamCount = 0;
    private int _maxJam = 3;

    private bool AbleToDrill => remainingSecs > 0 && !_jammed;
    
    float _colorChangeTimer = 0f;
    protected virtual void Update() {
        if (_jammed)
        {
            if (_colorChangeTimer < 0f)
                _colorChangeTimer = 1f;
            else
                _colorChangeTimer -= Time.deltaTime;
            spriteRenderer.color = Color.Lerp(Color.white, Color.red, Mathf.Sin(_colorChangeTimer * Mathf.PI * 4f));
        }
        
        if (AbleToDrill) {
            if (_cooldown > 0f) {
                _cooldown -= Time.deltaTime;
            } else if (_cooldown <= 0f) {
                remainingSecs--;
                _cooldown = 1f;
            }
        } else if (remainingSecs <= 0) {
            if (_door) {
                OnDrillFinished();
            }
            _cooldown = 0f;
        }
    }

    public override InteractableData GetData(InventoryHandler inv)
    {
        if (_jammed)
            return new InteractableData("Restart the Drill", 10f, false);
        else
            return new InteractableData("Drilling", 0f, true, 1f - ((float)remainingSecs / (float)maxSecs));
    }

    public override void OnInteract(InventoryHandler inv, string[] parameters)
    {
        _jammed = false;
    }

    protected virtual void OnDrillFinished() {
        _door.unlocked = true;
        _door.OnInteract(null, new string[] { transform.position.x.ToString(), transform.position.y.ToString() });
        Destroy(gameObject);
    }

    public override bool CanHover(InventoryHandler inv)
    {
        return true;
    }

    public override bool CanInteract(InventoryHandler inv)
    {
        return _jammed;
    }
}
