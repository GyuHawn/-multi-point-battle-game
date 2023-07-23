using UnityEngine;
using UnityEngine.UI;

public class SliderValue : MonoBehaviour
{
    public Slider PointGauge;
    public float maxValue = 1.0f;
    private float currentValue = 0.0f;
    private float timeElapsed = 0.0f;
    private bool isActive = false;
    private Object pointRedPrefab; // 생성할 프리팹
    private Object pointBluePrefab; // 생성할 프리팹

    private Vector3 pointGaugeOriginalPosition; // PointGauge의 원래 위치값을 저장할 변수

    private void Awake()
    {
        // Resources 폴더에서 PointRed 프리팹을 로드합니다.
        pointRedPrefab = Resources.Load("PointRed");
        pointBluePrefab = Resources.Load("PointBlue");
    }

    void Start()
    {
        // PointGauge 오브젝트를 찾아서 변수에 할당합니다.
        PointGauge = GameObject.Find("PointGauge").GetComponent<Slider>();

        // PointGauge의 원래 위치값을 저장합니다.
        pointGaugeOriginalPosition = PointGauge.transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("타겟은 " + gameObject.tag + "입니다.");
            isActive = true;

            // PointGauge 오브젝트의 위치값을 변경합니다.
            PointGauge.transform.position = new Vector3(PointGauge.transform.position.x, 600, PointGauge.transform.position.z);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("타겟은 " + gameObject.tag + "입니다.");
            PointGauge.value = 0;
            timeElapsed = 0;
            isActive = false;

            // PointGauge 오브젝트의 위치값을 원래대로 변경합니다.
            PointGauge.transform.position = pointGaugeOriginalPosition;

            // 타겟이 사라졌을 때 PointRed 프리팹을 생성합니다.
            GameObject pointRedObject = Instantiate(pointRedPrefab) as GameObject;
            pointRedObject.transform.position = transform.position;
        }
    }

    void Update()
    {
        if (isActive)
        {
            if (PointGauge == null)
            {
                return;
            }

            if (currentValue >= maxValue) // maxValue에 도달했다면
            {
                // PointGauge 오브젝트의 위치값을 변경합니다.
                PointGauge.transform.position = new Vector3(PointGauge.transform.position.x, 1000, PointGauge.transform.position.z);

                string targetObjectName = gameObject.tag;
                GameObject[] objectsToDestroy = GameObject.FindObjectsOfType<GameObject>();

                foreach (GameObject objectToDestroy in objectsToDestroy)
                {
                    if (objectToDestroy.name.Contains(targetObjectName))
                    {
                        Debug.Log(objectToDestroy.name + " 삭제");
                        Destroy(objectToDestroy);
                    }
                }

                PointGauge.value = 0;
                timeElapsed = 0;
                isActive = false;

                // 타겟이 사라졌을 때 PointRed 프리팹을 생성합니다.
                GameObject pointRedObject = Instantiate(pointRedPrefab) as GameObject;
                pointRedObject.transform.position = transform.position;
            }
            else
            {
                timeElapsed += Time.deltaTime;
                currentValue = maxValue * timeElapsed / 5;
                PointGauge.value = currentValue;
            }
        }
    }
}
