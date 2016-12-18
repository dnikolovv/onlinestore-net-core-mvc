namespace OnlineStore.IntegrationTests
{
    using Fixie;
    using System;

    public class DeleteData : FixtureBehavior, ClassBehavior
    {
        public void Execute(Class context, Action next)
        {
            SliceFixture.ResetCheckpoint();
            next();
        }

        public void Execute(Fixture context, Action next)
        {
            SliceFixture.ResetCheckpoint();
            next();
        }
    }
}