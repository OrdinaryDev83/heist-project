using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[Serializable]
public class TriggerEvent : UnityEvent<Collider2D> { }

[RequireComponent(typeof(Collider2D))]
public class OnTriggerDelegate : MonoBehaviour {
	[FormerlySerializedAs("OnTriggerEnter")] public TriggerEvent onTriggerEnter;
	[FormerlySerializedAs("OnTriggerExit")] public TriggerEvent onTriggerExit;

	private void OnTriggerEnter2D(Collider2D other) {
		if (onTriggerEnter != null) {
			onTriggerEnter.Invoke((Collider2D)other);
		}
	}

	private void OnTriggerExit2D(Collider2D other) {
		if (onTriggerExit != null) {
			onTriggerExit.Invoke((Collider2D)other);
		}
	}

}