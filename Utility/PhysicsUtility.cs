using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PhysicsUtility
{
    public static void AdvancedSphereCast(Vector3 position, float radius, Vector3 direction, string[] tags, out Transform[] output)
    {
        var hits = Physics.SphereCastAll(position, radius, direction, 0.0f);
        hits = hits.Where(hit => tags.Contains(hit.transform.tag)).ToArray();

        var listOutput = new List<Transform>();
        for (int i = 0; i < hits.Length; i++)
        {
            if (!listOutput.Contains(hits[i].transform))
                listOutput.Add(hits[i].transform);
        }

        output = listOutput.ToArray();

    }
    public static void AdvancedSphereCast(Vector3 position, float radius, Vector3 direction, string[] tags, ref Transform[] output, BoolEvent callback)
    {
        int currentLength = (int)Mathf.Clamp01(output.Length);

        AdvancedSphereCast(position, radius, direction, tags, out output);

        int newLength = (int)Mathf.Clamp01(output.Length);
        if (currentLength != newLength)
        {
            callback.SafeInvoke(newLength > 0);
        }

    }

    public static bool CheckForObstacles(Transform from, Transform to)
    {
        RaycastHit hit = new RaycastHit();
        bool connectionClear = false;

        if(Physics.Linecast(from.position, to.position, out hit))
        {
            connectionClear = (hit.transform == to);
        }

        return connectionClear;
    }

    public static bool CheckForGround(Vector3 position, float liftDistance, float maxDistance, string groundTag, out Vector3 groundPosition)
    {
        groundPosition = position;
        Vector3 startPosition = position + (Vector3.up * liftDistance);

        var hits = Physics.RaycastAll(startPosition, Vector3.down * (liftDistance + maxDistance));

        Transform ground = null;
        for (int i = 0; i < hits.Length; i++)
        {
            if(hits[i].transform.tag == groundTag)
            {
                if (ground == null)
                {
                    ground = hits[i].transform;
                    groundPosition = hits[i].point;
                }

                else
                    Debug.Log("Hiding spot found more than one ground available. Picking highest");
            }

        }

        if (ground == null) throw new System.Exception(string.Format("Ground check found no collider with the tag '{0}'", groundTag));
        return ground != null;

    }

    public static Vector3 GetGroundPos(Vector3 position, float liftDistance, float maxDistance, string groundTag)
    {
        Vector3 pos;
        CheckForGround(position, liftDistance, maxDistance, groundTag, out pos);
        return pos;

    }

}
