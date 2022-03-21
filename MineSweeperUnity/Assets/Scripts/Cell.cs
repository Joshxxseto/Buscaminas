using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public bool hasMine, isCovered;
    public Sprite[] numbersTextures;
    public Sprite mineTexture,flagTexture,panelTexture;
    public float mineProbability=0.15f;

    void Start()
    {
        isCovered = true;
        hasMine = (Random.value < mineProbability);
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonUp(1))
        {
            if (this.GetComponent<SpriteRenderer>().sprite.name == flagTexture.name)
            {
                SetTexture(panelTexture);
            } else if (this.GetComponent<SpriteRenderer>().sprite.name == panelTexture.name)
            {
                SetTexture(flagTexture);
            }
            
        }
    }
    private void OnMouseUpAsButton()
    {
        if (this.GetComponent<SpriteRenderer>().sprite.name != flagTexture.name)
        {
            if (!hasMine)
            {
                SetTexture(myMines());
                if (myMines() == 0)
                {
                    GridHelper.sharedIsntance.FloodFill((int)this.transform.position.x, (int)this.transform.position.y, new bool[GridHelper.width, GridHelper.height]);
                }
            }
            else
            {
                //Send GameOver message 
                GridHelper.sharedIsntance.UncoverAllTheMines();
            }
            if (GridHelper.HasTheGameEnded())
            {
                Debug.Log("El juego ha terminado");
            }
        }
    }

    public int myMines()
    {
        return GridHelper.sharedIsntance.CountAdjacentMines((int)this.transform.position.x, (int)this.transform.position.y);
    }

    public void SetTexture(int myNumber)
    {
        if (!hasMine)
        {
            this.GetComponent<SpriteRenderer>().sprite = numbersTextures[myNumber];
        }
        else
        {
            this.GetComponent<SpriteRenderer>().sprite = mineTexture;
        }
        isCovered = false;
    }
    public void SetTexture(Sprite spriteName)
    {
        this.GetComponent<SpriteRenderer>().sprite = spriteName;
    }
}
