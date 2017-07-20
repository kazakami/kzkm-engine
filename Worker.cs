
namespace KzkmEngine
{
    abstract class Worker
    {
        public bool isAlive {get; private set;} = true;
        public int priority {get; private set;} = 0;
        public abstract void Update();
        public abstract void Draw();
    }
}