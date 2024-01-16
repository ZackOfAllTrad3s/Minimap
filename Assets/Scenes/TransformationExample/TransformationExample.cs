using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI.Table;

[ExecuteInEditMode]
public class TransformationExample : MonoBehaviour
{
    public Vector3 translation;
    public Vector3 rotation;
    public Vector3 scale;

    [SerializeField]
    private TextMeshProUGUI trsField;
    [SerializeField]
    private Transform matrixUi;
    [SerializeField]
    private Image minimap;
    [SerializeField]
    private Image player;
    [SerializeField]
    private TextMeshProUGUI playerPositionField;
    [SerializeField]
    private Image mapPlayer;
    [SerializeField]
    private TextMeshProUGUI mapPlayerPositionField;


    [SerializeField]
    private RectTransform minimapRectTransform;

    [SerializeField]
    private RectTransform playerMapIcon;

    Matrix4x4 transformation;
    private void Update()
    {
        trsField.text = $"Translation: {translation}\nRotation: {rotation}\nScale: {scale}";
        transformation = Matrix4x4.TRS(translation, Quaternion.Euler(rotation), scale);

        int i = 0;
        for (int row = 0; row < 4; row++)
        {
            for (int col = 0; col < 4; col++)
            {
                matrixUi.GetChild(i).GetComponent<TextMeshProUGUI>().text = transformation[row, col].ToString("F2");
                i++;
            }
        }

        var worldPosition = player.rectTransform.anchoredPosition + player.rectTransform.parent.GetComponent<RectTransform>().sizeDelta / 2;
        playerPositionField.text = $"World Position: {worldPosition}";

        var mapPosition = transformation.MultiplyPoint3x4(worldPosition);
        mapPlayerPositionField.text = $"Map Position: {new Vector2(mapPosition.x, mapPosition.y)}";
        mapPlayer.rectTransform.anchoredPosition = mapPosition;

        CenterMapOnPlayer();
    }

    private void CenterMapOnPlayer()
    {
        float mapScale = minimapRectTransform.transform.localScale.x;
        // we simply move the map in the opposite direction the player moved, scaled by the mapscale
        minimapRectTransform.anchoredPosition = (-playerMapIcon.anchoredPosition * mapScale);
    }
}
