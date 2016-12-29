using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVision : MonoBehaviour {
    public GameObject player;
    public Map map;
    public PlayerData playerData;
    public GameObject invisibleMask, visibleMask;
    List<GameObject> mask;
	// Use this for initialization
	void Start () {
        Color col = invisibleMask.GetComponent<SpriteRenderer>().color;
        col.a = 0.7f;
        invisibleMask.GetComponent<SpriteRenderer>().color = col;
        mask = new List<GameObject>(map.width * map.height);
	}
	
	// Update is called once per frame
	public void updateMap () {
        foreach(GameObject toDestroy in mask)
        {
            Destroy(toDestroy);
        }
        mask.Clear();
        List<bool> visibleArea = getVisibleData();
        int count = 0;
        foreach(bool visible in visibleArea)
        {
            GameObject g = null;
            if (visible) g = Instantiate(visibleMask, intToPoint(count) * 0.5f, Quaternion.identity);
            else g = Instantiate(invisibleMask, intToPoint(count) * 0.5f, Quaternion.identity);
            mask.Add(g);
            count++;
        }
	}
    public List<bool> getVisibleData()
    {
        int width = map.width;
        int height = map.height;
        Vector2 startPoint = playerData.location;
        int characterHeight = playerData.height;
        double lookDirection = playerData.lookingDirection;
        double sightAngle = playerData.sightAngle;
        int sightRange = playerData.sightRange;
        int standingHeight = (map.mapTileData[pointToInt(startPoint)].height) + characterHeight;
        List<bool> result = new List<bool>(new bool[width * height]);
        //방해물이 없을때의 시야
        double cos_lookDirMinusAngle = Math.Cos(lookDirection - sightAngle / 2);
        double cos_lookDirPlusAngle = Math.Cos(lookDirection + sightAngle / 2);
        double sin_lookDirMinusAngle = Math.Sin(lookDirection - sightAngle / 2);
        double sin_lookDirPlusAngle = Math.Sin(lookDirection + sightAngle / 2);
        double max_testX = Math.Max(0, Math.Max(cos_lookDirMinusAngle, cos_lookDirPlusAngle)) * (sightRange + 1);
        double min_testX = Math.Min(0, Math.Min(cos_lookDirMinusAngle, cos_lookDirPlusAngle)) * (sightRange + 1);
        double max_testY = Math.Max(0, Math.Max(sin_lookDirMinusAngle, sin_lookDirPlusAngle)) * (sightRange + 1);
        double min_testY = Math.Min(0, Math.Min(sin_lookDirMinusAngle, sin_lookDirPlusAngle)) * (sightRange + 1);

        if (lookDirection - sightAngle / 2 < 0 && lookDirection + sightAngle / 2 > 0) max_testX = sightRange + 1;
        else if (Math.Abs(lookDirection - sightAngle / 2) < Math.PI && Math.Abs(lookDirection + sightAngle / 2) > Math.PI) min_testX = -(sightRange + 1);

        if (lookDirection - sightAngle / 2 < Math.PI / 2 && lookDirection + sightAngle / 2 > Math.PI / 2) max_testY = sightRange + 1;
        else if (lookDirection - sightAngle / 2 < Math.PI * 3 / 2 && lookDirection + sightAngle / 2 > Math.PI * 3 / 2) min_testY = -(sightRange + 1);



        double testAngle;
        bool canSeeHeight, isInsideAngle, isInsideRange, isShadedByObstacle;
        List<Vector2> obstacles = new List<Vector2>();
        Console.WriteLine(lookDirection * 180 / Math.PI);
        Console.WriteLine(max_testX);
        Console.WriteLine(min_testX);
        //장애물 존재 check
        for (double testX = startPoint.x + min_testX; testX < startPoint.x + max_testX; testX++)
        {
            if (testX < 0 || testX >= width) continue;
            for (double testY = startPoint.y + (int)min_testY; testY < startPoint.y + max_testY; testY++)
            {
                if (testY < 0 || testY >= height) continue;
                isInsideAngle = false;
                foreach (Vector2 obs in obstacles)
                {
                    if (!isInsideAngle && isInsideObsAngle(startPoint, obs, new Vector2((float)testX, (float)testY))) isInsideAngle = true;
                }
                if (isInsideAngle) continue;
                canSeeHeight = true;

                //여기에 Height 계산 알고리즘을 넣어라

                if (!map.mapTileData[pointToInt(testX, testY)].isVisible || !canSeeHeight) // invisible tile
                {
                    //obstacles.Add(new Vector2(testX, testY));
                }

            }
        }
        for (double testX = startPoint.x + min_testX; testX < startPoint.x + max_testX; testX++)
        {
            if (testX < 0 || testX >= width) continue;
            for (double testY = startPoint.y + min_testY; testY < startPoint.y + max_testY; testY++)
            {
                if (testY < 0 || testY >= height) continue;
                //testX, testY : result에 저장될 것들
                if (testX - startPoint.x == 0) testAngle = testY - startPoint.y > 0 ? Math.PI / 2 : -Math.PI / 2;
                else testAngle = Math.Atan((double)(testY - startPoint.y) / (testX - startPoint.x)); // 각도 check
                isInsideAngle = (lookDirection - sightAngle / 2 <= testAngle && testAngle <= lookDirection + sightAngle / 2)
                                || (lookDirection - sightAngle / 2 + Math.PI <= testAngle && testAngle <= Math.PI / 2)
                                || (lookDirection + sightAngle / 2 - Math.PI >= testAngle);
                if (!isInsideAngle) continue;
                isInsideRange = isInsideSightRange(new Vector2((float)testX, (float)testY), startPoint, sightRange);
                if (!isInsideRange) continue;
                isShadedByObstacle = false;
                foreach (Vector2 obs in obstacles)
                {
                    if (!isShadedByObstacle && isInsideLineAngle(startPoint, new Vector2((float)testX, (float)testY), Math.Tanh((testY - startPoint.y) / (testX - startPoint.x))))
                        isShadedByObstacle = true;
                }
                result[pointToInt(testX, testY)] = isInsideAngle && !isShadedByObstacle;
                //result[pointToInt(testX, testY)] = true;
            }
        }
        return result;
    }
    private bool isInsideObsAngle(Vector2 startPoint, Vector2 obstacle, Vector2 toCheck)
    {
        double obsMinAngle = Math.Atan((obstacle.y - startPoint.y - 0.5) / (obstacle.x - startPoint.x - 0.5));
        double obsMaxAngle = Math.Atan((obstacle.y - startPoint.y + 0.5) / (obstacle.x - startPoint.x + 0.5));
        double testMinAngle = Math.Atan((toCheck.y - startPoint.y - 0.5) / (toCheck.x - startPoint.x - 0.5));
        double testMaxAngle = Math.Atan((toCheck.y - startPoint.y + 0.5) / (toCheck.x - startPoint.x + 0.5));
        return obsMinAngle < testMinAngle && testMaxAngle < obsMaxAngle;
    }
    /// <summary>
    /// 특정 점이 특정 두 점을 잇는 선 위에 존재하는가를 계산
    /// </summary>
    /// <param name="startPoint"></param>
    /// <param name="toCheck"></param>
    /// <returns></returns>
    private bool isInsideLineAngle(Vector2 startPoint, Vector2 toCheck, double angle)
    {
        double minAngle = Math.Atan((toCheck.x - startPoint.x - 0.5) / (toCheck.y - startPoint.y - 0.5));
        double maxAngle = Math.Atan((toCheck.x - startPoint.x + 0.5) / (toCheck.y - startPoint.y + 0.5));

        return minAngle < angle && angle < maxAngle;
    }
    private bool isInsideSightRange(Vector2 startPoint, Vector2 toCheck, int sightRange)
    {
        Vector2 g = startPoint - toCheck;
        return Math.Sqrt(g.x * g.x + g.y * g.y) < sightRange;
    }
    public int pointToInt(Vector2 p)
    {
        return (int)(p.x + p.y * map.width);
    }
    public int pointToInt(double x, double y)
    {
        return (int)(x + y * map.width);
    }
    public Vector2 intToPoint(int val)
    {
        return new Vector2(val % map.width, val / map.width);
    }
    public bool isInsideRange(Vector2 p)
    {
        return isInsideRange(p.x, p.y);
    }
    public bool isInsideRange(double x, double y)
    {
        return (x >= 0 && x < map.width && y >= 0 && y < map.height);
    }
}
