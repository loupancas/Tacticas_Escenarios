using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField]
    new string name;

    public string Name
    {
        get
        {
            return name;
        }
    }

    public Vector3 Position
    {
        get
        {
            return rigidbody.position;
        }
    }

    public Vector3 Velocity
    {
        get
        {
            return rigidbody.velocity;
        }
    }

    public FirstPersonPlayer player { get; private set; }

    new Rigidbody rigidbody;

    List<Projectile> incomingProjectiles;
    const float sortInterval = 0.5f;
    float sortTimer;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        player = GetComponent<FirstPersonPlayer>();

        incomingProjectiles = new List<Projectile>();
    }

    void FixedUpdate()
    {
        sortTimer = Mathf.Max(0, sortTimer - Time.fixedDeltaTime);

        if (sortTimer == 0)
        {
            SortIncomingProjectiles();
            sortTimer = sortInterval;
        }
    }

    void SortIncomingProjectiles()
    {
        var position = Position;

        if (incomingProjectiles.Count > 0)
        {
            incomingProjectiles.Sort((Projectile a, Projectile b) => {
                var distA = Vector3.Distance(a.transform.position, position);
                var distB = Vector3.Distance(b.transform.position, position);
                return distA.CompareTo(distB);
            });
        }
    }

    public Projectile GetIncomingProjetile()
    {
        if (incomingProjectiles.Count > 0)
        {
            return incomingProjectiles[0];
        }
        return null;
    }

    public void NotifyProjectileLaunched(Projectile projectile, bool value)
    {
        if (value)
        {
            incomingProjectiles.Add(projectile);
            SortIncomingProjectiles();
        }
        else
        {
            incomingProjectiles.Remove(projectile);
        }
    }
}