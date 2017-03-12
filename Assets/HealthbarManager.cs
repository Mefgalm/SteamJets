using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthbarManager : MonoBehaviour {
    public GameObject playerObject;

    public RectTransform foreground;

    private float _healthBarWidth;

	// Use this for initialization
	void Start () {
        _healthBarWidth = foreground.rect.width;
    }
	
	// Update is called once per frame
	void Update () {
        Health health = playerObject.GetComponent<Health>();

        if (health != null)
        {
            //health.currentHealth

            float precentageHp = health.currentHealth / Health.maxHealth;

            foreground.localScale = new Vector3(precentageHp, foreground.localScale.y);
            
            foreground.localPosition = new Vector3((precentageHp * _healthBarWidth - _healthBarWidth) / 2, 0.07f);

            //foreground.rect.Set(foreground.rect.x, foreground.rect.y, newHealthBarWidth, foreground.rect.height);
        }

        transform.rotation = Quaternion.Euler(Vector3.zero);
        transform.position = new Vector3(playerObject.transform.position.x, playerObject.transform.position.y + 4f, 0);        
    }
}
