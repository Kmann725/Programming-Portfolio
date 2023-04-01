/*****************************************************************************
// File Name :         WorldChangeEvents.cs
// Author :            Kyle Manning
// Contact :           khmanning@mail.bradley.edu
// Creation Date :     2022-10-09
//
// Updated by :        Kyle Manning 10/27/2022
//
// Brief Description : Contains the functionality for each world change
//                     event when they are called
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Cinemachine;

public class WorldChangeEvents : NetworkBehaviour
{
    // Objects used in changing the grass
    [Header("Grass Assets")]
    [Tooltip("Texture 2D used as the texture for the Terrain Layer")]
    public Texture2D grass;
    [Tooltip("Texture 2D used for the grass around the ruins")]
    public Texture2D ruinsGrass;
    [Tooltip("Texture 2D used for the grass around the forest")]
    public Texture2D forestGrass;
    [Tooltip("Texture 2D used for the grass around the quarry")]
    public Texture2D quarryGrass;
    [Tooltip("Terrain object in the scene")]
    public Terrain terrain;
    [Tooltip("Material for the grass in the map's center")]
    public Material movingGrass;
    [Tooltip("Material for the grass around the ruins")]
    public Material ruinsGrassBlades;
    [Tooltip("Material for the grass in the forest")]
    public Material forestGrassBlades;
    [Tooltip("Material for the grass around the quarry")]
    public Material quarryGrassBlades;

    // Points where the dynamic shader's change radius originates
    [Header("Dynamic Change Shader Origins")]
    public ShaderInteractorPosition ruinsMain;
    public ShaderInteractorPosition ruinsSub1;
    public ShaderInteractorPosition ruinsSub2;
    public ShaderInteractorPosition forestMain;
    public ShaderInteractorPosition quarryMain;
    public ShaderInteractorPosition quarrySub;
    public ShaderInteractorPosition castleMain;

    // Objects which are switched out in the ruins local world change
    [Header("Ruins Local Objects")]
    public GameObject[] goodRuins;
    public GameObject[] evilRuins;
    public GameObject[] ruinSigns;
    public GameObject[] ruinSkulls;

    // Objects which are switched out in the forest local world change
    [Header("Forest Local Objects")]
    public GameObject[] forestGoodRuins;
    public GameObject[] forestEvilRuins;
    public GameObject[] forestSigns;
    public GameObject[] forestSkulls;
    private GameObject flowers;

    // Objects which are switched out in the quarry local world change
    [Header("Quarry Local Objects")]
    public GameObject[] quarrySigns;
    public GameObject[] quarrySkulls;

    // Objects which are switched out in the castle local world change
    [Header("Castle Objects")]
    public GameObject[] goodCastle;
    public GameObject[] evilCastle;
    public GameObject[] castleSigns;
    public GameObject[] castleSkull;

    // Used for post-processing
    [Header("")]
    private Volume gVolume;
    private ColorAdjustments colorAdjustments;

    // Skyboxes to change between
    [Header("Skyboxes")]
    [Tooltip("Skybox that should be visible at the start of the level")]
    public Material skybox1;
    [Tooltip("Skybox to be changed to during the first world change")]
    public Material skybox2;
    [Tooltip("Skybox to be changed to during the second world change")]
    public Material skybox3;
    [Tooltip("Skybox to be changed to during the third world change")]
    public Material skybox4;
    [Tooltip("Skybox to be changed to during the fourth world change")]
    public Material skybox5;
    private Material newSkybox;

    // Materials used for water changes
    [Header("Water")]
    [Tooltip("Water planes in the scene")]
    public MeshRenderer[] waterways;
    [Tooltip("Color water should be at the start of the level")]
    public Material water;

    // Crystal changes
    [Header("Crystals")]
    [Tooltip("Material that crystals use")]
    public Material crystalMaterial;
    [Tooltip("Base color that the crystals should start with")]
    public Color crystalStart;
    [Tooltip("Base color that the crystals should end with")]
    public Color crystalEnd;
    [Tooltip("Color the crystal glow should start with")]
    public Color glowStart;
    [Tooltip("Color the crystal glow should end with")]
    public Color glowEnd;

    // Ambient World Particles
    [Header("Ambient Particles")]
    [Tooltip("The particle systems for the world.")]
    public GameObject[] ambientParticles;
    private ParticleSystem setParticles;

    // Tree changes
    [Header("Trees")]
    [Tooltip("Material used for the trees")]
    public Material treeTrunk;
    public Material treeLeaves;
    [Tooltip("Color the trees should start with")]
    public Color leavesStart;
    [Tooltip("Color the trees should end with")]
    public Color leavesEnd;
    // All tree leaves in the scene
    public GameObject[] leaves;
    // All leaf particle effects in the scene
    public GameObject[] leafExplosion;

    // GameObjects and Materials needed for changing the trees in the ruins
    private GameObject[] ruinTrees;
    private GameObject[] ruinLeaves;
    private GameObject[] ruinLeafHolder;
    private GameObject[] ruinLeafEffect;
    private Material ruinTreeMat;
    private Material ruinLeavesMat;

    // GameObjects and Materials needed for changing the trees in the forest
    private GameObject[] forestTrees;
    private GameObject[] forestLeaves;
    private GameObject[] forestLeafHolder;
    private GameObject[] forestLeafEffect;
    private Material forestTreeMat;
    private Material forestLeavesMat;

    // GameObjects and Materials needed for changing the trees in the quarry
    private GameObject[] quarryTrees;
    private GameObject[] quarryLeaves;
    private GameObject[] quarryLeafHolder;
    private GameObject[] quarryLeafEffect;
    private Material quarryTreeMat;
    private Material quarryLeavesMat;

    // Used for changing bushes around castle
    private GameObject[] castleLeaves;
    private Material castleLeavesMat;

    // Colors used for grass changes
    [Header("Ground Colors")]
    [Tooltip("Color the ground will start the game with")]
    public Color startColor;
    [Tooltip("Color the ground will end at with the final world change")]
    public Color endColor;
    [Header("Grass Blade Colors")]
    [Tooltip("Color the grass will start the game with")]
    public Color bladesStart;
    [Tooltip("Color the grass will end at with the final world change")]
    public Color bladesEnd;
    public float grassVelocity;

    // Colors used for post-processing changes
    [Header("Post-Processing Colors")]
    public Color postStart;
    public Color postEnd;

    [Header("Energy Wave")]
    public Animator energyWave;

    // Screen shake variables
    [Header("Screen Shake")]
    [Tooltip("The intensity of the screen shake")]
    public float shakeForce;
    private CinemachineImpulseSource impulse;

    [Header("World Change Gradient")]
    [Range(0f, 1f)]
    public float t;

    // Orb
    [Header("Orb")]
    private OrbMovement orb;

    [Header("Orb Particle Effects")]
    public ParticleSystem[] orbVfx;
    public GameObject orbBeacon1;
    public GameObject[] orbBeacon2;
    public GameObject[] orbBeacon3;

    public GameObject winScreen;

    public ParticleSystem[] stormClouds;

    //Gabe: UI
    private OrbMoveTextController omtc;

    public static WorldChangeEvents instance;

    public BossUnicorn boss;

    /// <summary>
    /// Assigns functions as listeners to each world change event in gameController
    /// and sets the dynamic assets to their default appearances
    /// </summary>
    void Start()
    {
        // Sets world change functions as event listeners
        GlobalValues.gameController.worldChange1.AddListener(WorldChange1ClientRpc);
        GlobalValues.gameController.worldChange2.AddListener(WorldChange2ClientRpc);
        GlobalValues.gameController.worldChange3.AddListener(WorldChange3ClientRpc);
        GlobalValues.gameController.worldChange4.AddListener(WorldChange4ClientRpc);

        // Gets orb for movement calls
        orb = GameObject.FindGameObjectWithTag("base").GetComponent<OrbMovement>();

        // Gets the ambient world particles
        ambientParticles[0] = GameObject.FindWithTag("Ambient Particle 1");
        ambientParticles[1] = GameObject.FindWithTag("Ambient Particle 2");
        ambientParticles[2] = GameObject.FindWithTag("Ambient Particle 3");
        ambientParticles[3] = GameObject.FindWithTag("Ambient Particle 4");

        // sets the intial ambient particles
        setParticles = ambientParticles[0].GetComponent<ParticleSystem>();
        setParticles.Play();

        // assigns the game controller's impulse sources
        impulse = GetComponent<CinemachineImpulseSource>();

        // Creates new skybox and sets it to the current skybox
        newSkybox = new Material(skybox1);
        RenderSettings.skybox = newSkybox;        

        // Gets the post-processing object in the scene
        gVolume = GetComponent<Volume>();

        // Gets the color adjustment setting from the post-processing Volume
        if(gVolume.profile.TryGet(out colorAdjustments))
        {

        }

        // Sets the starting colors for the crystal material
        crystalMaterial.SetColor("_BaseColor", crystalStart);
        crystalMaterial.SetColor("_GlowColor", glowStart);

        // Sets the starting color for the trees
        treeTrunk.color = leavesStart;
        treeLeaves.SetFloat("_T", 0);

        // Gets all tree leaves and particles effects in the scene
        leaves = GameObject.FindGameObjectsWithTag("treeleaves");
        leafExplosion = GameObject.FindGameObjectsWithTag("leafexplosion");

        // Creates a cloned material and assigns it to all the ruin trees
        ruinTrees = GameObject.FindGameObjectsWithTag("ruins tree");
        ruinTreeMat = new Material(treeTrunk);
        foreach (GameObject tree in ruinTrees)
        {
            tree.GetComponent<MeshRenderer>().material = ruinTreeMat;
        }

        // Finds the ruin tree particle effects and leaves; also creates a cloned material and assigns it
        // to the leaves
        ruinLeaves = GameObject.FindGameObjectsWithTag("ruin leaf");
        ruinLeafHolder = GameObject.FindGameObjectsWithTag("ruin leaf holder");
        ruinLeafEffect = GameObject.FindGameObjectsWithTag("ruin effect");
        ruinLeavesMat = new Material(treeLeaves);
        foreach (GameObject leaf in ruinLeaves)
        {
            leaf.GetComponent<MeshRenderer>().material = ruinLeavesMat;
        }

        // Creates a cloned material and assigns it to all the forest trees
        forestTrees = GameObject.FindGameObjectsWithTag("forest tree");
        forestTreeMat = new Material(treeTrunk);
        foreach (GameObject tree in forestTrees)
        {
            tree.GetComponent<MeshRenderer>().material = forestTreeMat;
        }

        // Finds the forest tree particle effects and leaves; also creates a cloned material and assigns it
        // to the leaves
        forestLeaves = GameObject.FindGameObjectsWithTag("forest leaf");
        forestLeafHolder = GameObject.FindGameObjectsWithTag("forest leaf holder");
        forestLeafEffect = GameObject.FindGameObjectsWithTag("forest effect");
        forestLeavesMat = new Material(treeLeaves);
        foreach (GameObject leaf in forestLeaves)
        {
            leaf.GetComponent<MeshRenderer>().material = forestLeavesMat;
        }

        // Creates a cloned material and assigns it to all the quarry trees
        quarryTrees = GameObject.FindGameObjectsWithTag("quarry tree");
        quarryTreeMat = new Material(treeTrunk);
        foreach (GameObject tree in quarryTrees)
        {
            tree.GetComponent<MeshRenderer>().material = quarryTreeMat;
        }

        // Finds the quarry tree particle effects and leaves; also creates a cloned material and assigns it
        // to the leaves
        quarryLeaves = GameObject.FindGameObjectsWithTag("quarry leaf");
        quarryLeafHolder = GameObject.FindGameObjectsWithTag("quarry leaf holder");
        quarryLeafEffect = GameObject.FindGameObjectsWithTag("quarry effect");
        quarryLeavesMat = new Material(treeLeaves);
        foreach (GameObject leaf in quarryLeaves)
        {
            leaf.GetComponent<MeshRenderer>().material = quarryLeavesMat;
        }

        castleLeaves = GameObject.FindGameObjectsWithTag("castle leaf");
        castleLeavesMat = new Material(treeLeaves);
        foreach (GameObject leaf in castleLeaves)
        {
            leaf.GetComponent<MeshRenderer>().material = castleLeavesMat;
        }

        // Sets the ground in the midfield to the default color
        grass.SetPixel(0, 0, (startColor * (1 - t)) + (endColor * t));
        grass.Apply();

        // Sets the ground around the ruins to the default color
        ruinsGrass.SetPixel(0, 0, (startColor * (1 - t)) + (endColor * t));
        ruinsGrass.Apply();

        // Sets the ground around the forest to the default color
        forestGrass.SetPixel(0, 0, (startColor * (1 - t)) + (endColor * t));
        forestGrass.Apply();

        // Sets the ground around the quarry to the default color
        quarryGrass.SetPixel(0, 0, (startColor * (1 - t)) + (endColor * t));
        quarryGrass.Apply();

        // Sets the grass in the midfield to the default color
        movingGrass.SetColor("_Color", startColor);
        movingGrass.SetColor("_Color_2", bladesStart);

        // Sets the grass near the ruins to the default color
        ruinsGrassBlades.SetColor("_Color", startColor);
        ruinsGrassBlades.SetColor("_Color_2", bladesStart);

        // Sets the grass in the forest to the default color
        forestGrassBlades.SetColor("_Color", startColor);
        forestGrassBlades.SetColor("_Color_2", bladesStart);

        // Sets the grass near the quarry to the default color
        quarryGrassBlades.SetColor("_Color", startColor);
        quarryGrassBlades.SetColor("_Color_2", bladesStart);

        flowers = GameObject.FindGameObjectWithTag("flower");

        omtc = GameObject.Find("Orb Moved UI").GetComponent<OrbMoveTextController>();

        AkSoundEngine.PostEvent("Play_ambience", gameObject);

        instance = this;
    }    

    // NOTE: 'changes' parameter for the coroutine should be a number between 1 to 100
    //       as it represents how many steps of 0.01 the world change gradient should take
    /// <summary>
    /// Performs the first wave of changes to the world
    /// </summary>
    [ClientRpc]
    public void WorldChange1ClientRpc()
    {
        StartCoroutine(WorldChange(30, skybox2, 1, 1));
        StartCoroutine(RuinsLocalChange(30));

        AkSoundEngine.SetSwitch("Evilness_Level", "Early", gameObject);

        // Changes the active ambient particle system
        setParticles.Stop();
        setParticles = ambientParticles[1].GetComponent<ParticleSystem>();
        setParticles.Play();

        orbBeacon1.SetActive(false);
    }

    // NOTE: 'changes' parameter for the coroutine should be a number between 1 to 100
    //       as it represents how many steps of 0.01 the world change gradient should take
    /// <summary>
    /// Performs the second wave of changes to the world
    /// </summary>
    [ClientRpc]
    public void WorldChange2ClientRpc()
    {
        shakeForce += (shakeForce / 2);
        StartCoroutine(WorldChange(30, skybox3, 2, 2));
        StartCoroutine(ForestLocalChange(30));

        AkSoundEngine.SetSwitch("Evilness_Level", "Mid", gameObject);

        foreach (GameObject part in orbBeacon2)
        {
            part.SetActive(false);
        }
    }

    // NOTE: 'changes' parameter for the coroutine should be a number between 1 to 100
    //       as it represents how many steps of 0.01 the world change gradient should take
    /// <summary>
    /// Performs the third wave of changes to the world
    /// </summary>
    [ClientRpc]
    public void WorldChange3ClientRpc()
    {
        shakeForce += (shakeForce / 2);
        StartCoroutine(WorldChange(20, skybox4, 3, 3));
        StartCoroutine(QuarryLocalChange(20));

        AkSoundEngine.SetSwitch("Evilness_Level", "Late", gameObject);

        // Changes the active ambient particle system
        setParticles.Stop();
        setParticles = ambientParticles[2].GetComponent<ParticleSystem>();
        setParticles.Play();

        foreach (GameObject part in orbBeacon3)
        {
            part.SetActive(false);
        }
    }

    // NOTE: 'changes' parameter for the coroutine should be a number between 1 to 100
    //       as it represents how many steps of 0.01 the world change gradient should take
    /// <summary>
    /// Performs the fourth wave of changes to the world
    /// </summary>
    [ClientRpc]
    public void WorldChange4ClientRpc()
    {
        shakeForce += (shakeForce / 2);
        StartCoroutine(WorldChange(20, skybox5, 4, 4));
        StartCoroutine(CastleLocalChange(20));

        // Changes the active ambient particle system
        setParticles.Stop();
        setParticles = ambientParticles[3].GetComponent<ParticleSystem>();
        setParticles.Play();
    }

    /// <summary>
    /// Handles complete world change in the area around the ruins orb location
    /// </summary>
    /// <param name="changes">How many times shifts should occur in the change</param>
    /// <returns></returns>
    IEnumerator RuinsLocalChange(int changes)
    {
        changes = changes * 8;

        // This wait lines the changes up with the other partial changes
        yield return new WaitForSeconds(3f);

        // Activates evil ruin objects that will be made to appear
        foreach (GameObject obj in evilRuins)
        {
            obj.SetActive(true);
        }

        // Activates the unicorn skulls that will be made to appear
        foreach (GameObject obj in ruinSkulls)
        {
            obj.SetActive(true);
        }

        for (int i = 1; i <= changes; i++)
        {
            yield return new WaitForSeconds(0.025f);

            float change = (1f / changes) * i;

            // changes the color of the ground around the ruins
            ruinsGrass.SetPixel(0, 0, (startColor * (1 - change)) + (endColor * change));
            ruinsGrass.Apply();

            // changes color of the grass blades around the ruins
            Color tempRoots = Color.Lerp(startColor, endColor, change);
            Color tempBlades = Color.Lerp(bladesStart, bladesEnd, change);
            ruinsGrassBlades.SetColor("_Color", tempRoots);
            ruinsGrassBlades.SetColor("_Color_2", tempBlades);

            // changes the color of the tree bark and leaves
            Color tempTrunk = Color.Lerp(leavesStart, leavesEnd, change);
            ruinTreeMat.color = tempTrunk;
            ruinLeavesMat.SetFloat("_T", change);

            // Increases radius of shader object which makes world objects appear/disappear
            if (ruinsMain.radius <= 69.5f)
            {
                ruinsMain.radius += 0.5f;
            }
            
            // Increases radius of backup shader objects to cover extra bits of objects
            if (ruinsMain.radius >= 46f && ruinsSub1.radius <= 73f)
            {
                ruinsSub1.radius += 0.5f;
            }
            if (ruinsMain.radius >= 52f && ruinsSub2.radius <= 25f)
            {
                ruinsSub2.radius += 0.5f;
            }
        }

        // Plays the leaf explosion effect and disables the leaves for every tree near the ruins
        for (int i = 0; i < ruinLeafHolder.Length; i++)
        {
            ruinLeafEffect[i].GetComponent<ParticleSystem>().Play();
            ruinLeafHolder[i].SetActive(false);
        }

        // Hides ruins that were disappeared by the dynamic shader
        foreach (GameObject obj in goodRuins)
        {
            obj.SetActive(false);
        }

        // Hides the signs that were disappeared by the dynamic shader
        foreach (GameObject obj in ruinSigns)
        {
            obj.SetActive(false);
        }
    }

    /// <summary>
    /// Handles complete world change in the area around the forest orb location
    /// </summary>
    /// <param name="changes">How many times shifts should occur in the change</param>
    /// <returns></returns>
    IEnumerator ForestLocalChange(int changes)
    {
        changes = changes * 8;

        // Creates new colors as start points for color changes using lerp
        Color newStart = (startColor * (1 - t)) + (endColor * t);
        Color newRoot = forestGrassBlades.GetColor("_Color");
        Color newBlades = forestGrassBlades.GetColor("_Color_2");
        Color newTrunk = forestTreeMat.color;

        flowers.SetActive(false);

        // This wait lines the changes up with the other partial changes
        yield return new WaitForSeconds(3f);

        // Activates evil ruin objects that will be made to appear
        foreach (GameObject obj in forestEvilRuins)
        {
            obj.SetActive(true);
        }

        // Activates the unicorn skulls that will be made to appear
        foreach (GameObject obj in forestSkulls)
        {
            obj.SetActive(true);
        }

        for (int i = 0; i <= changes; i++)
        {
            yield return new WaitForSeconds(0.025f);

            float change = (1f / changes) * i;

            // changes the color of the ground around the forest
            forestGrass.SetPixel(0, 0, (newStart * (1 - change)) + (endColor * change));
            forestGrass.Apply();

            // changes color of the grass blades around the forest
            Color tempRoots = Color.Lerp(newRoot, endColor, change);
            Color tempBlades = Color.Lerp(newBlades, bladesEnd, change);
            forestGrassBlades.SetColor("_Color", tempRoots);
            forestGrassBlades.SetColor("_Color_2", tempBlades);

            // changes the color of the tree bark and leaves
            Color tempTrunk = Color.Lerp(newTrunk, leavesEnd, change);
            forestTreeMat.color = tempTrunk;
            forestLeavesMat.SetFloat("_T", change);

            // Increases radius of shader object which makes world objects appear/disappear
            if (forestMain.radius <= 179.5f)
            {
                forestMain.radius += 0.75f;
            }
        }

        // Plays the leaf explosion effect and disables the leaves for every tree in the forest
        for (int i = 0; i < forestLeafHolder.Length; i++)
        {
            forestLeafEffect[i].GetComponent<ParticleSystem>().Play();
            forestLeafHolder[i].SetActive(false);
        }

        // Hides ruins that were disappeared by the dynamic shader
        foreach (GameObject obj in forestGoodRuins)
        {
            obj.SetActive(false);
        }

        // Hides the signs that were disappeared by the dynamic shader
        foreach (GameObject obj in forestSigns)
        {
            obj.SetActive(false);
        }
    }

    /// <summary>
    /// Handles complete world change in the area around the quarry orb location
    /// </summary>
    /// <param name="changes">How many times shifts should occur in the change</param>
    /// <returns></returns>
    IEnumerator QuarryLocalChange(int changes)
    {
        changes = changes * 8;

        // Creates new colors as start points for color changes using lerp
        Color newStart = (startColor * (1 - t)) + (endColor * t);
        Color newRoot = quarryGrassBlades.GetColor("_Color");
        Color newBlades = quarryGrassBlades.GetColor("_Color_2");
        Color newTrunk = quarryTreeMat.color;
        Color newCrystal = crystalMaterial.GetColor("_BaseColor");
        Color newGlow = crystalMaterial.GetColor("_GlowColor");

        // This wait lines the changes up with the other partial changes
        yield return new WaitForSeconds(3f);

        // Activates the unicorn skulls that will be made to appear
        foreach (GameObject obj in quarrySkulls)
        {
            obj.SetActive(true);
        }

        for (int i = 0; i <= changes; i++)
        {
            yield return new WaitForSeconds(0.025f);

            float change = (1f / changes) * i;

            // changes the color of the ground around the quarry
            quarryGrass.SetPixel(0, 0, (newStart * (1 - change)) + (endColor * change));
            quarryGrass.Apply();

            // changes color of the grass blades around the quarry
            Color tempRoots = Color.Lerp(newRoot, endColor, change);
            Color tempBlades = Color.Lerp(newBlades, bladesEnd, change);
            quarryGrassBlades.SetColor("_Color", tempRoots);
            quarryGrassBlades.SetColor("_Color_2", tempBlades);

            // changes the color of the tree bark and leaves
            Color tempTrunk = Color.Lerp(newTrunk, leavesEnd, change);
            quarryTreeMat.color = tempTrunk;
            quarryLeavesMat.SetFloat("_T", change);

            // changes the color the the crystals
            Color tempCol = Color.Lerp(newCrystal, crystalEnd, change);
            Color tempGlow = Color.Lerp(newGlow, glowEnd, change);
            crystalMaterial.SetColor("_BaseColor", tempCol);
            crystalMaterial.SetColor("_GlowColor", tempGlow);

            // Increases radius of shader object which makes world objects appear/disappear
            if (quarryMain.radius <= 120f)
            {
                quarryMain.radius += 0.85f;
            }

            // Increases radius of backup shader object to cover extra bits of objects
            if (quarryMain.radius >= 95f && quarrySub.radius <=14.5f)
            {
                quarrySub.radius += 0.75f;
            }
        }

        // Plays the leaf explosion effect and disables the leaves for every tree near the quarry
        for (int i = 0; i < quarryLeafHolder.Length; i++)
        {
            quarryLeafEffect[i].GetComponent<ParticleSystem>().Play();
            quarryLeafHolder[i].SetActive(false);
        }

        // Hides the signs that were disappeared by the dynamic shader
        foreach (GameObject obj in quarrySigns)
        {
            obj.SetActive(false);
        }
    }

    /// <summary>
    /// Handles complete world change in the area around the castle orb location
    /// </summary>
    /// <param name="changes">How many times shifts should occur in the change</param>
    /// <returns></returns>
    IEnumerator CastleLocalChange(int changes)
    {
        changes = changes * 8;

        // This wait lines the changes up with the other partial changes
        yield return new WaitForSeconds(3f);

        // Activates evil ruin and castle objects that will be made to appear
        foreach (GameObject obj in evilCastle)
        {
            obj.SetActive(true);
        }

        // Activates the unicorn skulls that will be made to appear
        foreach (GameObject obj in castleSkull)
        {
            obj.SetActive(true);
        }

        for (int i = 0; i <= changes; i++)
        {
            yield return new WaitForSeconds(0.025f);

            // Increases radius of shader object which makes world objects appear/disappear
            if (castleMain.radius < 230)
            {
                castleMain.radius += 1.5f;
            }
        }

        // Hides ruins and castle that were disappeared by the dynamic shader
        foreach (GameObject obj in goodCastle)
        {
            obj.SetActive(false);
        }

        // Hides the signs that were disappeared by the dynamic shader
        foreach (GameObject obj in castleSigns)
        {
            obj.SetActive(false);
        }
    }

    /// <summary>
    /// Changes the color of the grass, shifts between skyboxes, changes
    /// post-processing colors gradually over time
    /// </summary>
    /// <param name="changes">How many times shifts should occur in the change</param>
    /// <param name="skybox">The skybox that is desired to transition to</param>
    /// <param name="moveSet">Int sent to OrbMovement.cs to determine which movement should be done</param>
    /// <param name="stage">Which world change is occuring</param>
    /// <returns></returns>
    IEnumerator WorldChange(int changes, Material skybox, int moveSet, int stage)
    {
        // Plays the energy wave effect
        energyWave.SetTrigger("Play");
        foreach (ParticleSystem particle in orbVfx)
        {
            particle.Play();
        }
        AkSoundEngine.PostEvent("World_change", gameObject);

        yield return new WaitForSeconds(0.3f);

        //omtc.SetSF(true);
        if (IsClient)
        {
            StartWaveEndServerRpc();
        }

        // Activates the storm clouds
        foreach (ParticleSystem cloud in stormClouds)
        {
            cloud.Play();
        }

        yield return new WaitForSeconds(2.7f);

        // Starts orb movement along a set of waypoints
        orb.OrbMove(moveSet);

        // Doubles number of changes for the sake of smoother changes
        changes = changes * 4;
        // Used for lerping skyboxes
        float skyLerp = 0;

        for (int i = 1; i <= changes; i++)
        {
            yield return new WaitForSeconds(0.05f);
            t += 0.0025f;

            // Sets a random direction to shake the camera towards
            impulse.m_DefaultVelocity = new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), Random.Range(-2, 5));
            impulse.GenerateImpulseWithForce(shakeForce);

            // Changes the color of the trees
            Color tempTrunk = Color.Lerp(leavesStart, leavesEnd, t);
            treeTrunk.color = tempTrunk;
            treeLeaves.SetFloat("_T", t);
            castleLeavesMat.SetFloat("_T", t);

            // Changes the ground color
            grass.SetPixel(0, 0, (startColor * (1 - t)) + (endColor * t));
            grass.Apply();

            // For Moving Grass
            Color tempRoots = Color.Lerp(startColor, endColor, t);
            Color tempBlades = Color.Lerp(bladesStart, bladesEnd, t);
            movingGrass.SetColor("_Color", tempRoots);
            movingGrass.SetColor("_Color_2", tempBlades);

            // Changes the water color
            foreach (MeshRenderer wPlane in waterways)
            {
                wPlane.material.SetFloat("_change", t);
            }

            // Performs the nested lines if it is the first world change
            if (stage < 2)
            {
                // Partially changes the color of the ground and grass in the forest
                forestGrass.SetPixel(0, 0, (startColor * (1 - t)) + (endColor * t));
                forestGrass.Apply();
                forestGrassBlades.SetColor("_Color", tempRoots);
                forestGrassBlades.SetColor("_Color_2", tempBlades);

                // Partially changes the color of the trees in the forest
                forestTreeMat.color = tempTrunk;
                forestLeavesMat.SetFloat("_T", t);
            }

            // Performs the nexted lines if it is the first or second world changes
            if (stage < 3)
            {
                // Partially changes the color of the crystals
                Color tempCol = Color.Lerp(crystalStart, crystalEnd, t);
                Color tempGlow = Color.Lerp(glowStart, glowEnd, t);
                crystalMaterial.SetColor("_BaseColor", tempCol);
                crystalMaterial.SetColor("_GlowColor", tempGlow);

                // Partially changes the color of the ground and grass around the quarry
                quarryGrass.SetPixel(0, 0, (startColor * (1 - t)) + (endColor * t));
                quarryGrass.Apply();
                quarryGrassBlades.SetColor("_Color", tempRoots);
                quarryGrassBlades.SetColor("_Color_2", tempBlades);

                // Partially changes the color of the trees around the quarry partially
                quarryTreeMat.color = tempTrunk;
                quarryLeavesMat.SetFloat("_T", t);
            }

            // Changes the post-processing color
            colorAdjustments.colorFilter.value = postStart * (1 - t) + (postEnd * t);

            // Changes the skybox
            skyLerp = (1f / changes) * i;
            RenderSettings.skybox.Lerp(newSkybox, skybox, skyLerp);
        }
        // Changes the current skybox material to the passed skybox
        newSkybox = skybox;

        // Deactivates the storm clouds
        foreach(ParticleSystem cloud in stormClouds)
        {
            cloud.Stop();
        }

        // Deactivates the orb's movement beacon
        foreach (ParticleSystem particle in orbVfx)
        {
            particle.Stop();
        }

        // After the final world change, the leaves on the trees disappear and play
        // a particle effect for them falling off
        if (t > 0.9f)
        {
            for (int i = 0; i < leaves.Length; i++)
            {
                leafExplosion[i].GetComponent<ParticleSystem>().Play();
                leaves[i].SetActive(false);
            }

            //StartCoroutine(boss.bossBeat());

            yield return new WaitForSeconds(5f);

            winScreen.SetActive(true);

            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            PauseMenuBehavior.PauseMenu.CanPause = false;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void StartWaveEndServerRpc()
    {
        omtc.SetSFClientRpc(true);
    }
}
