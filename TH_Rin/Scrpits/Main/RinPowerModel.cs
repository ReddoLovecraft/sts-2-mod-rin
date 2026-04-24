using BaseLib.Abstracts;

namespace TH_Rin.Scripts.Main
{
	public abstract class RinPowerModel : CustomPowerModel
	{
		public virtual void Trigger()
		{
			Flash();
		}
	}
	
}
