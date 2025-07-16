using UnityEngine;

namespace Weapon
{
    public class ExplosiveHitscanBullet : HitscanBullet
    {
        public override void OnHit(Transform @object)
        {
            base.OnHit(@object);
            transform.GetChild(0).position = hit.point;
            transform.GetChild(0).gameObject.SetActive(true);
            //spawn an explosion here
            Collider[] cols = new Collider[10];
            int nrOfHits = Physics.OverlapSphereNonAlloc(hit.point, 5, cols, hitMask);
            for (int i = 0; i < nrOfHits; i++)
            {
                base.OnHit(cols[i].transform.root);
            }
        }
    }
}