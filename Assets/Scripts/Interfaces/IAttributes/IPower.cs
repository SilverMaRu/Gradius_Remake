
namespace Assets.Scripts.Interfases.IAttributes
{
    public interface IPower
    {
        int maxPower { get; set; }
        int basePower { get; set; }
        int currentPower { get; set; }

        void UsePower();
    }

}