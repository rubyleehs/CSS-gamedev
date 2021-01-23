using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Player
{
    public int playerDamage = -1;

    private Transform target;
    private Animator animator;
    private bool inputChanged;
    private Vector2Int curInputDir;
    private Direction faceDir;
    private Vector2Int inputDir;

    int xDir;
    int yDir;
    int lastXDir;
    int lastYDir;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        target = GameObject.FindGameObjectWithTag("Player").transform;

        if (Mathf.Abs ((int)target.position.x - (int)transform.position.x) == 1 && Mathf.Abs((int)target.position.y - (int)transform.position.y) == 0 || Mathf.Abs((int)target.position.y - (int)transform.position.y) == 1 && Mathf.Abs((int)target.position.x - (int)transform.position.x) == 0)
        {
            ChangeHealthAmount(playerDamage);
        }
        else if ((int)target.position.x - (int) transform.position.x != 0)
        {
            xDir = (int)target.position.x > (int)transform.position.x ? 1 : -1;
            yDir = 0;
        }
        else
        {
            yDir = (int)target.position.y > (int)transform.position.y ? 1 : -1;
            xDir = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector2Int Moving = new Vector2Int(xDir, yDir);
        inputChanged = (Moving != curInputDir);
        inputDir = curInputDir;

        if (inputChanged)
        {
            faceDir = base.DirChange(inputDir, faceDir);

            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90 * (int)faceDir));
        }
            base.Move(Moving);
    }
}
