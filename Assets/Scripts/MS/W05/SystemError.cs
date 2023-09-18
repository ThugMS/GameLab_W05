using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ping
{
    public static class SystemError
    {
        private const string InfoText = "이 메시지를 클릭하면 하이어라키창에 표시됩니다. 주의: 이 메시지 이후의 코드는 중단되었습니다.";

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void Message(string message)
        {
            throw new Exception($"{message}. \n{InfoText} ");
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void Message(string message, GameObject gameObject, MonoBehaviour monoComponent)
        {
            throw new Exception($"{message} from  [{gameObject.name}]의 [{monoComponent.GetType().Name}] 컴포넌트 \n{InfoText}");
        }
    }
}

