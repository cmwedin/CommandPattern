using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SadSapphicGames.CommandPattern.SimpleDemo {
    /// <summary>
    /// A component to manage the selection of different projectile types
    /// </summary>
    public class ProjectileSelectManager : MonoBehaviour
    {
        /// <summary>
        /// The player object to set these projectile for
        /// </summary>
        [SerializeField] private Player player;
        /// <summary>
        /// The scriptable object for the fast shot projectile
        /// </summary>
        [SerializeField] private LinearProj fastShot;
        /// <summary>
        /// The switch to set the fast shot projectile as the main projectile
        /// </summary>
        [SerializeField] private Toggle fastShotMainToggle;
        /// <summary>
        /// the switch to set the fast shot projectile as the alternate projectile
        /// </summary>
        [SerializeField] private Toggle fastShotAltToggle;

        /// <summary>
        /// The scriptable object for the slow shot projectile
        /// </summary>
        [SerializeField] private LinearProj slowShot;
        /// <summary>
        /// The switch to set the slow shot projectile as the main projectile
        /// </summary>
        [SerializeField] private Toggle slowShotMainToggle;
        /// <summary>
        /// the switch to set the slow shot projectile as the alternate projectile
        /// </summary>
        [SerializeField] private Toggle slowShotAltToggle;

        /// <summary>
        /// The scriptable object for the wide zig-zag projectile
        /// </summary>
        [SerializeField] private ZigZagProj wideZigZag;
        /// <summary>
        /// The switch to set the wide zig-zag projectile as the main projectile
        /// </summary>
        [SerializeField] private Toggle wideZigZagMainToggle;
        /// <summary>
        /// the switch to set the wide zig-zag projectile as the alternate projectile
        /// </summary>
        [SerializeField] private Toggle wideZigZagAltToggle;

        /// <summary>
        /// The scriptable object for the narrow zig-zag projectile
        /// </summary>
        [SerializeField] private ZigZagProj narrowZigZag;
        /// <summary>
        /// The switch to set the narrow zig-zag projectile as the main projectile
        /// </summary>
        [SerializeField] private Toggle narrowZigZagMainToggle;
        /// <summary>
        /// the switch to set the narrow zig-zag projectile as the alternate projectile
        /// </summary>
        [SerializeField] private Toggle narrowZigZagAltToggle;

        /// <summary>
        /// The scriptable object for the random ricochet projectile
        /// </summary>
        [SerializeField] private RandomProj randomProj;
        /// <summary>
        /// The switch to set the random ricochet projectile as the main projectile
        /// </summary>
        [SerializeField] private Toggle randomMainToggle;
        /// <summary>
        /// the switch to set the random ricochet projectile as the alternate projectile
        /// </summary>
        [SerializeField] private Toggle randomAltToggle;

        /// <summary>
        /// Subscribe delegate to set the player's projectile types to the various projectile switches being turned on
        /// initializes the primary projectile to fast shot and the alternate projectile to slow shot
        /// </summary>
        void Awake() {
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
    }
}