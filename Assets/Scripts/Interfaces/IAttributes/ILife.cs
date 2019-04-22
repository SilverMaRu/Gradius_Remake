
namespace Assets.Scripts.Interfases.IAttributes
{
    public interface ILife
    {
        int maxLife { get; set; }
        int baseLife { get; set; }
        int currentLife { get; set; }

        void Relive();
    }

}