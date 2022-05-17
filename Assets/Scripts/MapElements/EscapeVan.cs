using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EscapeVan : MonoBehaviour {

    public List<MovementPlayer> players = new List<MovementPlayer>();

    public int PlayerInVan {
        get {
            return players.Count;
        }
    }

    public int BagsInVan {
        get {
            return bags.Count;
        }
    }

    public List<BagBase> bags = new List<BagBase>();
    public List<int> bagsIndexes = new List<int>();

    public TextMeshProUGUI bagText;

    public void OnBagEnter(Collider2D collision) {
        BagBase bb = null;
        if(collision.TryGetComponent<BagBase>(out bb)) {
            if (!bags.Contains(bb)) {
                bags.Add(bb);
                bagsIndexes.Add(1);
                UpdateText();
            } else {
                bagsIndexes[bags.IndexOf(bb)] += 1;
            }
        }
    }

    public void OnPlayerEnter(Collider2D collision) {
        MovementPlayer cp = null;
        if (collision.TryGetComponent<MovementPlayer>(out cp)) {
            players.Add(cp);
        }
    }
    public void OnPlayerExit(Collider2D collision) {
        MovementPlayer cp = null;
        if (collision.TryGetComponent<MovementPlayer>(out cp)) {
            players.Remove(cp);
        }
    }

    void UpdateText() {
        bagText.text = "<b>" + bags.Count.ToString() + " BAGS</b>";
    }

    public void OnBagExit(Collider2D collision) {
        BagBase bb = null;
        if (collision.TryGetComponent<BagBase>(out bb) && bags.Contains(bb)) {
            int i = bags.IndexOf(bb);
            bagsIndexes[i] -= 1;

            if (bagsIndexes[i] <= 0) {
                bags.RemoveAt(i);
                bagsIndexes.RemoveAt(i);
                UpdateText();
            }
        }
    }
}
