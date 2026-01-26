using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FinalCombat : MonoBehaviour
{
    public float hp;
    public float maxHp;

    public float atk;

    public bool isHit;
    public Material flashHit;
    public Material defaultMaterial;
    protected Combat PlayerCombat;
    protected SpriteRenderer selfSprite;
    protected Rigidbody2D rb;

    protected EnemyMovement movement;
    [SerializeField] protected BarController healthBar;
    [SerializeField] protected Animator anim;
    [SerializeField] protected LayerMask pLayer;
    [HideInInspector] public bool canAttack;
    //>-----------------------------------------------
    public bool[] canSkill;
    public float[] skillCd;
    public float[] skillTimer;
    public float[] skillDmg;
    public Transform[] skillPoint;
    public float[] skillRadius;
    public GameObject[] skillEffect;
    public EnemyCombat[] bossCombat;
    public float skill4Stack;
    public int state;
    public int stack;
    public int[] maxStack;
    public List<GameObject> miniBoss;
    public AudioSource bossTheme;
    public List<GameObject> spawnedBoss;
    public GameObject[] skill0Loc;
    public GameObject[] skill1Loc;
    public Vector3 skill2Loc;
    public Vector2[] skillPos;
    public GameObject[] skillObj;
    public Sprite[] skill1Sprite;

    void Start()
    {
        // maxStack = new int[2]; //2 in state 1, 4 in state 2
        // canSkill = new bool[8];
        // skillCd = new float[8];
        // skillTimer = new float[8];
        // skillDmg = new float[8];
        // skillObj = new GameObject[8];
        // skill0Loc = new GameObject[2];
        // skillEffect = new GameObject[8];
        // skillRadius = new float[8];
        // skillPoint = new Transform[8];
        anim = GetComponent<Animator>();
        movement = GetComponent<EnemyMovement>();
        selfSprite = GetComponent<SpriteRenderer>();
        defaultMaterial = selfSprite.material;
        canAttack = true;
        rb = GetComponent<Rigidbody2D>();

        stack = 0;
        state = 2;
        StartCoroutine(ChangeState());
        maxStack = new int[2] { 2, 4 };
        //play theme
        bossTheme.Play();
    }



    void Update()
    {
        // healthBar.UpdateBar(hp, maxHp);

        if (skillTimer[4] <= 0)
        {
            skillTimer[4] = skillCd[4];
            skill4Stack += 1;
        }

        //Cooldown
        for (int i = 0; i < skillTimer.Length; i++)
        {
            if (skillTimer[i] > 0)
            {
                skillTimer[i] -= Time.deltaTime;
            }
        }

        if (state == 1 && stack >= maxStack[1])
        {
            StartCoroutine(ChangeState());
        }
        else if (state == 0 && stack >= maxStack[0])
        {
            StartCoroutine(ChangeState());
        }

    }

    public IEnumerator ChangeState()
    {


        if (state == 0)
        {
            state = 1;
            Skill0();
            Skill4();
        }
        else if (state == 1)
        {
            Skill7();
            yield return new WaitForSeconds(3f);
            state = 0;
            Skill0();
        }
        else
        {
            state = 0;
            Skill0();
        }
    }
    public void Skill0()
    {
        spawnedBoss = null;
        if (state == 0)
        {
            spawnedBoss = new List<GameObject>();
            int rand1 = Random.Range(0, miniBoss.Count);
            int rand2 = Random.Range(0, miniBoss.Count);
            spawnedBoss.Insert(0, Instantiate(miniBoss[rand1], skill0Loc[rand1].transform.position, Quaternion.identity));
            spawnedBoss.Insert(1, Instantiate(miniBoss[rand2], skill0Loc[rand2].transform.position, Quaternion.identity));
            for (int i = 0; i < 2; i++)
            {
                spawnedBoss[i].GetComponent<EnemyCombat>().isSpawned = true;
                spawnedBoss[i].GetComponent<EnemyCombat>().hp /= 2;
                spawnedBoss[i].GetComponent<EnemyCombat>().maxHp /= 2;
                spawnedBoss[i].GetComponent<EnemyCombat>().atk /= 2;
            }
        }
        else if (state == 1)
        {
            spawnedBoss = new List<GameObject>();
            for (int i = 0; i < 4; i++)
            {
                spawnedBoss[i] = Instantiate(miniBoss[i], skill0Loc[i].transform.position, Quaternion.identity);
            }
            for (int i = 0; i < 2; i++)
            {
                spawnedBoss[i].GetComponent<EnemyCombat>().isSpawned = true;
                spawnedBoss[i].GetComponent<EnemyCombat>().hp /= 2;
                spawnedBoss[i].GetComponent<EnemyCombat>().maxHp /= 2;
                spawnedBoss[i].GetComponent<EnemyCombat>().atk /= 2;
            }
        }

    }


    public IEnumerator Skill1()
    {
        canSkill[1] = false;

        int r = Random.Range(0, 5) + 1;
        GameObject[] obj = new GameObject[r];
        for (int i = 0; i < r; i++)
        {
            //rotation 135
            obj[i] = Instantiate(skillObj[1], skill1Loc[i].transform.position, Quaternion.Euler(0f, 0f, 135f));
            int random = Random.Range(0, 8);

            obj[i].GetComponent<SpriteRenderer>().sprite = skill1Sprite[random];
            yield return new WaitForSeconds(0.25f);
        }
        for (int i = 0; i < r; i++)
        {
            Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
            Vector2 dir = playerPos - obj[i].transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            obj[i].transform.rotation = Quaternion.Euler(0f, 0f, angle - 135f);
            yield return new WaitForSeconds(0.25f);
            obj[i].GetComponent<Skill1>().player = playerPos;
            obj[i].GetComponent<Skill1>().canMove = true;
            obj[i].GetComponent<Skill1>().dmg = skillDmg[1];
            obj[i].GetComponent<Skill1>().radius = skillRadius[1];
            obj[i].GetComponent<Skill1>().pLayer = pLayer;
            yield return new WaitForSeconds(0.5f);
        }
        skillTimer[1] = skillCd[1];
        canSkill[1] = true;


    }

    public void UseSkill2()
    {
        Skill2();
    }
    //> later
    public void Skill2()
    {
        float rx = Random.Range(skillPos[2].x, skillPos[2].y + 1);
        float ry = 0;
        if (state == 1)
        {

            GameObject obj1 = Instantiate(skillObj[2], new Vector2(rx, ry), Quaternion.identity);
            obj1.GetComponent<Skill2>().obj = gameObject;
            GameObject obj2 = Instantiate(skillObj[2], new Vector2(rx, ry), Quaternion.identity);
            obj2.GetComponent<Skill2>().obj = gameObject;
        }
        else if (state == 0)
        {

            GameObject obj = Instantiate(skillObj[2], new Vector2(rx, ry), Quaternion.identity);
            obj.GetComponent<Skill2>().obj = gameObject;
        }

    }

    
    
    public IEnumerator StartFinal(GameObject playerObj)
    {
        playerObj.GetComponent<Combat>().finalDuration = 0;
        yield return new WaitForSeconds(0.1f);
        playerObj.GetComponent<Combat>().StartCoroutine(playerObj.GetComponent<Combat>().Finalization(5f, 2));
        yield return new WaitUntil(() => playerObj.GetComponent<Combat>().isFinal == false);
        playerObj.GetComponent<Health>().HealthChange(-skillDmg[2]*playerObj.GetComponent<Combat>().finalHit);
    }

    public void Skill3()
    {

        int r = Random.Range(0, spawnedBoss.Count);
        spawnedBoss[r].GetComponent<EnemyCombat>().TriggerSkill();
        skillTimer[3] = skillCd[3];

    }

    public void Skill4()
    {
        skillTimer[4] = skillCd[4];
    }

    public void Skill5()
    {

    }

    public IEnumerator Skill6()
    {
        canSkill[6] = false;
        Collider2D[] hitss = Physics2D.OverlapCircleAll(skillPoint[6].position, skillRadius[7], pLayer);
        GameObject obj1 = Instantiate(skillObj[6], hitss[0].transform.position, Quaternion.identity, hitss[0].transform);
        yield return new WaitForSeconds(2f);
        Destroy(obj1);
        GameObject obj2 = Instantiate(skillEffect[6], hitss[0].transform.position, Quaternion.identity);
        Collider2D[] hits = Physics2D.OverlapCircleAll(obj2.transform.position, skillRadius[6], pLayer);
        if (hits.Length > 0)
        {
            Destroy(obj2);
            hits[0].GetComponent<Health>().HealthChange(-skillDmg[6]);
            hits[0].GetComponent<Movement>().spd /= 2;
            yield return new WaitForSeconds(2f);
            hits[0].GetComponent<Movement>().spd *= 2;
        }
        skillTimer[6] = skillCd[6];
        canSkill[6] = true;


    }



    public void Skill7()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(skillPoint[7].position, skillRadius[7], pLayer);
        if (hits.Length > 0)
        {
            hits[0].GetComponent<Health>().HealthChange(-skillDmg[7] * skill4Stack);
        }
    }
    //*Skill 0: Summon 2 (or 4 in state 1) Random(not random in state 1) mini-bosses from earlier stage with fewer HP and Attack.
    //*When defeated, the main boss will take damage equal to the minibosses HP.
    //*When the miniboss is defeated, grant 1 stack.
    //*Can only be used at the start of each state.

    //*Skill 1: Spawn up to 5 object at once. It will move sequentially to a locked position
    //*Locked position is the position where the player is currently at before it start moving.
    //*Explode once it hit its locked position.

    //*Skill 2: Spawn 1(or 2 in state 1) Object to random predetermined position. If the player touches the object within the duration of the skill,
    //*the Player immediately enter its Finalization state with smaller dmg boost and longer duration.
    //*but will take damage at the end of the Finalization state for each hit done.

    //*Skill 3: Make a random spawned mini boss immediately trigger one of its skill.

    //*Skill 4: Trigger a 20 seconds timer. Everytime the timer reaches 0, it goes back to 20 seconds again.
    //*But will increase skill 7 multiplier for each time it reaches 0.

    //*Skill 5: Mark an Area and deal continous minor damage to the player within the area. This area exist for 4 seconds.

    //*Skill 6: Lock the player. After a few seconds, immedieately deal mid level damage. This attack is dodgeable and parryable.
    //*If the player is hit by this attack, reduce player speed by 50% for 2 seconds.

    //*Skill 7: Deal Unavoidable AoE damage. This damage can be reduced but can't be negated.
    //*Each stack gained from Skill 4 timer will increase the multiplier.
    //*If this attack is not being parry'd, there's 30% chance to stun the player for 2 seconds.
    //*Exit state 1 and Enter state 0 after the usage of this skill.

    //*State Explanation
    //*State 0: Immediately activate skill 0. In this state, this boss can use skill 0, 1, 2, 3. After 2 stacks, immediaely enter State 2.

    //*State 1: Immediately activate skill 0 and skill 4. In this state, this boss can use all of its skill.
    //*After 4 stacks, immediately activate skill 7.



    protected IEnumerator HurtAnim()
    {
        selfSprite.material = flashHit;
        yield return new WaitForSeconds(0.2f);
        selfSprite.material = defaultMaterial;
    }

    public void UseSkill1()
    {
        StartCoroutine(Skill1());
    }


}
