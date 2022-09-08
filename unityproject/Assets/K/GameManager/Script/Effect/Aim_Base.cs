using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

public class Aim_Base : Effect
{
    #region ����
    [SerializeField] protected Transform aimTr;
    [SerializeField] protected float radius;
    #endregion


    #region ������Ƽ

    #endregion


    #region �Լ�

    protected void OnEnable()
    {
        aimTr.transform.position = transform.position;

        #region ���� �ڵ�
        //var particles = new ParticleSystem.Particle[aimParticle.particleCount];
        //aimParticle.GetParticles(particles, particles.Length);

        //for(int i = 0, icount = particles.Length; i<icount; i++)
        //{
        //    var particle = particles[i];
        //    particle.position = transform.position;
        //}

        //aimParticle.SetParticles(particles);
        #endregion
    }


    public virtual void OnDrag(Vector2 dir)
    {
        aimTr.transform.position = transform.position + (Vector3)dir * radius;

        #region ���� �ڵ�
        //aimTr.transform.position = transform.position + (Vector3)dir * radius;
        //aimTr.transform.position = Vector3.Lerp(aimTr.transform.position, transform.position + (Vector3)dir * radius, Time.deltaTime * 30f);


        //var particles = new ParticleSystem.Particle[aimParticle.particleCount];
        //aimParticle.GetParticles(particles, particles.Length);

        //for (int i = 0, icount = particles.Length; i < icount; i++)
        //{
        //    var particle = particles[i];
        //    particle.position = transform.position + (Vector3)dir * radius;
        //}

        //Debug.Log(transform.position + (Vector3)dir * radius);
        //aimParticle.SetParticles(particles);
        #endregion
    }

    #endregion
}
