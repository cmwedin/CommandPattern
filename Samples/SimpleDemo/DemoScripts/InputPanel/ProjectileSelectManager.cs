using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SadSapphicGames.CommandPattern.SimpleDemo {
    public class ProjectileSelectManager : MonoBehaviour
    {
        [SerializeField] private Player player;
        [SerializeField] private LinearProj fastShot;
        [SerializeField] private Toggle fastShotMainToggle;
        [SerializeField] private Toggle fastShotAltToggle;

        [SerializeField] private LinearProj slowShot;
        [SerializeField] private Toggle slowShotMainToggle;
        [SerializeField] private Toggle slowShotAltToggle;

        [SerializeField] private ZigZagProj wideZigZag;
        [SerializeField] private Toggle wideZigZagMainToggle;
        [SerializeField] private Toggle wideZigZagAltToggle;

        [SerializeField] private ZigZagProj narrowZigZag;
        [SerializeField] private Toggle narrowZigZagMainToggle;
        [SerializeField] private Toggle narrowZigZagAltToggle;

        [SerializeField] private RandomProj randomProj;
        [SerializeField] private Toggle randomMainToggle;
        [SerializeField] private Toggle randomAltToggle;

    void Awake()
        {
            fastShotMainToggle.onValueChanged.AddListener((value) => {
                if(value) {
                    // Debug.Log("Setting primary fire to fast shot");     
                    player.SetPrimaryProj(fastShot);
                }
            });
            fastShotAltToggle.onValueChanged.AddListener((value) => {
                if(value) {
                    // Debug.Log("Setting alt fire to fast shot");     
                    player.SetAltProj(fastShot);
                }
            });
            slowShotMainToggle.onValueChanged.AddListener((value) => {
                if(value) {
                    // Debug.Log("Setting primary fire to slow shot");     
                    player.SetPrimaryProj(slowShot);
                }
            });
            slowShotAltToggle.onValueChanged.AddListener((value) => {
                if(value) {
                    // Debug.Log("Setting alt fire to slow shot");     
                    player.SetAltProj(slowShot);
                }
            });
            wideZigZagMainToggle.onValueChanged.AddListener((value) => {
                if(value) {
                    // Debug.Log("Setting primary fire to wide zig-zag");     
                    player.SetPrimaryProj(wideZigZag);
                }
            });
            wideZigZagAltToggle.onValueChanged.AddListener((value) => {
                if(value) {
                    // Debug.Log("Setting alt fire to wide zig-zag");     
                    player.SetAltProj(wideZigZag);
                }
            });
            narrowZigZagMainToggle.onValueChanged.AddListener((value) => {
                if(value) {
                    // Debug.Log("Setting primary fire to narrow zig-zag");     
                    player.SetPrimaryProj(narrowZigZag);
                }
            });
            narrowZigZagAltToggle.onValueChanged.AddListener((value) => {
                if(value) {
                    // Debug.Log("Setting alt fire to narrow zig-zag");     
                    player.SetAltProj(narrowZigZag);
                }
            });
            randomMainToggle.onValueChanged.AddListener((value) => {
                if(value) {
                    // Debug.Log("Setting primary fire to random ricochet");     
                    player.SetPrimaryProj(randomProj);
                }
            });
            randomAltToggle.onValueChanged.AddListener((value) => {
                if(value) {
                    // Debug.Log("Setting alt fire to random ricochet");     
                    player.SetAltProj(randomProj);
                }
            });
            fastShotMainToggle.isOn = true;
            slowShotAltToggle.isOn = true;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}