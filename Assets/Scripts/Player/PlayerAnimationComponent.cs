using Dreamteck.Splines;
using UnityEngine;

namespace Player
{
    public class PlayerAnimationComponent : BasePlayerComponent
    {
        [SerializeField] private Animator mAnimator;
        
        protected override void OnLevelFinish()
        {
            base.OnLevelFinish();
            PlayAnimation("HandAnim");
        }

        protected override void OnNextLevel()
        {
            base.OnNextLevel();
            PlayAnimation("Idle");
        }

        public void PlayAnimation(string animKey)
        {
            mAnimator.CrossFade(animKey, 0f);
        }
    }
}