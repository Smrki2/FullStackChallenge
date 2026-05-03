using System.Collections.Generic;
using UnityEngine;

public class SpriteIconManager : MonoBehaviour
{
    [SerializeField] private Sprite[] moveSprites;
    [SerializeField] private string[] moveSpriteIds;

    private Dictionary<string, Sprite> moveSpriteMap;

    void Awake()
    {
        moveSpriteMap = new Dictionary<string, Sprite>();
        for (int i = 0; i < moveSpriteIds.Length; i++)
            moveSpriteMap[moveSpriteIds[i]] = moveSprites[i];
    }

    public Sprite GetMoveSprite(string moveId)
    {
        if (moveSpriteMap.ContainsKey(moveId))
            return moveSpriteMap[moveId];
        return null;
    }
}
