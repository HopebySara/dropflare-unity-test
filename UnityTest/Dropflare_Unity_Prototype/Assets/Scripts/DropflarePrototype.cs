using System.Collections.Generic;
using UnityEngine;

// Mini prototype: everything is created automatically when Play starts.
public class DropflarePrototype : MonoBehaviour
{
    Transform hero;
    Camera followCamera;
    readonly List<Target> targets = new List<Target>();
    Vector2 move;
    Vector2 touchMove;
    float attackCooldown;
    int destroyed;
    int hp = 100;
    GUIStyle labelStyle;

    class Target
    {
        public GameObject body;
        public int hp = 3;
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void Initialize()
    {
        if (FindFirstObjectByType<DropflarePrototype>() == null)
            new GameObject("Dropflare Prototype Controller").AddComponent<DropflarePrototype>();
    }

    void Start()
    {
        Application.targetFrameRate = 60;
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        RenderSettings.ambientLight = new Color(.65f, .7f, .8f);
        MakeCube("Arena", new Vector3(0, -.3f, 0), new Vector3(32, .5f, 32), new Color(.22f,.34f,.36f));
        for (int i = -2; i <= 2; i++)
        {
            MakeCube("Floor line", new Vector3(i*6, -.025f, 0), new Vector3(.06f,.03f,32), new Color(.2f,.5f,.53f));
            MakeCube("Floor line", new Vector3(0, -.025f, i*6), new Vector3(32,.03f,.06f), new Color(.2f,.5f,.53f));
        }
        MakeCube("North border",new Vector3(0,1,16),new Vector3(32,2,1),Color.gray);
        MakeCube("South border",new Vector3(0,1,-16),new Vector3(32,2,1),Color.gray);
        MakeCube("East border",new Vector3(16,1,0),new Vector3(1,2,32),Color.gray);
        MakeCube("West border",new Vector3(-16,1,0),new Vector3(1,2,32),Color.gray);
        MakeCube("Cover A",new Vector3(-5,1,1),new Vector3(3,2,2),new Color(.48f,.46f,.4f));
        MakeCube("Cover B",new Vector3(5,1,5),new Vector3(2,2,3),new Color(.48f,.46f,.4f));
        MakeCube("Cover C",new Vector3(3,1,-6),new Vector3(2,2,2),new Color(.48f,.46f,.4f));

        var p = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        p.name = "Prototype player";
        p.transform.position = new Vector3(0,1,-10);
        p.GetComponent<Renderer>().material.color = new Color(.14f,.85f,.62f);
        hero = p.transform;
        MakeCube("Player facing visor",hero.position+new Vector3(0,.45f,.47f),new Vector3(.65f,.22f,.12f),Color.black).transform.SetParent(hero, true);
        var visor = GameObject.Find("Player facing visor");
        visor.transform.localPosition = new Vector3(0,.45f,.47f);

        SpawnTarget(new Vector3(-7,1,9));
        SpawnTarget(new Vector3(0,1,6));
        SpawnTarget(new Vector3(8,1,0));
        SpawnTarget(new Vector3(10,1,-9));

        var lightObj = new GameObject("Sun");
        var sun = lightObj.AddComponent<Light>();
        sun.type = LightType.Directional; sun.intensity = 1.4f;
        lightObj.transform.rotation = Quaternion.Euler(48,-30,0);
        var cameraObj = new GameObject("Third person camera");
        followCamera = cameraObj.AddComponent<Camera>();
        followCamera.fieldOfView = 66;
        followCamera.clearFlags = CameraClearFlags.SolidColor;
        followCamera.backgroundColor = new Color(.11f,.18f,.24f);
        cameraObj.AddComponent<AudioListener>();
    }

    GameObject MakeCube(string name, Vector3 pos, Vector3 scale, Color color)
    {
        var o = GameObject.CreatePrimitive(PrimitiveType.Cube);
        o.name = name; o.transform.position = pos; o.transform.localScale = scale;
        o.GetComponent<Renderer>().material.color = color;
        return o;
    }

    void SpawnTarget(Vector3 position)
    {
        var o = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        o.name = "Target"; o.transform.position = position;
        o.GetComponent<Renderer>().material.color = new Color(.95f,.35f,.29f);
        targets.Add(new Target { body = o });
    }

    void Update()
    {
        attackCooldown -= Time.deltaTime;
        Vector2 keys = Vector2.zero;
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) keys.y++;
        if(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) keys.y--;
        if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) keys.x++;
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) keys.x--;
        if(Input.GetKeyDown(KeyCode.Space)) Attack();
        move = (keys + touchMove).normalized;
        if (move.sqrMagnitude > 0.01f)
        {
            Vector3 velocity = new Vector3(move.x,0,move.y) * (5f * Time.deltaTime);
            Vector3 destination = hero.position + velocity;
            destination.x = Mathf.Clamp(destination.x,-14.5f,14.5f);
            destination.z = Mathf.Clamp(destination.z,-14.5f,14.5f);
            // Minimal movement; obstacle collision is not implemented in this first test.
            hero.position = destination;
            hero.rotation = Quaternion.Slerp(hero.rotation,Quaternion.LookRotation(new Vector3(move.x,0,move.y)),12f*Time.deltaTime);
        }
        Vector3 cameraPosition = hero.position + new Vector3(0,9,-10);
        followCamera.transform.position = Vector3.Lerp(followCamera.transform.position,cameraPosition,Mathf.Clamp01(Time.deltaTime*8));
        followCamera.transform.LookAt(hero.position + Vector3.up*.7f);
        foreach (var target in targets)
            if (target.body != null) target.body.transform.Rotate(Vector3.up,25*Time.deltaTime,Space.World);
    }

    void Attack()
    {
        if (attackCooldown > 0) return;
        attackCooldown = .35f;
        Target best = null;
        float bestDist = 2.8f;
        foreach (var target in targets)
        {
            if (target.body == null) continue;
            Vector3 direction = target.body.transform.position - hero.position;
            float dist = direction.magnitude;
            if (dist < bestDist && Vector3.Dot(hero.forward, direction.normalized) > .25f)
            { bestDist = dist; best = target; }
        }
        if (best == null) return;
        best.hp--;
        if (best.hp <= 0)
        {
            Destroy(best.body);
            best.body = null;
            destroyed++;
        }
    }

    void OnGUI()
    {
        float s = Mathf.Min(Screen.width/960f,Screen.height/540f);
        GUI.matrix = Matrix4x4.TRS(Vector3.zero,Quaternion.identity,new Vector3(s,s,1));
        float w = Screen.width/s, h=Screen.height/s;
        if(labelStyle == null)
        {
            labelStyle = new GUIStyle(GUI.skin.label);
            labelStyle.fontSize = 22; labelStyle.fontStyle = FontStyle.Bold;
            labelStyle.normal.textColor = Color.white;
        }
        GUI.Label(new Rect(22,12,600,50),"DROPFLARE  /  UNITY TEST 01",labelStyle);
        GUI.Label(new Rect(22,47,600,45),"Alvos: " + destroyed + "/4    Vida: " + hp + "    Ataque: 3 golpes",labelStyle);
        if(destroyed == 4)
            GUI.Label(new Rect(w/2-180, h/2-30,420,80),"ARENA CONCLUIDA!",labelStyle);
        // For mobile: onscreen D-pad. Buttons only read mouse/touch through IMGUI.
        bool up=GUI.RepeatButton(new Rect(95,h-210,80,72),"▲");
        bool left=GUI.RepeatButton(new Rect(12,h-135,80,72),"◀");
        bool down=GUI.RepeatButton(new Rect(95,h-135,80,72),"▼");
        bool right=GUI.RepeatButton(new Rect(178,h-135,80,72),"▶");
        touchMove = new Vector2((right?1:0)-(left?1:0),(up?1:0)-(down?1:0));
        if(GUI.Button(new Rect(w-160,h-150,135,110),"SOCO")) Attack();
        GUI.Label(new Rect(w-255,h-183,250,35),"Chegue perto do alvo",labelStyle);
    }
}
