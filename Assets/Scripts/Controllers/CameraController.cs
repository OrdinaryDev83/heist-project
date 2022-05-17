using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour {

    [HideInInspector]
    public Camera cam;
    private InventoryHandler _inv;
    private AnimationPlayer _anim;
    public Canvas canvas;

    public static CameraController I = null;

    private void Awake() {
        cam = GetComponent<Camera>();
        I = this;
        PlayersManager.OnPlayerSpawned += OnPlayerSpawned;
    }

    void OnPlayerSpawned(Transform player)
    {
        _target = player;
        _inv = player.GetComponent<InventoryHandler>();
        _anim = player.GetComponent<AnimationPlayer>();
    }
    

    private Transform _target;

    public Vector2 frequency;
    public Vector2 amplitude;
    public float dampRate = 1f;

    public RectTransform spreadCircle;

    float _rate;
    public void Shake(float amplitude = 1f) {
        this.amplitude = Vector2.one * (0.002f * amplitude);
        _rate = 1f;
    }

    float _time = 0f;
    Vector2 _shakeOffset;
    private void LateUpdate() {
        float min = Mathf.Min(frequency.y, frequency.x);
        if (_time >= min) {
            _time = 0f;
        } else {
            _time += Time.deltaTime * _rate;
        }

        if (_rate > 0f) {
            _rate -= Time.deltaTime * dampRate;
        } else if(_rate < 0f) {
            _rate = 0f;
        }
        _shakeOffset = new Vector2(Mathf.Cos(frequency.x * 2f * Mathf.PI * _time) * amplitude.x, Mathf.Cos(frequency.y * 2f * Mathf.PI * _time) * amplitude.y) * _rate;
        if (!_target) return;
        transform.position = (Vector3)_shakeOffset + Vector3.Slerp(transform.position, new Vector3(_target.position.x, _target.position.y, -10f), Time.deltaTime * 13f);

        var pos = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out pos);
        if(spreadCircle) spreadCircle.position = canvas.transform.TransformPoint(pos);

        Vector2 half = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = (half - (Vector2)transform.position);
        float y = dir.magnitude; //y

        if (spreadCircle) SetSpreadCircleSize(Mathf.Sin(Vector2.Angle(_inv.transform.GetChild(0).right, dir) * Mathf.Deg2Rad)*y);
        if (spreadCircle) SetCircleVisibility(_anim.isAiming && _inv.GetWearingWeaponType() == 0);
    }

    public void SetCircleVisibility(bool state) {
        spreadCircle.GetComponent<MPUIKIT.MPImage>().enabled = state;
    }

    public void SetSpreadCircleSize(float size) { // size en metres
        if (size <= 0.1f) {
            spreadCircle.sizeDelta = Vector2.one * 0.05f * 180f;
            return;
        }
        spreadCircle.sizeDelta = Vector2.one * size * 180f;
    }
}
