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
        /// attackEvent : ����� ���� ���� ����Ʈ���� �������� ��� � ���� ����Ʈ�� ���� ������.
        /// </summary>
        /// <param name="speed"></param>
        /// <param name="attackEvent">����� ���� ���� ����Ʈ���� �������� ��� � ���� ����Ʈ�� ���� ������.</param>
        /// <returns></returns>
        public bool AttackAnim(float speed, UnityAction attackEvent);
        public void StopAnim();

        public Transform GetTr();
    }

    public interface Item
    {
        #region ������Ƽ ���
        public string Name
        {
            get;
        }
        public Enums.ItemKind Kind
        {
            get;
        }

        /// <summary>
        /// ��� ��(��� �������� ����, �Һ� �������� �����)�̸� true
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
        /// ������ ���� ����
        /// </summary>
        public string Content
        {
            get;
        }
        #endregion


        #region �Լ� ���

        /// <summary>
        /// �������� �ı��� ��
        /// </summary>
        public void Destroy();

        /// <summary>
        /// �������� �κ��丮���� ������ ������ ��
        /// </summary>
        public void Drop();

        public Item Copy();

        public bool Equal(InterfaceList.Item item);
        #endregion
    }
}