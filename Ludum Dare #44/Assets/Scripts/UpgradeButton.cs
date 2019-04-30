using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UpgradeButton : MonoBehaviour {
    [SerializeField]
    Sprite[] icons;
    // 0: Double Jump
    // 1: Glide
    // 2: Long Jump
    // 3: Slo-Mo

    [SerializeField]
    GameObject description;

    [SerializeField]
    GameObject[] currentPowers;

    [SerializeField]
    UpgradeButton pair;

    int index;
    bool[] powers;
    bool instantiated;

    void Start() {
        // instantiate powers array
        powers = new bool[icons.Length];

        for (int i = 0; i < powers.Length; i++) {
            powers[i] = false;
        }

        instantiated = true;
    }

    public void LoadPowerUps() {
        if (!instantiated)
            Start();

        index = Random.Range(0, icons.Length);

        // player does not get offered power they already posses
        // make sure upgrade 2 button != upgrade 1 button
        if (gameObject.name.Equals("Upgrade 1 Button")) {
            // set upgrade 1 button index first
           while (powers[index] == true) {
                index = Random.Range(0, icons.Length);
                print("Upgrade 1: " + index);
           }

            print("Upgrade 1 DONE");

        } else {
            while (powers[index] == true || index == pair.index) {
                index = Random.Range(0, icons.Length);
                print("Upgrade 2: " + index);
            }

            print("Upgrade 2 DONE");
        }
        

        GetComponent<Image>().sprite = icons[index];

        switch (index) {
            case 0:
                description.GetComponent<TextMesh>().text = "Double Jump";                
                break;

            case 1:
                description.GetComponent<TextMesh>().text = "Hold Space to Glide";
                break;

            case 2:
                description.GetComponent<TextMesh>().text = "Platforms Are Closer";
                break;

            case 3:
                description.GetComponent<TextMesh>().text = "Press 'Shift' for Slo-Mo";
                break;
        }
    }

    public void Upgrade() {
        switch (index) {
            case 0:
                GameManager.instance.DoubleJumpUpgrade();
                powers[0] = true;
                // display current power
                for (int i = 0; i < currentPowers.Length; i++) {
                    if (currentPowers[i].GetComponent<CurrentUpgrades>().hasPower == false) {
                        currentPowers[i].GetComponent<CurrentUpgrades>().Display(0);
                        break;
                    }
                }

                break;

            case 1:
                GameManager.instance.GlideUpgrade();
                powers[1] = true;
                // display current power
                for (int i = 0; i < currentPowers.Length; i++) {
                    if (currentPowers[i].GetComponent<CurrentUpgrades>().hasPower == false) {
                        currentPowers[i].GetComponent<CurrentUpgrades>().Display(1);
                        break;
                    }
                }

                break;

            case 2:
                GameManager.instance.LongJumpUpgrade();
                powers[2] = true;
                // display current power
                for (int i = 0; i < currentPowers.Length; i++) {
                    if (currentPowers[i].GetComponent<CurrentUpgrades>().hasPower == false) {
                        currentPowers[i].GetComponent<CurrentUpgrades>().Display(2);
                        break;
                    }
                }

                break;

            case 3:
                GameManager.instance.SloMoUpgrade();
                powers[3] = true;
                // display current power
                for (int i = 0; i < currentPowers.Length; i++) {
                    if (currentPowers[i].GetComponent<CurrentUpgrades>().hasPower == false) {
                        currentPowers[i].GetComponent<CurrentUpgrades>().Display(3);
                        break;
                    }
                }

                break;
        }

        GameManager.instance.lives--;
        GameManager.instance.LoseHeart();
        GameManager.instance.CloseUpgrades();
    }

    public void CloseUpgrades() {
        GameManager.instance.CloseUpgrades();
    }
}
