using UnityEngine;

public class AnimationTest : MonoBehaviour
{
    [SerializeField] Animator animator1;
    [SerializeField] Animator animator2;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TestAnimation1();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            TestAnimation2();
        }
    }

    private void TestAnimation1()
    {
        animator1.SetBool("IsBroken", true);
    }

    private void TestAnimation2()
    {
        animator2.SetBool("IsBroken", true);
    }
}
