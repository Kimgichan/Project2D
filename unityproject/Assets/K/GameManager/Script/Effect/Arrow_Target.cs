using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

using DG.Tweening;
using NaughtyAttributes;

public class Arrow_Target : Arrow
{
    #region ���� ���
    /// <summary>
    /// �������� ����
    /// </summary>
    [SerializeField] protected float searchRange;
    [ReadOnly] [SerializeField] CreatureController target;
    [SerializeField] protected float delayTime;
    #endregion


    #region ������Ƽ ���

    #endregion


    #region �Լ� ���

    protected override IEnumerator MoveCor()
    {
        yield return null;

        StartCoroutine(SearchTargetCor(delayTime));

        trail.enabled = true;
        rigidbody.simulated = true;
        transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(dir.y, dir.x) *
            Mathf.Rad2Deg - 90f, Vector3.forward);

        while (true)
        {
            yield return null;

            if (target != null)
            {
                var targetDir = (target.transform.position - transform.position).normalized;


                #region smooth�� �������� �䱸�� �ʿ伺�� ����ٸ� �� �κ��� �ٽ� �츲
                //// ����(dot)�� ���� ���� ����
                //var dot = Vector3.Dot(Vector3.forward, targetDir);
                //if(dot < 1.0f)
                //{
                //    dir = targetDir;
                //    var angle = Mathf.Acos(dot) * Mathf.Rad2Deg;

                //    // ����(cross)�� ���� ������ ������ �Ǻ�
                //    var cross = Vector3.Cross(Vector3.forward, targetDir);

                //    if(cross.z < 0)
                //    {
                //        //rigidbody.MoveRotation(-angle*Time.deltaTime - 90f);
                //        transform.rotation = Quaternion.AngleAxis((angle * Mathf.Rad2Deg - 90f)*Time.deltaTime, Vector3.forward);
                //    }
                //    else
                //    {
                //        //rigidbody.MoveRotation(angle*Time.deltaTime - 90f);
                //    }
                //}
                #endregion

                dir = targetDir;
                transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f, Vector3.forward);
            }

            rigidbody.velocity = dir * speed;
        }
    }

    protected virtual IEnumerator SearchTargetCor(float delayTime)
    {
        var waitTime = new WaitForSeconds(delayTime);
        var controllerManager = GameManager.Instance.ControllerManager;

        float minDist = searchRange;

        target = null;

        while (true)
        {
            for(int i = 0, icount = controllerManager.GetCreatureControllerCount();
                i<icount; i++)
            {
                var currentDist = Vector3.Distance(controllerManager.GetCreatureController(i).transform.position, transform.position);
                if(currentDist < minDist)
                {
                    minDist = currentDist;
                    target = controllerManager.GetCreatureController(i);
                }
            }

            yield return waitTime;
        }
    }
    #endregion
}
