#if MM_HDRP
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace MoreMountains.Feedbacks
{
	public class MMSpringPaniniProjectionDistance_HDRP : MMSpringFloatComponent<Volume>
	{
		protected PaniniProjection _paniniProjection;
		
		protected override void Initialization()
		{
			if (Target == null)
			{
				Target = this.gameObject.GetComponent<Volume>();
			}
			Target.profile.TryGet(out _paniniProjection);
			base.Initialization();
		}
		
		public override float TargetFloat
		{
			get => _paniniProjection.distance.value;
			set => _paniniProjection.distance.Override(value);
		}
	}
}
#endif