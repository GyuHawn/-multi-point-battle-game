using UnityEngine;
using UnityEngine.UI;

public class SliderValue : MonoBehaviour
{
    public Slider PointGauge;
    public float maxValue = 1.0f;
    private float currentValue = 0.0f;
    private float timeElapsed = 0.0f;
    private bool isActive = false;
    private Object pointRedPrefab; // ������ ������
    private Object pointBluePrefab; // ������ ������

    private Vector3 pointGaugeOriginalPosition; // PointGauge�� ���� ��ġ���� ������ ����

    private void Awake()
    {
        // Resources �������� PointRed �������� �ε��մϴ�.
        pointRedPrefab = Resources.Load("PointRed");
        pointBluePrefab = Resources.Load("PointBlue");
    }

    void Start()
    {
        // PointGauge ������Ʈ�� ã�Ƽ� ������ �Ҵ��մϴ�.
        PointGauge = GameObject.Find("PointGauge").GetComponent<Slider>();

        // PointGauge�� ���� ��ġ���� �����մϴ�.
        pointGaugeOriginalPosition = PointGauge.transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Ÿ���� " + gameObject.tag + "�Դϴ�.");
            isActive = true;

            // PointGauge ������Ʈ�� ��ġ���� �����մϴ�.
            PointGauge.transform.position = new Vector3(PointGauge.transform.position.x, 600, PointGauge.transform.position.z);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Ÿ���� " + gameObject.tag + "�Դϴ�.");
            PointGauge.value = 0;
            timeElapsed = 0;
            isActive = false;

            // PointGauge ������Ʈ�� ��ġ���� ������� �����մϴ�.
            PointGauge.transform.position = pointGaugeOriginalPosition;

            // Ÿ���� ������� �� PointRed �������� �����մϴ�.
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

            if (currentValue >= maxValue) // maxValue�� �����ߴٸ�
            {
                // PointGauge ������Ʈ�� ��ġ���� �����մϴ�.
                PointGauge.transform.position = new Vector3(PointGauge.transform.position.x, 1000, PointGauge.transform.position.z);

                string targetObjectName = gameObject.tag;
                GameObject[] objectsToDestroy = GameObject.FindObjectsOfType<GameObject>();

                foreach (GameObject objectToDestroy in objectsToDestroy)
                {
                    if (objectToDestroy.name.Contains(targetObjectName))
                    {
                        Debug.Log(objectToDestroy.name + " ����");
                        Destroy(objectToDestroy);
                    }
                }

                PointGauge.value = 0;
                timeElapsed = 0;
                isActive = false;

                // Ÿ���� ������� �� PointRed �������� �����մϴ�.
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
