using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace GoofyGhosts
{
    public class DynamicCollectibleUI : MonoBehaviour
    {
        public GameObject[] images;
        public TextMeshProUGUI[] text;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Scrap"))
            {
                StartCoroutine("UI");
            }
        }

        private IEnumerator UI()
        {
            Color imageTemp = images[0].GetComponent<Image>().color;
            Color textTemp = new Color(226, 226, 226, 0f);

            if (images[0].GetComponent<Image>().color.a < 0.0001f)
            {
                for (int i = 0; i < 10; i++)
                {
                    imageTemp.a += 0.1f;
                    textTemp.a += 0.1f;

                    for (int j = 0; j < 5; j++)
                    {
                        if (j == 0)
                        {
                            images[j].GetComponent<Image>().color = imageTemp;
                        }
                        else
                        {
                            images[j].GetComponent<RawImage>().color = imageTemp;
                        }
                        text[j].color = textTemp;
                    }
                    yield return new WaitForSeconds(0.1f);
                }

                yield return new WaitForSeconds(1.5f);

                for (int i = 0; i < 10; i++)
                {
                    imageTemp.a -= 0.1f;
                    textTemp.a -= 0.1f;

                    for (int j = 0; j < 5; j++)
                    {
                        if (j == 0)
                        {
                            images[j].GetComponent<Image>().color = imageTemp;
                        }
                        else
                        {
                            images[j].GetComponent<RawImage>().color = imageTemp;
                        }
                        text[j].color = textTemp;
                    }
                    yield return new WaitForSeconds(0.1f);
                }
                //print("alpa " + images[0].GetComponent<Image>().color.a);
            }
            else
            {
                yield return new WaitForSeconds(0.01f);
            }
        }
    }
}