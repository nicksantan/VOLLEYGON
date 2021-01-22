using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CarouselItemScript : MonoBehaviour, ISelectHandler {

  RectTransform carouselRectTransform;
  CarouselScript carouselManager;
  RectTransform rectT;
    public bool usingNewCurtain = false;
    public GameObject newCurtain;
    public GameObject arenaName;
  public bool useCurtain = false;

  void Start(){
    Transform carousel = transform.parent.parent.parent; // TODO: make this less brittle
    carouselRectTransform = carousel.GetComponent<RectTransform>();
		carouselManager = carousel.GetComponent<CarouselScript>();
    rectT = GetComponent<RectTransform>();

        if (usingNewCurtain)
        {
            newCurtain = gameObject.transform.Find("NewCurtain").gameObject;
            arenaName = gameObject.transform.Find("ArenaName").gameObject;
        }
  }

    void OnGUI() {
        Vector2 myPos = rectT.position - carouselRectTransform.position;

        float scaleFactor = 1 - (Mathf.Abs(myPos.y) / 10);
        float clampedScale = Mathf.Clamp(scaleFactor, 0.5f, 1.2f);
        rectT.localScale = new Vector3(clampedScale, clampedScale, 1f);

        float opacityFactor = 1 - (Mathf.Abs(myPos.y) / 1);
        float clampedOpacity = Mathf.Clamp(opacityFactor, 0.3f, 1f);
       
        if (usingNewCurtain)
        {
            newCurtain.GetComponent<Image>().color = new Color(0f,0f,0f, 1 -clampedOpacity);
            arenaName.GetComponent<Text>().color = new Color(255f,255f,255f, clampedOpacity);
        } else
        {
            GetComponent<CanvasGroup>().alpha = useCurtain ? 1 - clampedOpacity : clampedOpacity;
        }

    }

  public void OnSelect(BaseEventData e) {
		carouselManager.MoveToSelected();
  }
}
