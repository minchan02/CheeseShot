using UnityEngine;
using System.Collections;

public class Thunder : MonoBehaviour
{
    void Start()
    {
        Destroy(this.gameObject, 0.8f);
        //객체가 생성된 뒤 0.8초 뒤에 객체를 삭제합니다.
    }
}
