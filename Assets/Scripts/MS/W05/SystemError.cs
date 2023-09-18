using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ping
{
    public static class SystemError
    {
        private const string InfoText = "�� �޽����� Ŭ���ϸ� ���̾��Űâ�� ǥ�õ˴ϴ�. ����: �� �޽��� ������ �ڵ�� �ߴܵǾ����ϴ�.";

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void Message(string message)
        {
            throw new Exception($"{message}. \n{InfoText} ");
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void Message(string message, GameObject gameObject, MonoBehaviour monoComponent)
        {
            throw new Exception($"{message} from  [{gameObject.name}]�� [{monoComponent.GetType().Name}] ������Ʈ \n{InfoText}");
        }
    }
}

