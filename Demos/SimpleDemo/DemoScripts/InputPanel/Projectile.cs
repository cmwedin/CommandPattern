using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SadSapphicGames.CommandPattern.SimpleDemo
{
    public class Projectile : MonoBehaviour
    {
        private bool active = false;
        private ProjSO data;

        public void Activate()
        {
            active = true;
        }
        public void LoadData(ProjSO _data)
        {
            this.data = _data;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (active)
            {
                Vector3 target = data.UpdatePosition(transform.position);
                if (target == Vector3.zero)
                {
                    Destroy(gameObject);
                }
                else
                {
                    this.transform.position = target;
                }
            }
        }
    }
}