using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Element_types
{
    SINGLE_SHOOT_TURRET = 0,
    AREA_SHOOT_TURRET = 1,
    WAVE_TURRET = 2,
    BLUE_LASER = 3,
    RED_LASER = 4,
    SPIKES_TRAP = 5,
    TRAP_THAT_TRAPS = 6,
    SPEED_UP = 7,
    SHIELD = 8,
    MINI_RELOAD = 9,
    LESS_COST = 10,
    EMPTY = 11
}

public class SceneSection : MonoBehaviour {

    public int rank = 1;
    public GameObject[] element_prefabs;

    public int max_turrets = 20;
    public int max_spikes = 20;
    public int max_lasers = 20;
    public int max_boost_zones = 5;

    ArrayList section_elements;

    int current_turrets = 0;
    int current_spikes = 0;
    int current_lasers = 0;
    int current_boost_zones = 0;

    void Start()
    {
        
        section_elements = new ArrayList();

        foreach(Transform t in transform)
        {
            Debug.Log("IM IN THE FIRST FOR");
            if (t.CompareTag("section_elements"))
            {
                Debug.Log("IM COMPARING TAG");
                ElementInScene eis = t.GetComponent<ElementInScene>();
                if (eis != null)
                {
                    
                    section_elements.Add(eis);
                }    
            }
            if(t != transform)
                AddElementsOfScene(t);
        }
         
          foreach (ElementInScene eis in section_elements)
          {
              Debug.Log("IM HERE");
              bool instantiated = false;
              while(!instantiated)
              {

                  int x = Random.Range(0, element_prefabs.Length);
                  Element e_tmp = element_prefabs[x].GetComponent<Element>();

                  while (!eis.types.Contains(e_tmp.type))
                  {
                      x = Random.Range(0, element_prefabs.Length);
                      e_tmp = element_prefabs[x].GetComponent<Element>();
                  }

                  if(e_tmp.type != Element_types.EMPTY)
                  {
                      Debug.Log("IM ABOUT TO CREATE A THING");
                      switch (e_tmp.type)
                      {
                          case Element_types.SINGLE_SHOOT_TURRET:
                              if (current_turrets < max_turrets)
                              {
                                  GenerateElement(e_tmp.gameObject, eis.transform);
                                  instantiated = true;
                              }
                              break;
                          case Element_types.AREA_SHOOT_TURRET:
                              if (current_turrets < max_turrets)
                              {
                                  GenerateElement(e_tmp.gameObject, eis.transform);
                                  instantiated = true;
                              }
                              break;
                          case Element_types.WAVE_TURRET:
                              if (current_turrets < max_turrets)
                              {
                                  GenerateElement(e_tmp.gameObject, eis.transform);
                                  instantiated = true;
                              }
                              break;
                          case Element_types.BLUE_LASER:
                              if (current_lasers < max_lasers)
                              {
                                  GenerateElement(e_tmp.gameObject, eis.transform);
                                  instantiated = true;
                              }
                              break;
                          case Element_types.RED_LASER:
                              if (current_lasers < max_lasers)
                              {
                                  GenerateElement(e_tmp.gameObject, eis.transform);
                                  instantiated = true;
                              }
                              break;
                          case Element_types.SPIKES_TRAP:
                              if (current_spikes < max_spikes)
                              {
                                  GenerateElement(e_tmp.gameObject, eis.transform);
                                  instantiated = true;
                              }
                              break;
                          case Element_types.TRAP_THAT_TRAPS:
                              if (current_spikes < max_spikes)
                              {
                                  GenerateElement(e_tmp.gameObject, eis.transform);
                                  instantiated = true;
                              }
                              break;
                          case Element_types.SPEED_UP:
                              if (current_boost_zones < max_boost_zones)
                              {
                                  GenerateElement(e_tmp.gameObject, eis.transform);
                                  instantiated = true;
                              }
                              break;
                          case Element_types.SHIELD:
                              if (current_boost_zones < max_boost_zones)
                              {
                                  GenerateElement(e_tmp.gameObject, eis.transform);
                                  instantiated = true;
                              }
                              break;
                          case Element_types.MINI_RELOAD:
                              if (current_boost_zones < max_boost_zones)
                              {
                                  GenerateElement(e_tmp.gameObject, eis.transform);
                                  instantiated = true;
                              }
                              break;
                          case Element_types.LESS_COST:
                              if (current_boost_zones < max_boost_zones)
                              {
                                  GenerateElement(e_tmp.gameObject, eis.transform);
                                  instantiated = true;
                              }
                              break;
                       }
                  }
                  else
                  {
                      Debug.Log("Empty Spot");
                      instantiated = true;
                  }
              }
          }
    }

    void GenerateElement(GameObject element,Transform generation_positon)
    {
        Instantiate(element, generation_positon);
    }

    void AddElementsOfScene(Transform trans)
    {
        foreach (Transform t in trans)
        {
            if (t.CompareTag("section_elements"))
            {
                Debug.Log("IM HERE");
                ElementInScene eis = t.GetComponent<ElementInScene>();
                if (eis != null)
                {

                    section_elements.Add(eis);
                }
            }
        }
    }
}
