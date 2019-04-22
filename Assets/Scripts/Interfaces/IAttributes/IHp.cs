
namespace Assets.Scripts.Interfases.IAttributes
{
    public interface IHp
    {
        int IMaxHp { get; set; }
        int IBaseHp { get; set; }
        int ICurrentHp { get; set; }
        bool isInvincible { get; set; }
        bool isDie { get; set; }

        void Hurt(int damage);
        void Die();
    }

}