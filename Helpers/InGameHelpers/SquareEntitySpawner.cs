// using Godot;

// public partial class SquareEntitySpawner : Node
// {
//     private PackedScene _squareScene;
//     private PackedScene _badBlockScene;
//     private PackedScene _prizeBoxScene;
//     private MissedClickHandler _missedClickHandler;
//     private int _squareSpawnCount;

//     public SquareEntitySpawner(PackedScene squareScene, PackedScene badBlockScene, PackedScene prizeBoxScene, MissedClickHandler missedClickHandler)
//     {
//         _squareScene = squareScene;
//         _badBlockScene = badBlockScene;
//         _prizeBoxScene = prizeBoxScene;
//         _missedClickHandler = missedClickHandler;
//     }

//     public void SpawnSquareEntity(Viewport viewport, Hud hud, Action onSquareClicked, Action onBadBlockClicked, Action onPrizeBoxClicked)
//     {
//         var rng = new RandomNumberGenerator();
//         float randomValue = rng.Randf();

//         if (randomValue > 0.75f)
//         {
//             if (randomValue < 0.85f)
//             {
//                 SpawnEntity(_badBlockScene, viewport, onBadBlockClicked);
//             }
//             else
//             {
//                 SpawnEntity(_prizeBoxScene, viewport, onPrizeBoxClicked);
//             }
//         }
//         else
//         {
//             SpawnEntity(_squareScene, viewport, onSquareClicked);
//         }
//     }

//     private void SpawnEntity(PackedScene scene, Viewport viewport, Action onClicked)
//     {
//         var entity = scene.Instantiate<SquareEntity>();
//         AddChild(entity);

//         _missedClickHandler.RegisterSquareEntity(entity);
//         entity.Clicked += onClicked;

//         _squareSpawnCount++;

//         var viewportRect = viewport.GetVisibleRect();
//         var rng = new RandomNumberGenerator();
//         float x = rng.RandfRange(viewportRect.Position.X + 25, viewportRect.Position.X + viewportRect.Size.X - entity.Size().X - 25);
//         float y = rng.RandfRange(viewportRect.Position.Y + 25, viewportRect.Position.Y + viewportRect.Size.Y - entity.Size().Y - 25);

//         entity.Position = new Godot.Vector2(x, y);
//     }
// }
