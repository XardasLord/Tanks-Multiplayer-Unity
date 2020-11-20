using UnityEngine;

namespace Units
{
    public class UnitSelectionDisplay : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer unitSelectionHighlight;

        private void Awake()
        {
            GetComponent<Unit>().OnSelected += ShowSelectionHighlight;
            GetComponent<Unit>().OnDeselected += HideSelectionHighlight;
        }

        private void ShowSelectionHighlight() 
            => unitSelectionHighlight.enabled = true;
        private void HideSelectionHighlight()
            => unitSelectionHighlight.enabled = false;
    }
}
