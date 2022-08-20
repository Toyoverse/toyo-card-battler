using Card.CardPile.Graveyard;
using Card.DeckSystem;
using Card.QueueSystem;
using CombatSystem.APSystem;
using HealthSystem;
using CardSystem.PlayerHand;
using Player;
using ToyoSystem;
using Zenject;

namespace Globals
{
    public class GameInstaller : MonoInstaller
    {
        public ApModel playerApModel;
        public ApModel enemyApModel;
        public HealthModel playerHealthModel;
        public HealthModel enemyHealthModel;
        
        public override void InstallBindings()
        {
            Container.Bind<PlayerNetworkManager>().FromInstance(FindObjectOfType<PlayerNetworkManager>()).AsSingle();
            Container.Bind<IFullToyo>().FromInstance(FindObjectOfType<FullToyo>()).AsSingle();
            Container.Bind<CardGraveyard>().FromInstance(FindObjectOfType<CardGraveyard>()).AsSingle();
            Container.Bind<IDeck>().FromInstance(FindObjectOfType<Deck>()).AsSingle();
            Container.Bind<IPlayerHand>().FromInstance(FindObjectOfType<PlayerHand>()).AsSingle();
            Container.Bind<PlayerHandUtils>().FromInstance(FindObjectOfType<PlayerHandUtils>()).AsSingle();
            Container.Bind<ApModel>().WithId("PlayerAP").FromInstance(playerApModel).AsTransient();
            Container.Bind<ApModel>().WithId("EnemyAP").FromInstance(enemyApModel).AsTransient();
            Container.Bind<HealthModel>().WithId("PlayerHealth").FromInstance(playerHealthModel).AsTransient();
            Container.Bind<HealthModel>().WithId("EnemyHealth").FromInstance(enemyHealthModel).AsTransient();
            Container.Bind<ComboSystem>().AsSingle();
            
            SignalBusInstaller.Install(Container);
            
            Container.DeclareSignal<PlayerNetworkInitializedSignal>();
            Container.DeclareSignal<CardQueueSystemPresenter.UpdateCardStatusSignal>();

        }
    }
}