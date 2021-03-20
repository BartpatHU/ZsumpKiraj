using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinChanger : MonoBehaviour
{
    public AnimatorOverrideController default_override;
    public AnimatorOverrideController green_override;
    public AnimatorOverrideController blue_override;
    public AnimatorOverrideController yellow_override;
    public AnimatorOverrideController red_override;
    public AnimatorOverrideController white_override;
    public AnimatorOverrideController cyan_override;
    public AnimatorOverrideController pink_override;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PinkSkin()
    {
        GetComponent<Animator>().runtimeAnimatorController = pink_override as RuntimeAnimatorController;
    }

    public void CyanSkin()
    {
        GetComponent<Animator>().runtimeAnimatorController = cyan_override as RuntimeAnimatorController;
    }

    public void WhitetSkin()
    {
        GetComponent<Animator>().runtimeAnimatorController = white_override as RuntimeAnimatorController;
    }

    public void RedSkin()
    {
        GetComponent<Animator>().runtimeAnimatorController = red_override as RuntimeAnimatorController;
    }

    public void YellowSkin()
    {
        GetComponent<Animator>().runtimeAnimatorController = yellow_override as RuntimeAnimatorController;
    }

    public void BlueSkin()
    {
        GetComponent<Animator>().runtimeAnimatorController = blue_override as RuntimeAnimatorController;
    }

    public void DefaultSkin()
    {
        GetComponent<Animator>().runtimeAnimatorController = default_override as RuntimeAnimatorController;
    }

    public void GreenSkin()
    {
        GetComponent<Animator>().runtimeAnimatorController = green_override as RuntimeAnimatorController;
    }
}
