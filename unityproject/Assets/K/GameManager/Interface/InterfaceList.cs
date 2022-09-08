using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

namespace InterfaceList
{
    public interface WeaponPrefab
    {

        /// <summary>
        /// attackEvent : 모션이 공격 판정 포인트까지 도달했을 경우 어떤 공격 이펙트가 나갈 것인지.
        /// </summary>
        /// <param name="speed"></param>
        /// <param name="attackEvent">모션이 공격 판정 포인트까지 도달했을 경우 어떤 공격 이펙트가 나갈 것인지.</param>
        /// <returns></returns>
        public bool AttackAnim(float speed, UnityAction attackEvent);
        public void StopAnim();

        public Transform GetTr();
    }

    public interface Item
    {
        #region 프로퍼티 목록
        public string Name
        {
            get;
        }
        public Enums.ItemKind Kind
        {
            get;
        }

        /// <summary>
        /// 사용 중(장비 아이템은 착용, 소비 아이템은 퀵등록)이면 true
        /// </summary>
        public bool Use
        {
            get;
            set;
        }


        public int CurrentCount
        {
            get;
            set;
        }

        public int MaxCount
        {
            get;
        }

        public Sprite Icon
        {
            get;
        }

        /// <summary>
        /// 아이템 설명 문구
        /// </summary>
        public string Content
        {
            get;
        }
        #endregion


        #region 함수 목록

        /// <summary>
        /// 아이템이 파괴될 때
        /// </summary>
        public void Destroy();

        /// <summary>
        /// 아이템이 인벤토리에서 밖으로 떨어질 때
        /// </summary>
        public void Drop();

        public Item Copy();

        public bool Equal(InterfaceList.Item item);
        #endregion
    }
}